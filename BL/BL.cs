using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
using IDAL;


namespace IBL
{
    public partial class BL : IBL
    {
        public List<DroneToList> ListDrones = new List<DroneToList>();

        IDal dal = new DalObject.DalObject();

        Random rand = new Random(DateTime.Now.Millisecond);

        public BL()
        {
            // km per hour
            double[] powerConsumption = dal.GetRequestPowerConsumption();
            double dAvailable = powerConsumption[0];
            double dLightW = powerConsumption[1];
            double dMediumW = powerConsumption[2];
            double dHeavyW = powerConsumption[3];
            double chargingRateOfDrone = powerConsumption[4];

            IEnumerable<IDAL.DO.Drone> listDronesIdalDo = dal.GetDrones();
            DroneToList newDrone = new DroneToList();
            foreach (var elementDrone in listDronesIdalDo)
            {
                newDrone.Id = elementDrone.Id;
                newDrone.Model = elementDrone.Model;
                newDrone.Weight = Enum.Parse<WeightCategories>(elementDrone.Weight.ToString());
                ListDrones.Add(newDrone);
            }

            IEnumerable<IDAL.DO.Parcel> ListParcelsIdalDo = dal.GetParcels();

            foreach (var elementDrone in ListDrones)
            {
                foreach (var elementParcel in ListParcelsIdalDo)
                {
                    if (elementDrone.Id == elementParcel.DroneId && elementParcel.Delivered == DateTime.MinValue)
                    {
                        elementDrone.Status = DroneStatuses.Delivery;
                        if (elementParcel.Scheduled != DateTime.MinValue && elementParcel.PickedUp == DateTime.MinValue)
                            elementDrone.Location = NearStationToCustomer(dal.GetCustomer(elementParcel.SenderId),
                                dal.GetStations());
                        if (elementParcel.Delivered == DateTime.MinValue && elementParcel.PickedUp != DateTime.MinValue)
                            elementDrone.Location = new Location()
                            {
                                Longitude = dal.GetCustomer(elementParcel.SenderId).Longitude,
                                Latitude = dal.GetCustomer(elementParcel.SenderId).Latitude
                            }; //the location of the customer

                        Location targetLocation = new Location()
                        {
                            Longitude = dal.GetCustomer(elementParcel.TargetId).Longitude,
                            Latitude = dal.GetCustomer(elementParcel.TargetId).Latitude
                        };

                        double distanceDelivery = Distance(elementDrone.Location, targetLocation); // the distance between the drone and the target
                        if (elementParcel.Weight == IDAL.DO.WeightCategories.Heavy)
                            distanceDelivery *= dHeavyW;
                        if (elementParcel.Weight == IDAL.DO.WeightCategories.Medium)
                            distanceDelivery *= dMediumW;
                        if (elementParcel.Weight == IDAL.DO.WeightCategories.Light)
                            distanceDelivery *= dLightW;

                        distanceDelivery += Distance(targetLocation,
                            NearStationToCustomer(dal.GetCustomer(elementParcel.TargetId), dal.GetStations())) * dAvailable;

                        elementDrone.Battery = rand.Next((int)distanceDelivery, 100);

                        elementDrone.IdParcel = elementParcel.Id;
                    }

                    if ((elementDrone.Status != DroneStatuses.Delivery))
                        elementDrone.Status = (DroneStatuses) rand.Next(0, 1);

                    if (elementDrone.Status == DroneStatuses.Maintenance)
                    {
                        IEnumerable<IDAL.DO.Station> listStationsIdalDo = dal.GetStations();
                        int index = rand.Next(0, listStationsIdalDo.Count());
                        elementDrone.Location = new Location()
                        {
                            Longitude = listStationsIdalDo.ElementAt(index).Longitude,
                            Latitude = listStationsIdalDo.ElementAt(index).Latitude
                        };

                        elementDrone.Battery = rand.Next(0, 20);
                    }

                    if (elementDrone.Status == DroneStatuses.Available)
                    {
                        IEnumerable<IDAL.DO.Customer> customersWithDelivery =
                            ListCustomersWithDelivery(dal.GetCustomers(), dal.GetParcels());
                        int index = rand.Next(0, customersWithDelivery.Count());
                        elementDrone.Location = new Location()
                        {
                            Longitude = customersWithDelivery.ElementAt(index).Longitude,
                            Latitude = customersWithDelivery.ElementAt(index).Latitude
                        };

                        double distanceFromNearStation = Distance(elementDrone.Location,
                            NearStationToDrone(elementDrone.Location, dal.GetStations()));

                        distanceFromNearStation *= dAvailable;

                        elementDrone.Battery = rand.Next((int) distanceFromNearStation, 100);
                    }
                }
            }
        }

        public Location NearStationToCustomer(IDAL.DO.Customer customer, IEnumerable<IDAL.DO.Station> stations)
        {
            List<double> distancesList = new List<double>();
            List<Location> locationsList = new List<Location>();
            Location stationLocation = new Location(),
                customerLocation = new Location() {Longitude = customer.Longitude, Latitude = customer.Latitude};

            foreach (var station in stations)
            {
                stationLocation = new Location() {Longitude = station.Longitude, Latitude = station.Latitude};
                distancesList.Add(Distance(stationLocation, customerLocation));
                locationsList.Add(stationLocation);
            }

            double minDistance = distancesList.Min();
            Location nearLocation = new Location();
            foreach (var station in stations)
            {
                stationLocation = new Location() {Longitude = station.Longitude, Latitude = station.Latitude};
                if (minDistance == Distance(stationLocation, customerLocation))
                    nearLocation = stationLocation;
            }

            return nearLocation;
        }

        public double Distance(Location from, Location to)
        {
            int R = 6371 * 1000; // metres -- radius of the earth
            double phi1 = from.Latitude * Math.PI / 180; // φ, λ in radians
            double phi2 = to.Latitude * Math.PI / 180;
            double deltaPhi = (to.Latitude - from.Latitude) * Math.PI / 180;
            double deltaLambda = (to.Longitude - from.Longitude) * Math.PI / 180;

            double a = Math.Sin(deltaPhi / 2) * Math.Sin(deltaPhi / 2) +
                       Math.Cos(phi1) * Math.Cos(phi2) *
                       Math.Sin(deltaLambda / 2) * Math.Sin(deltaLambda / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double d = R * c / 1000; // in kilometres
            return d;
        }

        public IEnumerable<IDAL.DO.Customer> ListCustomersWithDelivery(IEnumerable<IDAL.DO.Customer> customers,
            IEnumerable<IDAL.DO.Parcel> Parcels)
        {
            List<IDAL.DO.Customer> newCustomers = new List<IDAL.DO.Customer>();
            foreach (var elementCustomer in customers)
            {
                foreach (var elementParcel in Parcels)
                {
                    if (elementParcel.TargetId == elementCustomer.Id && elementParcel.Delivered != DateTime.MinValue)
                        newCustomers.Add(elementCustomer);
                }
            }

            return newCustomers;
        }

        public Location NearStationToDrone(Location droneLocation, IEnumerable<IDAL.DO.Station> stations)
        {
            List<double> distancesList = new List<double>();
            List<Location> locationsList = new List<Location>();
            Location stationLocation = new Location();

            foreach (var station in stations)
            {
                stationLocation = new Location() {Longitude = station.Longitude, Latitude = station.Latitude};
                distancesList.Add(Distance(stationLocation, droneLocation));
                locationsList.Add(stationLocation);
            }

            double minDistance = distancesList.Min();
            Location nearLocation = new Location();
            foreach (var station in stations)
            {
                stationLocation = new Location() {Longitude = station.Longitude, Latitude = station.Latitude};
                if (minDistance == Distance(stationLocation, droneLocation))
                    nearLocation = stationLocation;
            }

            return nearLocation;
        }
    }
}


//void f()
//{
//    IEnumerable<IDAL.DO.Station> ListStations = dal.GetStations();
//    var updateDrones = from drone in ListDrones
//        from parcel in ListParcels
//        where drone.Id == parcel.DroneId && parcel.Delivered == DateTime.MinValue &&
//              parcel.Scheduled != DateTime.MinValue && parcel.PickedUp == DateTime.MinValue
//        select new BO.Drone()
//        {
//            Id = drone.Id,
//            Model = drone.Model,
//            Weight = Enum.Parse<WeightCategories>(drone.Weight.ToString()),
//            Location = NearStationToCustomer(dal.GetCustomer(parcel.SenderId), dal.GetStations())
//        };

//    updateDrones.ToList().ForEach(UpdateDrone);
//}

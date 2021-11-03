using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
using IDAL;


namespace IBL
{
    public partial class BL : IBL
    {
        public List<Drone> ListDrones = new List<Drone>();

        public BL()
        {
            IDal dal = new DalObject.DalObject();

            double[] powerConsumption = dal.GetRequestPowerConsumption();
            double dAvailable = powerConsumption[0];
            double dLightW = powerConsumption[1];
            double dMediumW = powerConsumption[2];
            double dHeavyW = powerConsumption[3];
            double chargingRateOfDrone = powerConsumption[4]; //Percent per hour

            IEnumerable<IDAL.DO.Drone> listDronesIdalDo = dal.GetDrones();
            Drone newDrone = new Drone();
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
                        double distanceDelivery =
                            Distance(elementDrone.Location,
                                targetLocation); // the distance between the drone and the target
                        distanceDelivery = distanceDelivery + Distance(targetLocation,
                            NearStationToCustomer(dal.GetCustomer(elementParcel.TargetId),
                                dal.GetStations())); // add the distance of the target from the station

                        // צריך לחשב אחוזי טעינה מינימלי... נתון המרחק


                        // לעדכן את השדה האחרון שך הרחפן של החבילה....
                    }

                    Random rand = new Random(DateTime.Now.Millisecond);
                    if (elementDrone.DeliveryByTransfer.Id == 0)
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

                        double distanceStationCharge = Distance(elementDrone.Location,
                            NearStationToDroneToCharge(elementDrone.Location, dal.GetStations()));

                        // צריך לחשב אחוזי טעינה מינימלי... נתון המרחק

                        // לבדוק עם יאיר שהכל תקין מבחינת הכללים
                    }
                }
            }
        }

        Location NearStationToCustomer(IDAL.DO.Customer customer, IEnumerable<IDAL.DO.Station> stations)
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

        double Distance(Location location1, Location location2)
        {
            return Math.Pow(
                Math.Pow(location1.Longitude - location2.Longitude, 2) +
                Math.Pow(location1.Latitude - location2.Latitude, 2), 1 / 2);
        }

        IEnumerable<IDAL.DO.Customer> ListCustomersWithDelivery(IEnumerable<IDAL.DO.Customer> customers,
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

        Location NearStationToDroneToCharge(Location droneLocation, IEnumerable<IDAL.DO.Station> stations)
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

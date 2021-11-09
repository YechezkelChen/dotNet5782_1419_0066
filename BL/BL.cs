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
                        elementDrone.Statuse = DroneStatuses.Delivery;
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

                    if ((elementDrone.Statuse != DroneStatuses.Delivery))
                        elementDrone.Statuse = (DroneStatuses) rand.Next(0, 1);

                    if (elementDrone.Statuse == DroneStatuses.Maintenance)
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

                    if (elementDrone.Statuse == DroneStatuses.Available)
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

        public void AddStation(Station newStation)
        {
            IDAL.DO.Station station = new IDAL.DO.Station();
            station.Id = newStation.Id;
            station.Name = newStation.Name;
            station.Longitude = newStation.Location.Longitude;
            station.Latitude = newStation.Location.Latitude;
            station.ChargeSlots = newStation.ChargeSlots;
            dal.AddStation(station);
        }

        public void AddDrone(Drone newDrone, int idStation)
        {
            IDAL.DO.Drone drone = new IDAL.DO.Drone();
            DroneToList newDroneToList = new DroneToList();

            drone.Id = newDrone.Id;
            drone.Model = newDrone.Model;
            drone.Weight = (IDAL.DO.WeightCategories)newDrone.Weight;
            newDrone.Battery = rand.Next(20, 40);
            newDrone.Status = DroneStatuses.Maintenance;
            newDrone.Location.Longitude = dal.GetStation(idStation).Longitude;
            newDrone.Location.Latitude = dal.GetStation(idStation).Latitude;

            newDroneToList.Id = newDrone.Id;
            newDroneToList.Model = newDrone.Model;
            newDroneToList.Weight= newDrone.Weight;
            newDroneToList.Battery= rand.Next(20, 40);
            newDroneToList.Statuse= DroneStatuses.Maintenance;
            newDroneToList.Location.Longitude= dal.GetStation(idStation).Longitude;
            newDroneToList.Location.Latitude = dal.GetStation(idStation).Latitude;
            int foundDrone = dal.CheckDroneAndParcel(newDroneToList.Id, dal.GetParcels());
            if (foundDrone != -1)//else we not initialized 
                newDroneToList.IdParcel = foundDrone;
            
            ListDrones.Add(newDroneToList);//לשאול את יאיר איך ההסופה עצמה מתבצעת
            dal.AddDrone(drone);
        }

        public void AddCustomer(Customer newCustomer)
        {
            IDAL.DO.Customer customer = new IDAL.DO.Customer();
            customer.Id = newCustomer.Id;
            customer.Name = newCustomer.Name;
            customer.Phone = newCustomer.Phone;
            customer.Longitude = newCustomer.Location.Longitude;
            customer.Latitude = newCustomer.Location.Latitude;
            dal.AddCustomer(customer);
        }

        public void AddParcel(Parcel newParcel)
        {
            IDAL.DO.Parcel parcel = new IDAL.DO.Parcel();
            parcel.SenderId = newParcel.SenderId;
            parcel.TargetId = newParcel.TargetId;
            parcel.Weight = (IDAL.DO.WeightCategories) newParcel.Weight;
            parcel.Priority = (IDAL.DO.Priorities) newParcel.Priority;
            parcel.DroneId = 0;
            parcel.Requested = DateTime.Now;
            parcel.Scheduled = DateTime.MinValue;
            parcel.PickedUp = DateTime.MinValue;
            parcel.Delivered = DateTime.MinValue;
            dal.AddParcel(parcel);
        }

        public IDAL.DO.Station GetStation(int id)
        {
            IDAL.DO.Station idalStation = new IDAL.DO.Station();
            try
            {
                idalStation = dal.GetStation(id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return idalStation;
        }

        public IDAL.DO.Drone GetDrone(int id)
        {
            IDAL.DO.Drone idalDrone = new IDAL.DO.Drone();
            try
            {
                idalDrone = dal.GetDrone(id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return idalDrone;
        }

        public IDAL.DO.Customer GetCustomer(int id)
        {
            IDAL.DO.Customer idalCustomer = new IDAL.DO.Customer();
            try
            {
                idalCustomer = dal.GetCustomer(id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return idalCustomer;
        }

        public IDAL.DO.Parcel GetParcel(int id)
        {
            IDAL.DO.Parcel idalParcel = new IDAL.DO.Parcel();
            try
            {
                idalParcel = dal.GetParcel(id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return idalParcel;
        }

        public void PrintStations()
        {
            foreach (IDAL.DO.Station elementStation in dal.GetStations())
                Console.WriteLine(elementStation.ToString());
        }

        public void PrintDrones()
        {
            foreach (IDAL.DO.Drone elementDrone in dal.GetDrones())
                Console.WriteLine(elementDrone.ToString());
        }

        public void PrintCustomers()
        {
            foreach (IDAL.DO.Customer elementCustomer in dal.GetCustomers())
                Console.WriteLine(elementCustomer.ToString());
        }

        public void PrintParcels()
        {
            foreach (IDAL.DO.Parcel elementParcel in dal.GetParcels())
                Console.WriteLine(elementParcel.ToString());
        }

        public void PrintParcelsNoDrones()
        {
            foreach (IDAL.DO.Parcel elementParcel in dal.GetParcels())
                if (elementParcel.DroneId == -1)//the Id drone is not exist
                    Console.WriteLine(elementParcel.ToString());
        }

        public void PrintStationsCharge()
        {
            foreach (IDAL.DO.Station elementStation in dal.GetStations())
                if (elementStation.ChargeSlots > 0)
                    Console.WriteLine(elementStation.ToString());
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

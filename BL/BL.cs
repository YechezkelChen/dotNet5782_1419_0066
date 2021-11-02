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
        public IEnumerable<IDAL.DO.Drone> ListDrones = new List<IDAL.DO.Drone>();

      

        public BL()
        {
            IDal dal = new DalObject.DalObject();

            double[] powerConsumption = dal.GetRequestPowerConsumption();
            double dAvailable = powerConsumption[0];
            double dLightW = powerConsumption[1];
            double dMediumW = powerConsumption[2];
            double dHeavyW = powerConsumption[3];
            double chargingRateOfDrone = powerConsumption[4]; //Percent per hour

            IEnumerable<IDAL.DO.Parcel> ListParcels = dal.GetParcels();
            IEnumerable<IDAL.DO.Station> ListStations = dal.GetStations();

            ListDrones = dal.GetDrones();
            

            var updateDrones = from drone in ListDrones
                from parcel in ListParcels
                where drone.Id == parcel.DroneId && parcel.Delivered == DateTime.MinValue &&
                      parcel.Scheduled != DateTime.MinValue && parcel.PickedUp == DateTime.MinValue
                select new BO.Drone()
                {
                    Id = drone.Id,
                    Model = drone.Model,
                    Weight = Enum.Parse<WeightCategories>(drone.Weight.ToString()),
                    Location = NearStationToCustomer(dal.GetCustomer(parcel.SenderId),dal.GetStations())
                };

                


                updateDrones = from drone in ListDrones
                from parcel in ListParcels
                where drone.Id == parcel.DroneId && parcel.Delivered == DateTime.MinValue &&
                      parcel.PickedUp != DateTime.MinValue
                select new BO.Drone()
                {
                    Id = drone.Id,
                    Model = drone.Model,
                    Weight = Enum.Parse<WeightCategories>(drone.Weight.ToString()),
                    Location = new Location(){ Longitude = dal.GetCustomer(parcel.SenderId).Longitude,
                                               Latitude = dal.GetCustomer(parcel.SenderId).Latitude },//the location of the customer
                    Battery = 
                };
            updateDrones.ToList().ForEach(UpdateDrone);


        }

        void UpdateDrone(BO.Drone drone)
        {
            drone.Status = DroneStatuses.Delivery;
        }

        double Distance(Location location1, Location location2)
        {
            return Math.Pow(
                Math.Pow(location1.Longitude - location2.Longitude, 2) +
                Math.Pow(location1.Latitude - location2.Latitude, 2), 1 / 2);
        }

        Location NearStationToCustomer(IDAL.DO.Customer customer, IEnumerable<IDAL.DO.Station> stations)
        {
            var newStation = from station in stations
                select new BO.Station()
                {
                    Id = station.Id,
                    Name = station.Name,
                    Location = new Location(){ Longitude = station.Longitude, Latitude = station.Latitude },
                    ChargeSlots = station.ChargeSlots
                };
            double minDistance = 9999999999, tmp;
            Location nearLocation = new Location(),
                customerLocation = new Location() {Longitude = customer.Longitude, Latitude = customer.Latitude};
            foreach (var station in newStation)
            {
                tmp = Distance(station.Location, customerLocation);
                if (tmp < minDistance)
                {
                    minDistance = tmp;
                    nearLocation = station.Location;
                }
            }
            return nearLocation;
        }

        Location NearStationToDrone(Drone drone, IEnumerable<IDAL.DO.Station> stations)
        {
            var newStation = from station in stations
                select new BO.Station()
                {
                    Id = station.Id,
                    Name = station.Name,
                    Location = new Location() { Longitude = station.Longitude, Latitude = station.Latitude },
                    ChargeSlots = station.ChargeSlots
                };
            double minDistance = 9999999999, tmp;
            Location nearLocation = new Location(),
                droneLocation = new Location() {Latitude  = drone.Location.Latitude, Longitude = drone.Location.Longitude};
            foreach (var station in newStation)
            {
                tmp = Distance(station.Location, droneLocation);
                if (tmp < minDistance && station.ChargeSlots > 0) //ther are place to charge
                {
                    minDistance = tmp;
                    nearLocation = station.Location;
                }
            }
            return nearLocation;
        }

        Location NearStationToDelivery(Location deliveryLocation, IEnumerable<IDAL.DO.Station> stations)
        {
            var newStation = from station in stations
                select new BO.Station()
                {
                    Id = station.Id,
                    Name = station.Name,
                    Location = new Location() { Longitude = station.Longitude, Latitude = station.Latitude },
                    ChargeSlots = station.ChargeSlots
                };
            double minDistance = 9999999999, tmp;
            Location nearLocation = new Location(),
                droneLocation = new Location() { Latitude = drone.Location.Latitude, Longitude = drone.Location.Longitude };
            foreach (var station in newStation)
            {
                tmp = Distance(station.Location, droneLocation);
                if (tmp < minDistance && station.ChargeSlots > 0) //ther are place to charge
                {
                    minDistance = tmp;
                    nearLocation = station.Location;
                }
            }
            return nearLocation;
        }

    }
}

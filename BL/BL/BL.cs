using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using BO;


namespace BL
{
    public partial class BL : BlApi.IBL
    {
        List<DroneToList> ListDrones = new List<DroneToList>();

        DalApi.IDal dal;

        double BatteryAvailable, BatteryLightWeight, BatteryMediumWeight, BatteryHeavyWeight, ChargingRateOfDrone;

        Random rand = new Random(DateTime.Now.Millisecond);

        /// <summary>
        /// constractur of the BL
        /// </summary>
        /// <returns></no returns >
        public BL()
        {

            dal = new DO.DalFactory();

            // km per hour
            double[] powerConsumption = dal.GetRequestPowerConsumption();
            BatteryAvailable = powerConsumption[0];
            BatteryLightWeight = powerConsumption[1];
            BatteryMediumWeight = powerConsumption[2];
            BatteryHeavyWeight = powerConsumption[3];
            ChargingRateOfDrone = powerConsumption[4];

            IEnumerable<DO.Drone> listDronesIdalDo = dal.GetDrones(drone => true);
            foreach (var elementDrone in listDronesIdalDo)
            {
                DroneToList newDrone = new DroneToList();
                newDrone.Id = elementDrone.Id;
                newDrone.Model = elementDrone.Model;
                newDrone.Weight = (WeightCategories)elementDrone.Weight;
                ListDrones.Add(newDrone);
            }

            IEnumerable<DO.Parcel> ListParcelsIdalDo = dal.GetParcels(parcel => true);

            for (int i = 0; i < ListDrones.Count(); i++)
            {  
                DroneToList drone = new DroneToList();
                drone = ListDrones[i];
                foreach (var parcel in ListParcelsIdalDo)
                {
                    if (drone.Id == parcel.DroneId && parcel.Scheduled != null && parcel.Delivered == null) //if the parcel are not in deliver and the drone is connect
                    {
                        double batteryDelivery = 0;
                        if (parcel.PickedUp == null)
                        {
                            drone.Location = NearStationToCustomer(GetCustomer(parcel.SenderId)).Location;
                            batteryDelivery += Distance(drone.Location, GetCustomer(parcel.SenderId).Location) * BatteryAvailable;
                        }
                        else // if the parcel is pick up
                            drone.Location = new Location() { Longitude = dal.GetCustomer(parcel.SenderId).Longitude, Latitude = dal.GetCustomer(parcel.SenderId).Latitude }; //the location of the sender

                        // the distance between the sender and the target
                        if (parcel.Weight == DO.WeightCategories.Heavy)
                            batteryDelivery += Distance(GetCustomer(parcel.SenderId).Location, GetCustomer(parcel.TargetId).Location) * BatteryHeavyWeight;
                        if (parcel.Weight == DO.WeightCategories.Medium)
                            batteryDelivery += Distance(GetCustomer(parcel.SenderId).Location, GetCustomer(parcel.TargetId).Location) * BatteryMediumWeight;
                        if (parcel.Weight == DO.WeightCategories.Light)
                            batteryDelivery += Distance(GetCustomer(parcel.SenderId).Location, GetCustomer(parcel.TargetId).Location) * BatteryLightWeight;

                        batteryDelivery += Distance(GetCustomer(parcel.TargetId).Location, NearStationToCustomer(GetCustomer(parcel.TargetId)).Location) * BatteryAvailable;

                        drone.Battery = (100 - batteryDelivery) * rand.NextDouble() + batteryDelivery;
                        drone.Battery = (double)System.Math.Round(drone.Battery, 3);

                        drone.Status = DroneStatuses.Delivery;
                        drone.IdParcel = parcel.Id;
                    }
                }

                if (drone.Status != DroneStatuses.Delivery)
                    drone.Status = (DroneStatuses) rand.Next(0, 2);

                if (drone.Status == DroneStatuses.Maintenance)
                {
                    drone.Status = DroneStatuses.Available; // for the charge, after he will be in Maintenance.
                    IEnumerable<DO.Station> listStationsIdalDo = dal.GetStations(s => true);
                    int index = rand.Next(0, listStationsIdalDo.Count());
                    drone.Location = new Location() { Longitude = listStationsIdalDo.ElementAt(index).Longitude, Latitude = listStationsIdalDo.ElementAt(index).Latitude };

                    drone.Battery = 20 * rand.NextDouble();
                    drone.Battery = (double)System.Math.Round(drone.Battery, 3);

                    drone.IdParcel = 0;
                    try
                    {
                        SendDroneToDroneCharge(drone.Id);
                    }
                    catch (DroneException) // there is no throw, for the program continue the constructor.
                    { }
                }

                if (drone.Status == DroneStatuses.Available)
                {
                    IEnumerable<DO.Customer> customersWithDelivery = new List<DO.Customer>();
                    foreach (var parcel in dal.GetParcels(parcel => true))
                        customersWithDelivery = dal.GetCustomers(customer => customer.Id == parcel.TargetId && parcel.Delivered != null);

                    if (customersWithDelivery.Any())
                    {
                        int index = rand.Next(0, customersWithDelivery.Count());
                        drone.Location = new Location() { Longitude = customersWithDelivery.ElementAt(index).Longitude, Latitude = customersWithDelivery.ElementAt(index).Latitude };
                    }
                    else
                    {
                        int index = rand.Next(0, GetStations().Count());
                        Station station = GetStation(GetStations().ElementAt(index).Id);
                        drone.Location = new Location() { Longitude = station.Location.Longitude, Latitude = station.Location.Latitude }; // put in rand station
                    }

                    double batteryToNearStation = 0;

                    batteryToNearStation = Distance(drone.Location, NearStationToDrone(GetDrone(drone.Id)).Location) * BatteryAvailable;

                    drone.Battery = (100 - batteryToNearStation) * rand.NextDouble() + batteryToNearStation;
                    drone.Battery = (double)System.Math.Round(drone.Battery, 3);

                    drone.IdParcel = 0;
                }
                ListDrones[i] = drone;
            }
        }

        /// <summary>
        /// the distances from "FROM" to "TO"
        /// </summary>
        /// <returns></returns the distance on double type>
        private double Distance(Location from, Location to)
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
            double d = R * c / 1000; // in kilometers
            return d;
        }
    }
}
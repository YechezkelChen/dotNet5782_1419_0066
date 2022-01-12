using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BO;


namespace BL
{
    sealed partial class BL : BlApi.IBL
    {
        private List<DroneToList> listDrones = new List<DroneToList>();

        internal DalApi.IDal dal;

        private double freeBatteryUsing, lightBatteryUsing, mediumBatteryUsing, heavyBatteryUsing, chargingRate;

        private Random rand = new Random(DateTime.Now.Millisecond);

        #region singelton

        private static volatile Lazy<BL> instance = new Lazy<BL>(() => new BL());

        private static object syncRoot = new object();
        internal static BL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new Lazy<BL>(() => new BL());
                    }
                }

                return instance.Value;
            }
        }
        static BL() { }

        /// <summary>
        /// constractur of the BL
        /// </summary>
        /// <returns></no returns >
        BL()
        {
            dal = DalApi.DalFactory.GetDal();

            // km per hour
            double[] powerConsumption = dal.GetRequestPowerConsumption();
            freeBatteryUsing = powerConsumption[0];
            lightBatteryUsing = powerConsumption[1];
            mediumBatteryUsing = powerConsumption[2];
            heavyBatteryUsing = powerConsumption[3];
            chargingRate = powerConsumption[4];

            foreach (var elementDrone in dal.GetDrones(drone => drone.Deleted == false))
            {
                DroneToList newDrone = new DroneToList();
                newDrone.Id = elementDrone.Id;
                newDrone.Model = elementDrone.Model;
                newDrone.Weight = (WeightCategories)elementDrone.Weight;
                newDrone.Location = new Location();
                listDrones.Add(newDrone);
            }

            foreach (var parcel in dal.GetParcels(parcel => parcel.Deleted == false))
                if (parcel.Scheduled != null && parcel.Delivered == null) //if the parcel in deliver and the drone is connect
                    for (int i = 0; i < listDrones.Count(); i++)
                        if (listDrones[i].Id == parcel.DroneId)
                        {
                            DroneToList drone = new DroneToList();
                            drone = listDrones[i];
                            double batteryDelivery = 0;
                            Customer sender = GetCustomer(parcel.SenderId);
                            Customer target = GetCustomer(parcel.TargetId);
                            StationToList nearStationToSender = GetStations().OrderByDescending(station => Distance(sender.Location, GetStation(station.Id).Location)).First();
                            StationToList nearStationToTarget = GetStations().OrderByDescending(station => Distance(target.Location, GetStation(station.Id).Location)).First();

                            if (parcel.PickedUp == null)
                            {
                                drone.Location = GetStation(nearStationToSender.Id).Location;
                                batteryDelivery += Distance(drone.Location, sender.Location) * freeBatteryUsing;
                            }
                            else // if the parcel is pick up
                                drone.Location = new Location() { Longitude = sender.Location.Longitude, Latitude = sender.Location.Latitude };

                            // the distance between the sender and the target
                            if (parcel.Weight == DO.WeightCategories.Heavy)
                                batteryDelivery += Distance(sender.Location, target.Location) * heavyBatteryUsing;
                            if (parcel.Weight == DO.WeightCategories.Medium)
                                batteryDelivery += Distance(sender.Location, target.Location) * mediumBatteryUsing;
                            if (parcel.Weight == DO.WeightCategories.Light)
                                batteryDelivery += Distance(sender.Location, target.Location) * lightBatteryUsing;

                            batteryDelivery += Distance(target.Location, GetStation(nearStationToTarget.Id).Location) * freeBatteryUsing;

                            drone.Battery = (100 - batteryDelivery) * rand.NextDouble() + batteryDelivery;
                            drone.Battery = (double)System.Math.Round(drone.Battery, 2);

                            drone.Status = DroneStatuses.Delivery;
                            drone.IdParcel = parcel.Id;
                            drone = listDrones[i];
                        }

            foreach (var droneCharge in dal.GetDronesCharge(droneCharge => droneCharge.Deleted == false))
                for (int i = 0; i < listDrones.Count(); i++)
                    if (listDrones[i].Id == droneCharge.DroneId)
                    {
                        DroneToList drone = new DroneToList();
                        drone = listDrones[i];

                        DO.Station station = dal.GetStation(droneCharge.StationId);
                        drone.Location = new Location() { Longitude = station.Longitude, Latitude = station.Latitude };

                        drone.Battery = 20 * rand.NextDouble();
                        drone.Battery = (double)System.Math.Round(drone.Battery, 2);

                        drone.Status = DroneStatuses.Maintenance;
                        drone.IdParcel = 0;
                    }

            for (int i = 0; i < listDrones.Count(); i++)
            {
                DroneToList drone = new DroneToList();
                drone = listDrones[i];

                // For the first run to keep the drone in Maintenance to drone charge.

                //if (drone.Status != DroneStatuses.Delivery)
                //    drone.Status = (DroneStatuses)rand.Next(0, 2);

                //if (drone.Status == DroneStatuses.Maintenance)
                //{
                //    IEnumerable<DO.Station> stationList = dal.GetStations(station =>
                //        station.Deleted == false && station.AvailableChargeSlots > 0);
                //    int index = rand.Next(0, stationList.Count());
                //    DO.Station station = stationList.ElementAt(index);
                //    drone.Location = new Location() { Longitude = station.Longitude, Latitude = station.Latitude };

                //    drone.Battery = 20 * rand.NextDouble();
                //    drone.Battery = (double)System.Math.Round(drone.Battery, 2);
                //    drone.IdParcel = 0;

                //    // Update the station
                //    station.AvailableChargeSlots--;
                //    dal.UpdateStation(station);

                //    // Add drone charge
                //    DO.DroneCharge droneCharge = new DO.DroneCharge();
                //    droneCharge.StationId = station.Id;
                //    droneCharge.DroneId = drone.Id;
                //    droneCharge.StartCharging = DateTime.Now;
                //    dal.AddDroneCharge(droneCharge);
                //}

                if (drone.Status == DroneStatuses.Available)
                {
                    var customersWithDelivery = from parcel in dal.GetParcels(parcel => parcel.Deleted == false)
                                                from customer in dal.GetCustomers(customer => customer.Deleted == false)
                                                where customer.Id == parcel.TargetId && parcel.Delivered != null
                                                select customer;

                    if (customersWithDelivery.Any())
                    {
                        int index = rand.Next(customersWithDelivery.Count());
                        DO.Customer customer = customersWithDelivery.ElementAt(index);
                        drone.Location = new Location() { Longitude = customer.Longitude, Latitude = customer.Latitude };
                    }
                    else
                    {
                        int index = rand.Next(GetStations().Count());
                        Station station = GetStation(GetStations().ElementAt(index).Id);
                        drone.Location = new Location() { Longitude = station.Location.Longitude, Latitude = station.Location.Latitude }; // put in rand station
                    }

                    double batteryToNearStation = 0;
                    StationToList nearStation = GetStationsWithAvailableCharge().OrderByDescending(station => Distance(GetStation(station.Id).Location, drone.Location)).FirstOrDefault();
                    batteryToNearStation = Distance(drone.Location, GetStation(nearStation.Id).Location) * freeBatteryUsing;

                    drone.Battery = (100 - batteryToNearStation) * rand.NextDouble() + batteryToNearStation;
                    drone.Battery = (double)System.Math.Round(drone.Battery, 2);
                    drone.IdParcel = 0;
                }
                listDrones[i] = drone;
            }
        }

        #endregion

        public void CopyPropertiesTo<T, S>(S from, T to)
        {
            foreach (PropertyInfo propTo in to.GetType().GetProperties())
            {
                PropertyInfo propFrom = typeof(S).GetProperty(propTo.Name);
                if (propFrom == null)
                    continue;
                var value = propFrom.GetValue(from, null);
                if (value is ValueType || value is string)
                    propTo.SetValue(to, value);
            }
        }

        public void SimulatorMod(int droneId, Action updateView, Func<bool> stopSimulator)
        {
            try
            {
                new Simulator(this, droneId, updateView, stopSimulator);
            }
            catch (NoParcelsToDroneException ex)
            {
                throw new NoParcelsToDroneException(ex.Message);
            }
        }

        /// <summary>
        /// the distances from "FROM" to "TO"
        /// </summary>
        /// <returns></returns the distance on double type>
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
            double d = R * c / 1000; // in kilometers
            return d;
        }
    }
}
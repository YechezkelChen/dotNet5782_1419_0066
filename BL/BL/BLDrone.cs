using System;
using System.Collections.Generic;
using System.Linq;
using BO;
using System.Runtime.CompilerServices;
using BlApi;

namespace BL
{
    partial class BL : BlApi.IBL
    {
        /// <summary>
        /// add a drone
        /// </summary>
        /// <returns></no returns, add a drone>
       
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddDrone(Drone newDrone, int idStation)
        {
            Station station = new Station();
            try
            {
                station = CheckDroneAndStationCharge(newDrone, idStation); // check the input of the user
            }
            catch (IdException e)
            {
                throw new IdException(e.Message, e);
            }
            catch(ModelException e)
            {
                throw new ModelException(e.Message, e);
            }
            catch(ChargeSlotsException e)
            {
                throw new ChargeSlotsException(e.Message, e);
            }

            // For the list of drones here
            DroneToList droneToList = new DroneToList();
            droneToList.Id = newDrone.Id;
            droneToList.Model = newDrone.Model;
            droneToList.Weight = newDrone.Weight;
            droneToList.Battery = 20 * rand.NextDouble() + 20;
            droneToList.Battery = (double)System.Math.Round(droneToList.Battery, 2);
            droneToList.Status = DroneStatuses.Maintenance;
            droneToList.Location = station.Location;
            droneToList.IdParcel = 0;
            listDrones.Add(droneToList);

            lock (dal)
            {
                // For dal
                DO.Drone drone = new DO.Drone();
                drone.Id = newDrone.Id;
                drone.Model = newDrone.Model;
                drone.Weight = (DO.WeightCategories) newDrone.Weight;
                drone.Deleted = false;
                try
                {
                    dal.AddDrone(drone); // add the drone just if the drone not in the dataSource
                }
                catch (DO.IdExistException e)
                {
                    throw new IdException(e.Message, e);
                }

                // Update the station
                DO.Station updateStation = dal.GetStation(station.Id);
                updateStation.AvailableChargeSlots--;
                dal.UpdateStation(updateStation);

                // Add drone charge
                DO.DroneCharge droneCharge = new DO.DroneCharge();
                droneCharge.StationId = station.Id;
                droneCharge.DroneId = newDrone.Id;
                droneCharge.StartCharging = DateTime.Now;
                try
                {
                    dal.AddDroneCharge(droneCharge);
                }
                catch (DO.IdExistException e)
                {
                    throw new IdException(e.Message, e);
                }
            }
        }

        /// <summary>
        /// Removes a drone from the list of drones.
        /// </summary>
        /// <param name="droneId"></param>
      
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void RemoveDrone(int droneId)
        {
            lock (dal)
            {
                try
                {
                    dal.RemoveDrone(droneId); // Remove the drone
                }
                catch (DO.IdExistException e)
                {
                    throw new IdException(e.Message, e);
                }
                catch (DO.IdNotFoundException e)
                {
                    throw new IdException(e.Message, e);
                }
            }
        }

        /// <summary>
        /// get a drone
        /// </summary>
        /// <returns></return the drone>
      
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Drone GetDrone(int droneId)
        {
            Drone drone = new Drone();

            DroneToList droneToList = listDrones.Find(drone => drone.Id == droneId);
            if (droneToList is null)
                throw new IdException("ERROR: the drone is not found.");

            drone.Id = droneToList.Id;
            drone.Model = droneToList.Model;
            drone.Weight = droneToList.Weight;
            drone.Battery = droneToList.Battery;
            drone.Status = droneToList.Status;
            drone.Location = new Location() { Longitude = droneToList.Location.Longitude, Latitude = droneToList.Location.Latitude };

            if (drone.Status != DroneStatuses.Delivery)
                return drone;

            lock (dal)
            {
                // If the drone in the delivery fill the parcel data
                DO.Parcel parcel = dal.GetParcel(droneToList.IdParcel);
                drone.ParcelByTransfer = new ParcelInTransfer();
                drone.ParcelByTransfer.Id = parcel.Id;

                if (parcel.PickedUp == null)
                    drone.ParcelByTransfer.OnTheWay = false;
                else // The parcel has been collected but not yet delivered
                    drone.ParcelByTransfer.OnTheWay = true;

                drone.ParcelByTransfer.Priority = (Priorities)parcel.Priority;
                drone.ParcelByTransfer.Weight = (WeightCategories)parcel.Weight;

                DO.Customer customer = dal.GetCustomer(parcel.SenderId);
                drone.ParcelByTransfer.Sender = new CustomerInParcel() { Id = customer.Id, Name = customer.Name };
                drone.ParcelByTransfer.PickUpLocation = new Location() { Longitude = customer.Longitude, Latitude = customer.Latitude };

                customer = dal.GetCustomer(parcel.TargetId);
                drone.ParcelByTransfer.Target = new CustomerInParcel() { Id = customer.Id, Name = customer.Name };
                drone.ParcelByTransfer.TargetLocation = new Location() { Longitude = customer.Longitude, Latitude = customer.Latitude };

                if (drone.ParcelByTransfer.OnTheWay == false) // distance between drone to sender
                    drone.ParcelByTransfer.DistanceOfTransfer = Distance(drone.Location, drone.ParcelByTransfer.PickUpLocation);
                else  // distance between drone(and sender) to target
                    drone.ParcelByTransfer.DistanceOfTransfer = Distance(drone.Location, drone.ParcelByTransfer.TargetLocation);

                drone.ParcelByTransfer.DistanceOfTransfer = (double)System.Math.Round(drone.ParcelByTransfer.DistanceOfTransfer, 2);
            }

            return drone;
        }

        /// <summary>
        /// get a drones
        /// </summary>
        /// <returns></return all drones>
       
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<DroneToList> GetDrones()
        {
            IEnumerable<DroneToList> drones = listDrones;
            return drones;
        }

        /// <summary>
        /// get a drones with filtering of status
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
      
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<DroneToList> GetDronesByStatus(DroneStatuses status)
        {
            IEnumerable<DroneToList> drones = listDrones.FindAll(drone => drone.Status == status);
            return drones;
        }

        /// <summary>
        /// get a drones with filtering of max weight
        /// </summary>
        /// <param name="weight"></param>
        /// <returns></returns>
      
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<DroneToList> GetDronesByMaxWeight(WeightCategories weight)
        {
            IEnumerable<DroneToList> drones = listDrones.FindAll(drone => drone.Weight <= weight);
            return drones;
        }

        /// <summary>
        /// return the list after groping
        /// </summary>
        /// <returns></returns>
      
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<IGrouping<DroneStatuses, DroneToList>> GetDronesByGroupStatus()
        {
            IEnumerable<IGrouping<DroneStatuses, DroneToList>> drones = listDrones.GroupBy(drone => drone.Status);
            return drones;
        }

        /// <summary>
        /// Update the model of the drone
        /// </summary>
        /// <returns></no returns, update the model of the drone>5
       
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateDroneModel(int droneId, string newModel)
        {
            if (newModel == "") // if the user not put anything
                throw new ModelException("ERROR: Model must have value");

            lock (dal)
            {
                DO.Drone updateDrone = new DO.Drone();
                try
                {
                    updateDrone = dal.GetDrone(droneId);
                }
                catch (DO.IdNotFoundException e) // if the drone not exist
                {
                    throw new IdException(e.Message, e);
                }

                updateDrone.Model = newModel;
                // For dataSource
                dal.UpdateDrone(updateDrone);
            }

            // For the list of drones here
            for (int i = 0; i < listDrones.Count(); i++)
                if (listDrones[i].Id == droneId)
                {
                    DroneToList updateDroneToList = listDrones[i];
                    updateDroneToList.Model = newModel;
                    listDrones[i] = updateDroneToList;
                }
        }

        /// <summary>
        /// Send the drone to drone charge
        /// </summary>
        /// <returns></no returns, just send the drone to drone charge>

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void SendDroneToDroneCharge(int droneId)
        {
            Drone drone = new Drone();
            try
            {
                drone = GetDrone(droneId);
            }
            catch (IdException e) // if the drone not exist
            {
                throw new IdException(e.Message, e);
            }

            if (drone.Status != DroneStatuses.Available)
                throw new StatusDroneException("ERROR: the drone not available to charge ");
            
            StationToList nearStationToList = GetStationsWithAvailableCharge().OrderByDescending(station => Distance(GetStation(station.Id).Location, drone.Location)).FirstOrDefault();
            if(nearStationToList is null)
                throw new ChargeSlotsException("ERROR: there is no station with avalible charge ");
            
            Station nearStation = GetStation(nearStationToList.Id);
            double distance = Distance(drone.Location, nearStation.Location);
            if (distance * freeBatteryUsing > drone.Battery || distance * freeBatteryUsing > 100)
                throw new BatteryDroneException("ERROR: the drone not have battery to go to station charge ");

            for (int i = 0; i < listDrones.Count; i++)
                if (listDrones[i].Id == drone.Id)
                {
                    DroneToList newDrone = listDrones[i];
                    newDrone.Battery -= distance * freeBatteryUsing;
                    newDrone.Battery = (double)System.Math.Round(newDrone.Battery, 2);
                    newDrone.Location = nearStation.Location;
                    newDrone.Status = DroneStatuses.Maintenance;
                    listDrones[i] = newDrone;
                }

            lock (dal)
            {
                DO.Station updateStation = dal.GetStation(nearStation.Id);
                updateStation.AvailableChargeSlots--;
                dal.UpdateStation(updateStation);

                DO.DroneCharge newDroneCharge = new DO.DroneCharge();
                newDroneCharge.StationId = nearStation.Id;
                newDroneCharge.DroneId = drone.Id;
                newDroneCharge.StartCharging = DateTime.Now;
                try
                {
                    dal.AddDroneCharge(newDroneCharge);
                }
                catch (DO.IdExistException e)
                {
                    throw new IdException(e.Message, e);
                }
            }
        }

        /// <summary>
        /// Release the drone from the drone charge
        /// </summary>
        /// <returns></no returns, release the drone from the drone charge>
       
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void ReleaseDroneFromDroneCharge(int droneId)
        {
            Drone drone = new Drone();
            try
            {
                drone = GetDrone(droneId);
            }
            catch (IdException e)
            {
                throw new IdException(e.Message, e);
            }

            if (drone.Status != DroneStatuses.Maintenance)
                throw new StatusDroneException("The drone can not release because he isn't in maintenance status.\n");

            lock (dal)
            {
                DO.DroneCharge droneCharge = dal.GetDronesCharge(droneCharge => droneCharge.Deleted == false && droneCharge.DroneId == droneId).First();

                DO.Station updateStation = dal.GetStation(droneCharge.StationId);
                updateStation.AvailableChargeSlots++;
                dal.UpdateStation(updateStation);

                TimeSpan? chargeTime = DateTime.Now - droneCharge.StartCharging;
                double batteryCharge = chargeTime.Value.TotalSeconds * (chargingRate / 3600);

                for (int i = 0; i < listDrones.Count; i++)
                    if (listDrones[i].Id == drone.Id)
                    {
                        DroneToList newDrone = listDrones[i];
                        newDrone.Battery += batteryCharge;
                        newDrone.Battery = (double)System.Math.Round(newDrone.Battery, 2);
                        if (newDrone.Battery > 100)
                            newDrone.Battery = 100;
                        newDrone.Status = DroneStatuses.Available;
                        listDrones[i] = newDrone;
                    }

                dal.RemoveDroneCharge(droneCharge);
            }
        }

        /// <summary>
        /// find parcel in the data conditions and connect it to the drone
        /// </summary>
        /// <param name="droneId"></param>
       
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void ConnectParcelToDrone(int droneId)
        {
            Drone drone = new Drone();
            try
            {
                drone = GetDrone(droneId);
            }
            catch (IdException e)
            {
                throw new IdException(e.Message, e);
            }

            if (drone.Status != DroneStatuses.Available)
                throw new StatusDroneException("ERROR: The drone is not available:\n ");

            var x = GetParcelsNoDrones();
            var parcelsDroneCanCarry = from parcel in GetParcelsNoDrones()
                                       let sender = GetCustomer(GetParcel(parcel.Id).Sender.Id)
                                       let target = GetCustomer(GetParcel(parcel.Id).Target.Id)
                                       orderby parcel.Priority descending , parcel.Weight descending , Distance(drone.Location, sender.Location)
                                       where drone.Weight >= parcel.Weight && drone.Battery >= BatteryDelivery(drone, parcel, sender, target)
                                       select parcel;

            ParcelToList parcelToConnect = parcelsDroneCanCarry.FirstOrDefault();
            if (parcelToConnect is null)
                throw new NoPackagesToDroneException("There are no parcels that the drone you entered can carry.");

            lock (dal)
            {
                DO.Parcel updateParcel = updateParcel = dal.GetParcel(parcelToConnect.Id);
                updateParcel.DroneId = drone.Id;
                updateParcel.Scheduled = DateTime.Now;

                dal.UpdateParcel(updateParcel);

                for (int i = 0; i < listDrones.Count; i++)
                    if (listDrones[i].Id == drone.Id)
                    {
                        DroneToList newDrone = listDrones[i];
                        newDrone.Status = DroneStatuses.Delivery;
                        newDrone.IdParcel = updateParcel.Id;
                        listDrones[i] = newDrone;
                    }
            }
        }

        /// <summary>
        /// Collection the parcel by the drone
        /// </summary>
        /// <param name="droneId"></param>
       
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void CollectionParcelByDrone(int droneId)
        {
            Drone drone = new Drone();
            try
            {
                drone = GetDrone(droneId);
            }
            catch (IdException e)
            {
                throw new IdException(e.Message, e);
            }

            if (drone.Status != DroneStatuses.Delivery)
                throw new StatusDroneException("ERROR: The drone is not in delivery so he can not collect the parcel.");

            if(drone.ParcelByTransfer.OnTheWay == true)
                throw new StatusDroneException("ERROR: The drone has already collected the parcel");

            for (int i = 0; i < listDrones.Count(); i++)
            {
                if (listDrones[i].Id == drone.Id)
                {
                    DroneToList updateDrone = listDrones[i];
                    updateDrone.Battery -= Distance(drone.Location, drone.ParcelByTransfer.PickUpLocation) * freeBatteryUsing;
                    updateDrone.Battery = (double)System.Math.Round(updateDrone.Battery, 2);
                    updateDrone.Location = drone.ParcelByTransfer.PickUpLocation;
                    listDrones[i] = updateDrone;
                }
            }

            lock (dal)
            {
                DO.Parcel updateParcel = dal.GetParcel(drone.ParcelByTransfer.Id);
                updateParcel.PickedUp = DateTime.Now;
                dal.UpdateParcel(updateParcel);
            }
        }

        /// <summary>
        /// Supply parcel by drone
        /// </summary>
        /// <param name="droneId"></the id of the drone>
       
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void SupplyParcelByDrone(int droneId)
        {
            Drone drone = new Drone();
            try
            {
                drone = GetDrone(droneId);
            }
            catch (IdException e)
            {
                throw new IdException(e.Message, e);
            }

            if (drone.Status != DroneStatuses.Delivery)
                throw new StatusDroneException("ERROR: The drone is not in delivery so he can not supply the parcel.");

            if(drone.ParcelByTransfer.OnTheWay == false)
                throw new StatusDroneException("ERROR: The drone isn't collect the parcel.");

            lock (dal)
            {
                DO.Parcel updateParcel = dal.GetParcel(drone.ParcelByTransfer.Id);
                updateParcel.Delivered = DateTime.Now;
                dal.UpdateParcel(updateParcel);
            }
            
            for (int i = 0; i < listDrones.Count(); i++)
            {
                if (listDrones[i].Id == drone.Id)
                {
                    DroneToList newDrone = listDrones[i];
                    if (drone.ParcelByTransfer.Weight == WeightCategories.Heavy)
                        newDrone.Battery -= drone.ParcelByTransfer.DistanceOfTransfer * heavyBatteryUsing;
                    if (drone.ParcelByTransfer.Weight == WeightCategories.Medium)
                        newDrone.Battery -= drone.ParcelByTransfer.DistanceOfTransfer * mediumBatteryUsing;
                    if (drone.ParcelByTransfer.Weight == WeightCategories.Light)
                        newDrone.Battery -= drone.ParcelByTransfer.DistanceOfTransfer * lightBatteryUsing;
                    newDrone.Battery = (double)System.Math.Round(newDrone.Battery, 2);

                    newDrone.Location = drone.ParcelByTransfer.TargetLocation;
                    newDrone.Status = DroneStatuses.Available;
                    newDrone.IdParcel = 0;
                    listDrones[i] = newDrone;
                }
            }
        }

        /// <summary>
        /// Check the input of the user
        /// </summary>
        /// <returns></no returns, just check the input of the user>
        private Station CheckDroneAndStationCharge(Drone drone, int idStation)
        {
            if (drone.Id < 100000 || drone.Id > 999999)//Check that it's 6 digits
                throw new IdException("ERROR: the ID is illegal! ");
            if (drone.Model == "")
                throw new ModelException("ERROR: model must have value");

            Station station = new Station();
            try
            {
                station = GetStation(idStation); // try to find the station the user want to put the drone in charge
                if (station.AvailableChargeSlots == 0) // if the station that the user ask there is no place for charge
                    throw new ChargeSlotsException("ERROR: The station you ask not have more place.");
            }
            catch (IdException e)
            {
                throw new IdException(e.Message, e);
            }
            return station;
        }

        private double BatteryDelivery(Drone drone, ParcelToList parcel, Customer sender, Customer target)
        {
            double distanceDelivery, batteryDelivery;

            // From the location of drone to the pickup location
            distanceDelivery = Distance(drone.Location, sender.Location);
            batteryDelivery = distanceDelivery * freeBatteryUsing;

            // From the pickup location to the target location
            distanceDelivery = Distance(sender.Location, target.Location);
            if (parcel.Weight == WeightCategories.Heavy)
                batteryDelivery += distanceDelivery * heavyBatteryUsing;
            if (parcel.Weight == WeightCategories.Medium)
                batteryDelivery += distanceDelivery * mediumBatteryUsing;
            if (parcel.Weight == WeightCategories.Light)
                batteryDelivery += distanceDelivery * lightBatteryUsing;

            // From the target location to the station charge location
            StationToList nearStation = GetStationsWithAvailableCharge().OrderByDescending(station => Distance(target.Location, GetStation(station.Id).Location)).First();
            distanceDelivery = Distance(target.Location, GetStation(nearStation.Id).Location);
            batteryDelivery += distanceDelivery * freeBatteryUsing;

            return batteryDelivery;
        }
    }
}

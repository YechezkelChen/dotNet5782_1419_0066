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
    partial class BL : BlApi.IBL
    {
        /// <summary>
        /// add a drone
        /// </summary>
        /// <returns></no returns, add a drone>
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

            // For dataSource
            DO.Drone drone = new DO.Drone();
            drone.Id = newDrone.Id;
            drone.Model = newDrone.Model;
            drone.Weight = (DO.WeightCategories)newDrone.Weight;
            drone.Deleted = false;
            try
            {
                dal.AddDrone(drone); // add the drone just if the drone not in the dataSource
            }
            catch (DO.IdExistException e)
            {
                throw new IdException(e.Message, e);
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
            ListDrones.Add(droneToList);

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

        /// <summary>
        /// get a drone
        /// </summary>
        /// <returns></return the drone>
        public Drone GetDrone(int droneId)
        {
            Drone drone = new Drone();

            DroneToList droneToList = ListDrones.Find(drone => drone.Id == droneId);
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

            drone.ParcelByTransfer.DistanceOfTransfer = Distance(drone.ParcelByTransfer.PickUpLocation, drone.ParcelByTransfer.TargetLocation);
            drone.ParcelByTransfer.DistanceOfTransfer = (double)System.Math.Round(drone.ParcelByTransfer.DistanceOfTransfer, 2);

            return drone;
        }

        /// <summary>
        /// get a drones
        /// </summary>
        /// <returns></return all drones>
        public IEnumerable<DroneToList> GetDrones()
        {
            IEnumerable<DroneToList> drones = ListDrones;
            return drones;
        }

        /// <summary>
        /// get a drones with filtering of status
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public IEnumerable<DroneToList> GetDronesByStatus(DroneStatuses status)
        {
            IEnumerable<DroneToList> drones = ListDrones.FindAll(drone => drone.Status == status);
            return drones;
        }

        /// <summary>
        /// get a drones with filtering of max weight
        /// </summary>
        /// <param name="weight"></param>
        /// <returns></returns>
        public IEnumerable<DroneToList> GetDronesByMaxWeight(WeightCategories weight)
        {
            IEnumerable<DroneToList> drones = ListDrones.FindAll(drone => drone.Weight <= weight);
            return drones;
        }

        /// <summary>
        /// Update the model of the drone
        /// </summary>
        /// <returns></no returns, update the model of the drone>
        public void UpdateDroneModel(int droneId, string newModel)
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
            
            if (newModel == "") // if the user not put anything
                throw new ModelException("ERROR: Model must have value");

            updateDrone.Model = newModel;

            // For dataSource
            dal.UpdateDrone(updateDrone);

            // For the list of drones here
            for (int i = 0; i < ListDrones.Count(); i++)
                if (ListDrones[i].Id == droneId)
                {
                    DroneToList updateDroneToList = ListDrones[i];
                    updateDroneToList.Model = newModel;
                    ListDrones[i] = updateDroneToList;
                }
        }

        /// <summary>
        /// Send the drone to drone charge
        /// </summary>
        /// <returns></no returns, just send the drone to drone charge>
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
            if (distance * BatteryAvailable > drone.Battery || distance * BatteryAvailable > 100)
                throw new BatteryDroneException("ERROR: the drone not have battery to go to station charge ");

            for (int i = 0; i < ListDrones.Count; i++)
                if (ListDrones[i].Id == drone.Id)
                {
                    DroneToList newDrone = ListDrones[i];
                    newDrone.Battery -= distance * BatteryAvailable;
                    newDrone.Battery = (double)System.Math.Round(newDrone.Battery, 2);
                    newDrone.Location = nearStation.Location;
                    newDrone.Status = DroneStatuses.Maintenance;
                    ListDrones[i] = newDrone;
                }

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

        /// <summary>
        /// Release the drone from the drone charge
        /// </summary>
        /// <returns></no returns, release the drone from the drone charge>
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

            DO.DroneCharge droneCharge = dal.GetDronesCharge(droneCharge => droneCharge.Deleted == false && droneCharge.DroneId == droneId).First();
            
            DO.Station updateStation = dal.GetStation(droneCharge.StationId);
            updateStation.AvailableChargeSlots++;
            dal.UpdateStation(updateStation);

            TimeSpan? chargeTime = DateTime.Now - droneCharge.StartCharging;
            double batteryCharge = chargeTime.Value.TotalSeconds * (ChargingRateOfDrone / 3600);

            for (int i = 0; i < ListDrones.Count; i++)
                if (ListDrones[i].Id == drone.Id)
                {
                    DroneToList newDrone = ListDrones[i];
                    newDrone.Battery += batteryCharge;
                    newDrone.Battery = (double)System.Math.Round(newDrone.Battery, 2);
                    if (newDrone.Battery > 100)
                        newDrone.Battery = 100;
                    newDrone.Status = DroneStatuses.Available;
                    ListDrones[i] = newDrone;
                }

            dal.RemoveDroneCharge(droneCharge);
        }

        /// <summary>
        /// find parcel in the data conditions and connect it to the drone
        /// </summary>
        /// <param name="droneId"></param>
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

            var ParcelsDroneCanCarry = from parcel in GetParcelsNoDrones()
                                       let sender = GetCustomer(GetParcel(parcel.Id).Sender.Id)
                                       let target = GetCustomer(GetParcel(parcel.Id).Target.Id)
                                       orderby parcel.Priority, parcel.Weight, Distance(drone.Location, sender.Location) descending
                                       where drone.Battery >= BatteryDelivery(drone, parcel, sender, target)
                                       select parcel;

            ParcelToList parcelToConnect = ParcelsDroneCanCarry.First();
            if (parcelToConnect is null)
                throw new NoPackagesToDroneException("There are no parcels that the drone you entered can carry.");

            DO.Parcel updateParcel = updateParcel = dal.GetParcel(parcelToConnect.Id);
            updateParcel.DroneId = drone.Id;
            updateParcel.Scheduled = DateTime.Now;

            dal.UpdateParcel(updateParcel);

            for (int i = 0; i < ListDrones.Count; i++)
                if (ListDrones[i].Id == drone.Id)
                {
                    DroneToList newDrone = ListDrones[i];
                    newDrone.Status = DroneStatuses.Delivery;
                    newDrone.IdParcel = updateParcel.Id;
                    ListDrones[i] = newDrone;
                }
        }

        /// <summary>
        /// Collection the parcel by the drone
        /// </summary>
        /// <param name="droneId"></param>
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

            for (int i = 0; i < ListDrones.Count(); i++)
            {
                if (ListDrones[i].Id == drone.Id)
                {
                    DroneToList updateDrone = ListDrones[i];
                    updateDrone.Battery -= Distance(drone.Location, drone.ParcelByTransfer.PickUpLocation) * BatteryAvailable;
                    updateDrone.Battery = (double)System.Math.Round(updateDrone.Battery, 2);
                    updateDrone.Location = drone.ParcelByTransfer.PickUpLocation;
                    ListDrones[i] = updateDrone;
                }
            }

            DO.Parcel updateParcel = dal.GetParcel(drone.ParcelByTransfer.Id);
            updateParcel.PickedUp = DateTime.Now;
            dal.UpdateParcel(updateParcel);
        }

        /// <summary>
        /// Supply parcel by drone
        /// </summary>
        /// <param name="droneId"></the id of the drone>
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

            DO.Parcel updateParcel = dal.GetParcel(drone.ParcelByTransfer.Id);
            updateParcel.Delivered = DateTime.Now;
            dal.UpdateParcel(updateParcel);

            for (int i = 0; i < ListDrones.Count(); i++)
            {
                if (ListDrones[i].Id == drone.Id)
                {
                    DroneToList newDrone = ListDrones[i];
                    if (drone.ParcelByTransfer.Weight == WeightCategories.Heavy)
                        newDrone.Battery -= drone.ParcelByTransfer.DistanceOfTransfer * BatteryHeavyWeight;
                    if (drone.ParcelByTransfer.Weight == WeightCategories.Medium)
                        newDrone.Battery -= drone.ParcelByTransfer.DistanceOfTransfer * BatteryMediumWeight;
                    if (drone.ParcelByTransfer.Weight == WeightCategories.Light)
                        newDrone.Battery -= drone.ParcelByTransfer.DistanceOfTransfer * BatteryLightWeight;
                    newDrone.Battery = (double)System.Math.Round(newDrone.Battery, 2);

                    newDrone.Location = drone.ParcelByTransfer.TargetLocation;
                    newDrone.Status = DroneStatuses.Available;
                    newDrone.IdParcel = 0;
                    ListDrones[i] = newDrone;
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
                if (station.AvalibleChargeSlots == 0) // if the station that the user ask there is no place for charge
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
            batteryDelivery = distanceDelivery * BatteryAvailable;

            // From the pickup location to the target location
            distanceDelivery = Distance(sender.Location, target.Location);
            if (parcel.Weight == WeightCategories.Heavy)
                batteryDelivery += distanceDelivery * BatteryHeavyWeight;
            if (parcel.Weight == WeightCategories.Medium)
                batteryDelivery += distanceDelivery * BatteryMediumWeight;
            if (parcel.Weight == WeightCategories.Light)
                batteryDelivery += distanceDelivery * BatteryLightWeight;

            // From the target location to the station charge location
            StationToList nearStation = GetStationsWithAvailableCharge().OrderByDescending(station => Distance(target.Location, GetStation(station.Id).Location)).First();
            distanceDelivery = Distance(target.Location, GetStation(nearStation.Id).Location);
            batteryDelivery += distanceDelivery * BatteryAvailable;

            return batteryDelivery;
        }
    }
}

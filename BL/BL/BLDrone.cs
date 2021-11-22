using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
using IDAL;
using IDAL.DO;
using Customer = IBL.BO.Customer;
using Drone = IBL.BO.Drone;
using Priorities = IBL.BO.Priorities;
using Station = IBL.BO.Station;
using WeightCategories = IBL.BO.WeightCategories;


namespace IBL
{
    public partial class BL : IBL
    {
        /// <summary>
        /// add a drone
        /// </summary>
        /// <returns></no returns, add a drone>
        public void AddDrone(Drone newDrone, int idStation)
        {
            try
            {
                CheckDrone(newDrone);// check the input of the user
            }
            catch (DroneException e)
            {
                throw new DroneException("" + e);
            }
            IDAL.DO.Drone drone = new IDAL.DO.Drone();
            DroneToList newDroneToList = new DroneToList();

            drone.Id = newDrone.Id;
            drone.Model = newDrone.Model;
            drone.Weight = (IDAL.DO.WeightCategories)newDrone.Weight;


            newDroneToList.Id = newDrone.Id;
            newDroneToList.Model = newDrone.Model;
            newDroneToList.Weight = newDrone.Weight;
            newDroneToList.Battery = 20 * rand.NextDouble() + 20;
            newDroneToList.Status = DroneStatuses.Available; // for the charge after he will be in Maintenance.
            newDroneToList.IdParcel = 0;
            try
            {
                IDAL.DO.Station station = dal.GetStation(idStation);// try to find the station the user want to connect the drone to and if the station the
                if (station.ChargeSlots == 0) // user ask have place for charge
                    throw new StationException("The station you ask not have more place.");
            }
            catch (DalObject.stationException e)
            {
                throw new StationException("" + e);
            }

            newDroneToList.Location = new Location();
            newDroneToList.Location.Longitude = dal.GetStation(idStation).Longitude;
            newDroneToList.Location.Latitude = dal.GetStation(idStation).Latitude;

            try
            {
                int foundDrone = CheckDroneAndParcel(newDroneToList.Id, dal.GetParcels());//return the id of the parcel
                newDroneToList.IdParcel = foundDrone;
            }
            catch (DroneException e)
            {
                throw new DroneException("" + e);
            }

            ListDrones.Add(newDroneToList);
            try
            {
                dal.AddDrone(drone);// add the drone just if the drone not in the data center
            }
            catch (DalObject.DroneException e)
            {
                throw new DroneException("" + e);
            }

            try
            {
                SendDroneToDroneCharge(newDroneToList.Id);
            }
            catch (DroneException )
            { }
        }
        /// <summary>
        /// get a drone
        /// </summary>
        /// <returns></return the drone>
        public Drone GetDrone(int id)
        {
            IDAL.DO.Drone idalDrone = new IDAL.DO.Drone();
            try
            {
                idalDrone = dal.GetDrone(id);
            }
            catch (DalObject.DroneException e)
            {
                throw new DroneException("" + e);
            }

            Drone drone = new Drone();
            drone.Id = idalDrone.Id;
            drone.Model = idalDrone.Model;
            drone.Weight = Enum.Parse<WeightCategories>(idalDrone.Weight.ToString());

            foreach (var eleDroneToList in ListDrones)
            {
                if (eleDroneToList.Id == drone.Id)
                {
                    drone.Battery = eleDroneToList.Battery;
                    drone.Status = eleDroneToList.Status;
                    drone.Location = new Location()
                        {Longitude = eleDroneToList.Location.Longitude, Latitude = eleDroneToList.Location.Latitude};

                    IDAL.DO.Parcel parcel = new IDAL.DO.Parcel();
                    drone.ParcelByTransfer = new ParcelByTransfer();
                    try
                    {
                        parcel = dal.GetParcel(eleDroneToList.IdParcel);
                    }
                    catch (DalObject.ParcelException )
                    {
                        drone.ParcelByTransfer.Status = false;
                        return drone;
                    }

                    if (parcel.DroneId == drone.Id)
                    {
                        drone.ParcelByTransfer.Id = parcel.Id;
                        drone.ParcelByTransfer.Weight = Enum.Parse<WeightCategories>(parcel.Weight.ToString());

                        if (parcel.Scheduled != null && parcel.PickedUp == null)
                            drone.ParcelByTransfer.Status = false;

                        if (parcel.PickedUp != null && parcel.Delivered == null)
                            drone.ParcelByTransfer.Status = true;

                        drone.ParcelByTransfer.Priority = Enum.Parse<Priorities>(parcel.Priority.ToString());

                        IDAL.DO.Customer customer = dal.GetCustomer(parcel.SenderId);
                        drone.ParcelByTransfer.SenderInParcel = new CustomerInParcel()
                            {Id = customer.Id, NameCustomer = customer.Name};
                        drone.ParcelByTransfer.PickUpLocation = new Location()
                            {Longitude = customer.Longitude, Latitude = customer.Latitude};

                        customer = dal.GetCustomer(parcel.TargetId);
                        drone.ParcelByTransfer.ReceiverInParcel = new CustomerInParcel()
                            {Id = customer.Id, NameCustomer = customer.Name};
                        drone.ParcelByTransfer.TargetLocation = new Location()
                            {Longitude = customer.Longitude, Latitude = customer.Latitude};

                        drone.ParcelByTransfer.DistanceOfTransfer = Distance(drone.ParcelByTransfer.PickUpLocation,
                            drone.ParcelByTransfer.TargetLocation);
                    }
                }
            }
            return drone;
        }
        /// <summary>
        /// get a drones
        /// </summary>
        /// <returns></return all drones>
        public IEnumerable<DroneToList> GetDrones()
        {
            return ListDrones;
        }
        /// <summary>
        /// Update the model of the drone
        /// </summary>
        /// <returns></no returns, update the model of the drone>
        public void UpdateDroneModel(int droneId, string newModel)
        {
            IDAL.DO.Drone updateDrone = new IDAL.DO.Drone();
            try
            {
                updateDrone = dal.GetDrone(droneId);
            }
            catch (DalObject.DroneException e)
            {
                throw new DroneException("" + e);
            }
            
            if (newModel == "")
                throw new DroneException("ERROR: Model must have value");


            updateDrone.Model = newModel;

            for (int i = 0; i < ListDrones.Count(); i++)
            {
                if (ListDrones[i].Id == droneId)
                {
                    DroneToList updateDroneToList = ListDrones[i];
                    updateDroneToList.Model = newModel;
                    ListDrones[i] = updateDroneToList;
                }
            }

            dal.UpdateDrone(updateDrone);
        }
        /// <summary>
        /// Send the drone to drone charge
        /// </summary>
        /// <returns></no returns, just send the drone to drone charge>
        public void SendDroneToDroneCharge(int id)
        {
            Drone drone = new Drone();
            try
            {
                drone = GetDrone(id);
            }
            catch (DroneException e)
            {
                throw new DroneException("" + e);
            }
            if (drone.Status != DroneStatuses.Available)
                throw new DroneException("ERROR: the drone not available to charge ");

            Station nearStation = new Station();
            try
            {
                nearStation = NearStationToDrone(dal.GetDrone(drone.Id));// if the drone is not exist
            }
            catch (DroneException e)
            {
                throw new DroneException("" + e);
            }
            double distance = Distance(drone.Location, nearStation.Location);
            if (distance * BatteryAvailable > drone.Battery)
                throw new DroneException("ERROR: the drone not have battery to go to station charge ");

            for (int i = 0; i < ListDrones.Count; i++)
                if (ListDrones[i].Id == drone.Id)
                {
                    DroneToList newDrone = ListDrones[i];
                    if (distance * BatteryAvailable > 100)
                        newDrone.Battery = 100;
                    else
                        newDrone.Battery += distance * BatteryAvailable;

                    newDrone.Location = nearStation.Location;
                    newDrone.Status = DroneStatuses.Maintenance;
                    ListDrones[i] = newDrone;
                }

            IDAL.DO.Station updateStation = dal.GetStation(nearStation.Id);
            updateStation.ChargeSlots--;
            dal.UpdateStation(updateStation);

            IDAL.DO.DroneCharge newDroneCharge = new DroneCharge();
            newDroneCharge.StationId = nearStation.Id;
            newDroneCharge.DroneId = drone.Id;
            try
            {
                dal.AddDroneCharge(newDroneCharge);
            }
            catch (DalObject.DroneChargeException e)
            {
                throw new DroneException("" + e);
            }
        }
        /// <summary>
        /// Release the drone from the drone charge
        /// </summary>
        /// <returns></no returns, release the drone from the drone charge>
        public void ReleaseDroneFromDroneCharge(int id, int chargeTime)
        {
            try
            {
                GetDrone(id);
            }
            catch (DroneException e)
            {
                throw new DroneException("" + e);
            }

            if (chargeTime < 0)
                throw new DroneException("ERROR: the charge time must a positive value! ");

            if (GetDrone(id).Status != DroneStatuses.Maintenance)
                throw new DroneException("The drone can not release because he is in maintenance statuses:\n ");

            foreach (var elementDroneCharge in dal.GetDronesCharge())
            {
                if (id == elementDroneCharge.DroneId)
                {
                    foreach (var elementListDrone in ListDrones)
                    {
                        if (elementDroneCharge.DroneId == elementListDrone.Id)
                        {
                            foreach (var elementStationToList in GetStations(s => true))
                            {
                                if (elementDroneCharge.StationId == elementStationToList.Id)
                                {
                                    IDAL.DO.Station updateStation = dal.GetStation(elementDroneCharge.StationId);
                                    updateStation.ChargeSlots++;
                                    dal.UpdateStation(updateStation);

                                    if (chargeTime * ChargingRateOfDrone > 100)
                                        elementListDrone.Battery = 100;
                                    else
                                    {
                                        elementListDrone.Battery += chargeTime * ChargingRateOfDrone;
                                        if (elementListDrone.Battery > 100)
                                            elementListDrone.Battery = 100;
                                    }
                                        

                                    elementListDrone.Status = DroneStatuses.Available;
                                    dal.RemoveDroneCharge(elementDroneCharge);
                                    return;
                                }
                            }

                        }
                    }
                }
            }
        }
        /// <summary>
        /// Check the input of the user
        /// </summary>
        /// <returns></no returns, just check the input of the user>
        private void CheckDrone(Drone drone)
        {
            if (drone.Id < 0)
                throw new DroneException("ERROR: the ID is illegal! ");
            if (drone.Model == "")
                throw new DroneException("ERROR: Model must have value");
        }

        /// <summary>
        /// check if the drone have parcel for the DroneToList in bl
        /// </summary>
        /// <param name="droneId"></the drone we search for>
        /// <param name="parcels"></all the parcels in dal>
        /// <returns></return the parcel id if the drone coneccted to some parcel else -1 (not conected)>
        private int CheckDroneAndParcel(int droneId, IEnumerable<IDAL.DO.Parcel> parcels)
        {
            foreach (IDAL.DO.Parcel elementParcel in parcels)
                if (elementParcel.DroneId == droneId)
                    return elementParcel.Id;
            return 0;
        }
    }
}

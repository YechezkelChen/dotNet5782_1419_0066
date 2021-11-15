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
        public void AddDrone(Drone newDrone, int idStation)
        {
            try
            {
                CheckDrone(newDrone);
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
            newDroneToList.Status = DroneStatuses.Maintenance;
            try
            {
                newDroneToList.Location.Longitude = dal.GetStation(idStation).Longitude;
                newDroneToList.Location.Latitude = dal.GetStation(idStation).Latitude;
            }
            catch (DalObject.stationException e)
            {
                throw new DroneException("" + e);
            }

            try
            {
                int foundDrone = CheckDroneAndParcel(newDroneToList.Id, dal.GetParcels());
                newDroneToList.IdParcel = foundDrone;
            }
            catch (DroneException e)
            {
                throw new DroneException("" + e);
            }

            ListDrones.Add(newDroneToList);
            dal.AddDrone(drone);
        }

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
                    drone.Location = eleDroneToList.Location;

                    IDAL.DO.Parcel parcel = new IDAL.DO.Parcel();
                    try
                    {
                        parcel = dal.GetParcel(eleDroneToList.IdParcel);
                    }
                    catch (Exception e)
                    {
                        return drone;
                    }

                    if (parcel.DroneId == drone.Id)
                    {
                        drone.ParcelByTransfer.Id = parcel.Id;
                        drone.ParcelByTransfer.Weight= Enum.Parse<WeightCategories>(parcel.Weight.ToString());

                        if (parcel.Scheduled != DateTime.MinValue && parcel.PickedUp == DateTime.MinValue)
                            drone.ParcelByTransfer.Status = false;

                        if (parcel.PickedUp != DateTime.MinValue && parcel.Delivered == DateTime.MinValue)
                            drone.ParcelByTransfer.Status = true;

                        drone.ParcelByTransfer.Priority= Enum.Parse<Priorities>(parcel.Priority.ToString());

                        IDAL.DO.Customer customer = dal.GetCustomer(parcel.SenderId);
                        drone.ParcelByTransfer.SenderInParcel.Id = customer.Id;
                        drone.ParcelByTransfer.SenderInParcel.NameCustomer = customer.Name;
                        drone.ParcelByTransfer.PickUpLocation.Longitude = customer.Longitude;
                        drone.ParcelByTransfer.PickUpLocation.Latitude = customer.Latitude;

                        customer = dal.GetCustomer(parcel.TargetId);
                        drone.ParcelByTransfer.ReceiverInParcel.Id = customer.Id;
                        drone.ParcelByTransfer.ReceiverInParcel.NameCustomer = customer.Name;
                        drone.ParcelByTransfer.TargetLocation.Longitude = customer.Longitude;
                        drone.ParcelByTransfer.TargetLocation.Latitude = customer.Latitude;

                        drone.ParcelByTransfer.DistanceOfTransfer = Distance(drone.ParcelByTransfer.PickUpLocation,
                            drone.ParcelByTransfer.TargetLocation);
                    }
                }
            }
            return drone;
        }

        public IEnumerable<DroneToList> GetDrones()
        {
            return ListDrones;
        }

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
            else
                updateDrone.Model = newModel;
            
            dal.UpdateDrone(updateDrone);
        }


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
                nearStation = NearStationToDrone(dal.GetDrone(drone.Id));
            }
            catch (DroneException e)
            {
                throw new DroneException("" + e);
            }
            double distance = Distance(drone.Location, nearStation.Location);
            if (distance * BatteryAvailable < drone.Battery)
                throw new DroneException("ERROR: the drone not have battery to go to station charge ");

            for (int i = 0; i < ListDrones.Count; i++)
                if (ListDrones[i].Id == drone.Id)
                {
                    DroneToList newDrone = ListDrones[i];
                    newDrone.Battery -= distance * BatteryAvailable;
                    newDrone.Location = nearStation.Location;
                    newDrone.Status = DroneStatuses.Maintenance;
                    ListDrones[i] = newDrone;
                }

            IDAL.DO.Station updateStation = dal.GetStation(nearStation.Id);
            updateStation.ChargeSlots--;
            dal.UpdateStation(updateStation);

            IDAL.DO.DroneCharge newDroneCharge = new DroneCharge();
            newDroneCharge.Stationld = nearStation.Id;
            newDroneCharge.DroneId = drone.Id;
            dal.AddDroneCharge(newDroneCharge);
        }

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
                            foreach (var elementStationToList in GetStations())
                            {
                                if (elementDroneCharge.Stationld == elementStationToList.Id)
                                {
                                    IDAL.DO.Station updateStation = dal.GetStation(elementDroneCharge.Stationld);
                                    updateStation.ChargeSlots++;
                                    dal.UpdateStation(updateStation);

                                    elementListDrone.Battery += chargeTime * ChargingRateOfDrone;
                                    elementListDrone.Status = DroneStatuses.Available;
                                    dal.RemoveDroneCharge(elementDroneCharge);
                                }
                            }

                        }
                    }
                }
            }
        }

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
            throw new DroneException("ERROR: the drone not exist! ");
        }
    }
}

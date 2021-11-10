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
            newDrone.Battery = rand.Next(20, 40);
            newDrone.Status = DroneStatuses.Maintenance;
            try
            {
                newDrone.Location.Longitude = dal.GetStation(idStation).Longitude;
                newDrone.Location.Latitude = dal.GetStation(idStation).Latitude;
            }
            catch (DalObject.StationExeption e)
            {
                throw new DroneException("Try again you make a " + e);
            }

            newDroneToList.Id = newDrone.Id;
            newDroneToList.Model = newDrone.Model;
            newDroneToList.Weight = newDrone.Weight;
            newDroneToList.Battery = rand.Next(20, 40);
            newDroneToList.Status = DroneStatuses.Maintenance;
            try
            {
                newDroneToList.Location.Longitude = dal.GetStation(idStation).Longitude;
                newDroneToList.Location.Latitude = dal.GetStation(idStation).Latitude;
            }
            catch (DalObject.StationExeption e)
            {
                throw new DroneException("Try again you make a " + e);
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
            catch (DalObject.DroneExeption e)
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

                    IDAL.DO.Parcel parcel = dal.GetParcel(eleDroneToList.IdParcel);
                    if (parcel.DroneId == drone.Id)
                    {
                        drone.ParcelByTransfer.Id = parcel.Id;
                        drone.ParcelByTransfer.Weight= Enum.Parse<WeightCategories>(parcel.Weight.ToString());

                        if (parcel.Scheduled != DateTime.MinValue && parcel.PickedUp == DateTime.MinValue)
                            drone.ParcelByTransfer.ParcelStatus = false;

                        if (parcel.PickedUp != DateTime.MinValue && parcel.Delivered == DateTime.MinValue)
                            drone.ParcelByTransfer.ParcelStatus = true;

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

        public void UpdateDrone(int droneId, string newModel)
        {
            try
            {
                if (dal.CheckNotExistDrone(dal.GetDrone(droneId), dal.GetDrones()))
                    throw new DroneException("ERROR: the drone not exist:");

            }
            catch (DroneException e)
            {
                throw new DroneException("" + e);
            }

            dal.UpdateDroneModel(droneId, newModel);
        }

        public void CheckDrone(Drone drone)
        {
            if (drone.Id < 0)
                throw new DroneException("ERROR: the ID is illegal! ");
        }

        /// <summary>
        /// check if the drone have parcel for the DroneToList in bl
        /// </summary>
        /// <param name="droneId"></the drone we search for>
        /// <param name="parcels"></all the parcels in dal>
        /// <returns></return the parcel id if the drone coneccted to some parcel else -1 (not conected)>
        public int CheckDroneAndParcel(int droneId, IEnumerable<IDAL.DO.Parcel> parcels)
        {
            foreach (IDAL.DO.Parcel elementParcel in parcels)
                if (elementParcel.DroneId == droneId)
                    return elementParcel.Id;
            throw new DroneException("ERROR: the drone not exist! ");
        }
    }
}

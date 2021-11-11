﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
using IDAL;


namespace IBL
{
    public partial class BL : IBL
    {
        public void AddParcel(Parcel newParcel)
        {
            try
            {
                CheckParcel(newParcel);
            }
            catch (ParcelException e)
            {
                throw new ParcelException("" + e);
            }
            IDAL.DO.Parcel parcel = new IDAL.DO.Parcel();
            parcel.SenderId = newParcel.SenderId;
            parcel.TargetId = newParcel.TargetId;
            parcel.Weight = (IDAL.DO.WeightCategories)newParcel.Weight;
            parcel.Priority = (IDAL.DO.Priorities)newParcel.Priority;
            parcel.DroneId = 0;
            parcel.Requested = DateTime.Now;
            parcel.Scheduled = DateTime.MinValue;
            parcel.PickedUp = DateTime.MinValue;
            parcel.Delivered = DateTime.MinValue;
            dal.AddParcel(parcel);
        }

        public Parcel GetParcel(int id)
        {
            IDAL.DO.Parcel idalParcel = new IDAL.DO.Parcel();
            try
            {
                idalParcel = dal.GetParcel(id);
            }
            catch (DalObject.ParcelExeption e)
            {
                throw new ParcelException("" + e);
            }

            Parcel parcel = new Parcel();
            parcel.Id = idalParcel.Id;
            parcel.SenderId = idalParcel.SenderId;
            parcel.TargetId = idalParcel.TargetId;
            parcel.Weight = Enum.Parse<WeightCategories>(idalParcel.Weight.ToString());
            parcel.Priority = Enum.Parse<Priorities>(idalParcel.Priority.ToString());

            foreach (var elementDrone in ListDrones)
                if (elementDrone.Id == idalParcel.DroneId)
                {
                    parcel.DroneInParcel.Id = elementDrone.Id;
                    parcel.DroneInParcel.Battery = elementDrone.Battery;
                    parcel.DroneInParcel.Location = elementDrone.Location;
                }

            parcel.Requested = idalParcel.Requested;
            parcel.Scheduled = idalParcel.Scheduled;
            parcel.PickedUp = idalParcel.PickedUp;
            parcel.Delivered = idalParcel.Delivered;

            return parcel;
        }

        public IEnumerable<ParcelToList> GetParcels()
        {
            IEnumerable<IDAL.DO.Parcel> idalParcels = dal.GetParcels();
            List<ParcelToList> parcelToLists = new List<ParcelToList>();
            ParcelToList newParcel = new ParcelToList();

            foreach (var idalParcel in idalParcels)
            {
                newParcel.Id = idalParcel.Id;
                newParcel.SenderId = idalParcel.SenderId;
                newParcel.TargetId = idalParcel.TargetId;
                newParcel.Weight = Enum.Parse<WeightCategories>(idalParcel.Weight.ToString());
                newParcel.Priority = Enum.Parse<Priorities>(idalParcel.Priority.ToString());

                if (idalParcel.Requested != DateTime.MinValue)
                    newParcel.ParcelStatuses = ParcelStatuses.Requested;
                if (idalParcel.Scheduled != DateTime.MinValue)
                    newParcel.ParcelStatuses = ParcelStatuses.Scheduled;
                if (idalParcel.PickedUp != DateTime.MinValue)
                    newParcel.ParcelStatuses = ParcelStatuses.PickedUp;
                if (idalParcel.Delivered != DateTime.MinValue)
                    newParcel.ParcelStatuses = ParcelStatuses.Delivered;

                parcelToLists.Add(newParcel);
            }

            return parcelToLists;
        }

        public IEnumerable<ParcelToList> GetParcelsNoDrones()
        {
            IEnumerable<IDAL.DO.Parcel> idalParcels = dal.GetParcels();
            List<ParcelToList> parcelNoDrones = new List<ParcelToList>();
            ParcelToList newParcel = new ParcelToList();

            foreach (var idalParcel in idalParcels)
                if (idalParcel.Scheduled == DateTime.MinValue)
                {
                    newParcel.Id = idalParcel.Id;
                    newParcel.SenderId = idalParcel.SenderId;
                    newParcel.TargetId = idalParcel.TargetId;
                    newParcel.Weight = Enum.Parse<WeightCategories>(idalParcel.Weight.ToString());
                    newParcel.Priority = Enum.Parse<Priorities>(idalParcel.Priority.ToString());

                    newParcel.ParcelStatuses = ParcelStatuses.Requested;

                    parcelNoDrones.Add(newParcel);
                }
            
            return parcelNoDrones;
        }

        public void CheckParcel(Parcel parcel)
        {
            if (parcel.SenderId < 0)
                throw new ParcelException("ERROR: the Sender ID is illegal! ");
            if (parcel.TargetId < 0)
                throw new ParcelException("ERROR: the Target ID is illegal! ");
            if (parcel.SenderId == parcel.TargetId)
                throw new ParcelException("ERROR: the Target ID and the Sender ID are equals! ");
        }

        public void ConnectParcelToDrone(int droneId)
        {
            try
            {
                dal.GetDrone(droneId);
            }
            catch (DroneException e)
            {
                throw new DroneException(""+e);
            }

            Drone connectDrone = GetDrone(droneId);
            if (connectDrone.Status != DroneStatuses.Available)
                throw new DataException("The drone is not available:\n ");

            foreach (var elementParcelsNoDrone in GetParcelsNoDrones())
            {
                if (elementParcelsNoDrone.Priority == Priorities.Emergency)
                {
                    bool flag = new bool();
                    flag = false;

                    if (connectDrone.Weight == WeightCategories.Heavy)
                        flag = true;

                    if (connectDrone.Weight == WeightCategories.Medium &&
                        (elementParcelsNoDrone.Weight == WeightCategories.Medium ||
                         elementParcelsNoDrone.Weight == WeightCategories.Light))
                        flag = true;

                    if (connectDrone.Weight == WeightCategories.Light &&
                        elementParcelsNoDrone.Weight == WeightCategories.Light)
                        flag = true;

                    if (!flag)
                        throw new DataException(
                            "The maximum weight that the drone can carry is less than the weight of the parcel :(\n ");
                    Customer senderCustomer = GetCustomer(elementParcelsNoDrone.SenderId);
                    IDAL.DO.Customer targetCustomer = dal.GetCustomer(elementParcelsNoDrone.TargetId);

                    Location targetLocation = new Location()
                        {Longitude = targetCustomer.Longitude, Latitude = targetCustomer.Latitude};

                    double distanceDelivery = Distance(connectDrone.Location, senderCustomer.Location);
                    double batteryDelivery = distanceDelivery * dAvailable;

                    distanceDelivery = Distance(senderCustomer.Location, targetLocation); // the distance between the drone and the target
                    if (elementParcelsNoDrone.Weight == WeightCategories.Heavy)
                        batteryDelivery += distanceDelivery * dHeavyW;
                    if (elementParcelsNoDrone.Weight == WeightCategories.Medium)
                        batteryDelivery += distanceDelivery * dMediumW;
                    if (elementParcelsNoDrone.Weight == WeightCategories.Light)
                        batteryDelivery += distanceDelivery * dLightW;

                    distanceDelivery = Distance(targetLocation, NearStationToCustomer(targetCustomer).Location);
                    batteryDelivery += distanceDelivery * dAvailable;

                    if (connectDrone.Battery < batteryDelivery)
                        throw new DroneException("ERROR: The drone not have much battery to send the parcel!!!\n");
                    connectDrone.Status = DroneStatuses.Delivery;
                    for (int i = 0; i < ListDrones.Count(); i++)
                    {
                        if (ListDrones[i].Id == connectDrone.Id)
                        {
                            DroneToList updateDrone = ListDrones[i];
                            updateDrone.Status = DroneStatuses.Delivery;
                            ListDrones[i] = updateDrone;
                        }
                    }


                    IDAL.DO.Parcel updateParcel = dal.GetParcel(elementParcelsNoDrone.Id);
                    updateParcel.DroneId = connectDrone.Id;
                    dal.UpdateParcel(updateParcel);


                }
                  

                }
                    
                    




            }



        }
    }
}
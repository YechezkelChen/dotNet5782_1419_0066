using System;
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
            parcel.SenderId = newParcel.Sender.Id;
            parcel.TargetId = newParcel.Target.Id;
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
            catch (DalObject.ParcelException e)
            {
                throw new ParcelException("" + e);
            }

            Parcel parcel = new Parcel();
            parcel.Id = idalParcel.Id;
            parcel.Sender.Id = idalParcel.SenderId;
            parcel.Target.Id = idalParcel.TargetId;
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
                newParcel.SenderName = GetCustomer(idalParcel.SenderId).Name;
                newParcel.TargetName = GetCustomer(idalParcel.TargetId).Name;
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
                    newParcel.SenderName = GetCustomer(idalParcel.SenderId).Name;
                    newParcel.TargetName = GetCustomer(idalParcel.TargetId).Name;
                    newParcel.Weight = Enum.Parse<WeightCategories>(idalParcel.Weight.ToString());
                    newParcel.Priority = Enum.Parse<Priorities>(idalParcel.Priority.ToString());

                    newParcel.ParcelStatuses = ParcelStatuses.Requested;

                    parcelNoDrones.Add(newParcel);
                }
            
            return parcelNoDrones;
        }

        public void ConnectParcelToDrone(int droneId)
        {
            Drone connectDrone = new Drone();
            try
            {
                connectDrone = GetDrone(droneId);
            }
            catch (DroneException e)
            {
                throw new DroneException("" + e);
            }

            if (connectDrone.Status != DroneStatuses.Available)
                throw new DataException("ERROR: The drone is not available:\n ");

            ParcelToList parcel = new ParcelToList();

            IEnumerable<ParcelToList> parcelNoDrones = GetParcelsNoDrones();
            List<ParcelToList> prioritiesParcel = new List<ParcelToList>();
            List<ParcelToList> weightParcel = new List<ParcelToList>();
            Parcel parcelToConnect = new Parcel();

            for (int i = (int)Priorities.Emergency; i > (int)Priorities.Normal; i--)
            {
                prioritiesParcel = parcelNoDrones.ToList().FindAll(parcel => parcel.Priority == (Priorities)i);
                for (int j = (int) connectDrone.Weight; j > (int) WeightCategories.Light; j--)
                {
                    weightParcel = prioritiesParcel.ToList().FindAll(parcel => parcel.Weight == (WeightCategories) j);
                    while (true)
                    {
                        parcelToConnect = NearParcelToDrone(connectDrone, weightParcel);

                        double distanceDelivery = Distance(connectDrone.Location,
                            GetCustomer(GetParcel(parcelToConnect.Id).Sender.Id).Location);
                        double batteryDelivery = distanceDelivery * BatteryAvailable;

                        distanceDelivery = Distance(GetCustomer(GetParcel(parcelToConnect.Id).Sender.Id).Location,
                            GetCustomer(GetParcel(parcelToConnect.Id).Target.Id).Location); // the distance between the drone and the target
                        if (parcelToConnect.Weight == WeightCategories.Heavy)
                            batteryDelivery += distanceDelivery * BatteryHeavyWeight;
                        if (parcelToConnect.Weight == WeightCategories.Medium)
                            batteryDelivery += distanceDelivery * BatteryMediumWeight;
                        if (parcelToConnect.Weight == WeightCategories.Light)
                            batteryDelivery += distanceDelivery * BatteryLightWeight;

                        distanceDelivery = Distance(GetCustomer(GetParcel(parcelToConnect.Id).Target.Id).Location,
                            NearStationToCustomer(dal.GetCustomer(GetParcel(parcelToConnect.Id).Target.Id)).Location);
                        batteryDelivery += distanceDelivery * BatteryAvailable;

                        if (connectDrone.Battery >= batteryDelivery)
                            break;
                    }
                }
            }

            foreach (var drone in ListDrones)
                if (drone.Id == connectDrone.Id)
                    drone.Status = DroneStatuses.Delivery;

            IDAL.DO.Parcel updateParcel = dal.GetParcel(parcelToConnect.Id);
            updateParcel.DroneId = connectDrone.Id;
            updateParcel.Scheduled = DateTime.Now;
             
            dal.UpdateParcel(updateParcel);
        }

        public void CollectionParcelByDrone(int idDrone)
        {
            Drone collectionDrone = new Drone();
            try
            {
                collectionDrone = GetDrone(idDrone);
            }
            catch (DroneException e)
            {
                throw new DroneException("" + e);
            }

            if (collectionDrone.Status != DroneStatuses.Delivery || collectionDrone.ParcelByTransfer.Status == true)
                throw new ParcelException("ERROR: The parcel early in delivery ");

            for (int i = 0; i < ListDrones.Count(); i++)
            {
                if (ListDrones[i].Id == collectionDrone.Id)
                {
                    DroneToList updateDrone = ListDrones[i];
                    updateDrone.Battery = Distance(collectionDrone.Location,
                        collectionDrone.ParcelByTransfer.PickUpLocation) * BatteryAvailable;
                    updateDrone.Location = collectionDrone.ParcelByTransfer.PickUpLocation;
                    ListDrones[i] = updateDrone;
                }
            }

            IDAL.DO.Parcel updateParcel = dal.GetParcel(collectionDrone.ParcelByTransfer.Id);
            updateParcel.PickedUp = DateTime.Now;
            dal.UpdateParcel(updateParcel);
        }

        public void SupplyParcelByDrone(int idDrone)
        {
            Drone drone = new Drone();
            try
            {
                drone = GetDrone(idDrone);
            }
            catch (DroneException e)
            {
                throw new DroneException("" + e);
            }

            Parcel parcel = new Parcel();
            foreach (var elementParcelToList in GetParcels())
            {
                parcel = GetParcel(elementParcelToList.Id);

                if (elementParcelToList.ParcelStatuses != ParcelStatuses.PickedUp ||
                    parcel.DroneInParcel.Id == idDrone)
                    throw new ParcelException("ERROR: the parcel not pickup:\n");

                double distance = Distance(drone.Location, GetCustomer(parcel.Target.Id).Location);

                if (parcel.Weight == WeightCategories.Heavy)
                    drone.Battery = distance * BatteryHeavyWeight;
                if (parcel.Weight == WeightCategories.Medium)
                    drone.Battery = distance * BatteryMediumWeight;
                if (parcel.Weight == WeightCategories.Light)
                    drone.Battery = distance * BatteryLightWeight;
                drone.Location = GetCustomer(parcel.Target.Id).Location;
                drone.Status = DroneStatuses.Available;
                for (int i = 0; i < ListDrones.Count(); i++)
                {
                    if (ListDrones[i].Id == drone.Id)
                    {
                        DroneToList updateDrone = ListDrones[i];
                        updateDrone.Battery = drone.Battery;
                        updateDrone.Location = drone.Location;
                        updateDrone.Status = drone.Status;
                        ListDrones[i] = updateDrone;
                    }
                }

                parcel.PickedUp = DateTime.Now;
                IDAL.DO.Parcel updateParcel = new IDAL.DO.Parcel();
                updateParcel.PickedUp = parcel.PickedUp;
                dal.UpdateParcel(updateParcel);
            }
        }

        private void CheckParcel(Parcel parcel)
        {
            if (parcel.Sender.Id < 0)
                throw new ParcelException("ERROR: the Sender ID is illegal! ");
            if (parcel.Target.Id < 0)
                throw new ParcelException("ERROR: the Target ID is illegal! ");
            if (parcel.Sender.Id == parcel.Target.Id)
                throw new ParcelException("ERROR: the Target ID and the Sender ID are equals! ");
        }

        private Parcel NearParcelToDrone(Drone drone, IEnumerable<ParcelToList> parcels)
        {
            List<double> distancesList = new List<double>();
            Location parcelLocation = new Location();
            Location droneLocation = drone.Location;

            foreach (var parcel in parcels)
                if (parcel.ParcelStatuses == ParcelStatuses.Requested)
                {
                    var senderParcel = GetCustomer(GetParcel(parcel.Id).Sender.Id);
                    distancesList.Add(Distance(senderParcel.Location, droneLocation));
                }

            double minDistance = distancesList.Min();

            Parcel nearparcel = new Parcel();
            foreach (var parcel in parcels)
                if (parcel.ParcelStatuses == ParcelStatuses.Requested)
                {
                    var senderParcel = GetCustomer(GetParcel(parcel.Id).Sender.Id);
                    if (minDistance == Distance(senderParcel.Location, droneLocation))
                        nearparcel = GetParcel(parcel.Id);
                }

            return nearparcel;
        }
    }
}
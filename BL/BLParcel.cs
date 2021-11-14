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
            catch (DalObject.ParcelExeption e)
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

        public void CheckParcel(Parcel parcel)
        {
            if (parcel.Sender.Id < 0)
                throw new ParcelException("ERROR: the Sender ID is illegal! ");
            if (parcel.Target.Id < 0)
                throw new ParcelException("ERROR: the Target ID is illegal! ");
            if (parcel.Sender.Id == parcel.Target.Id)
                throw new ParcelException("ERROR: the Target ID and the Sender ID are equals! ");
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

            for (int i = (int)Priorities.Emergency; i > (int)Priorities.Fast; i--)
            {
                prioritiesParcel = parcelNoDrones.ToList().FindAll(parcel => parcel.Priority == (Priorities)i);
                for (int j = (int)WeightCategories.Heavy; j > (int)WeightCategories.Light; j--)
                    weightParcel = prioritiesParcel.ToList().FindAll(parcel => parcel.Weight == (WeightCategories)j);


            }

            IEnumerable<DroneToList> dronesWithBattery = new List<DroneToList>();
            foreach (var parcelNoDrones in GetParcelsNoDrones())
            {
                Customer senderCustomer = GetCustomer(parcelNoDrones.SenderId);
                IDAL.DO.Customer targetCustomer = dal.GetCustomer(parcelNoDrones.TargetId);

                Location targetLocation = new Location()
                { Longitude = targetCustomer.Longitude, Latitude = targetCustomer.Latitude };

                double distanceDelivery = Distance(connectDrone.Location, senderCustomer.Location);
                double batteryDelivery = distanceDelivery * dAvailable;

                distanceDelivery = Distance(senderCustomer.Location, targetLocation); // the distance between the drone and the target
                if (parcelNoDrones.Weight == WeightCategories.Heavy)
                    batteryDelivery += distanceDelivery * dHeavyW;
                if (parcelNoDrones.Weight == WeightCategories.Medium)
                    batteryDelivery += distanceDelivery * dMediumW;
                if (parcelNoDrones.Weight == WeightCategories.Light)
                    batteryDelivery += distanceDelivery * dLightW;

                distanceDelivery = Distance(targetLocation, NearStationToCustomer(targetCustomer).Location);
                batteryDelivery += distanceDelivery * dAvailable;


                dronesWithBattery = dronesWithBattery.ToList().FindAll(parcel => connectDrone.Battery < batteryDelivery);
            }


            bool flagParcel = new bool();
            flagParcel = false;
            while (!flagParcel)
            {
                foreach (var elementParcelsNoDrone in GetParcelsNoDrones())
                    if (elementParcelsNoDrone.Priority == Priorities.Emergency)
                        parcel = elementParcelsNoDrone;

                if (parcel.Id == 0)
                    foreach (var elementParcelsNoDrone in GetParcelsNoDrones())
                        if (elementParcelsNoDrone.Priority == Priorities.Normal)
                            parcel = elementParcelsNoDrone;

                if (parcel.Id == 0)
                    foreach (var elementParcelsNoDrone in GetParcelsNoDrones())
                        if (elementParcelsNoDrone.Priority == Priorities.Fast)
                            parcel = elementParcelsNoDrone;

                if (parcel.Id == 0)
                    throw new ParcelException("ERROR: There is no parcels to send! ");

            }



            bool flagWeight = new bool();
            flagWeight = false;

            if (connectDrone.Weight == WeightCategories.Heavy)
                flagWeight = true;

            if (connectDrone.Weight == WeightCategories.Medium &&
                (parcel.Weight == WeightCategories.Medium ||
                 parcel.Weight == WeightCategories.Light))
                flagWeight = true;

            if (connectDrone.Weight == WeightCategories.Light &&
                parcel.Weight == WeightCategories.Light)
                flagWeight = true;

            if (!flagWeight)
                throw new DataException(
                    "ERROR: The maximum weight that the drone can carry is less than the weight of the parcel :(\n ");

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
                    { Longitude = targetCustomer.Longitude, Latitude = targetCustomer.Latitude };

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

            if (collectionDrone.Status != DroneStatuses.Delivery || collectionDrone.ParcelByTransfer.ParcelStatus == true)
                throw new ParcelException("ERROR: The parcel early in delivery ");

            for (int i = 0; i < ListDrones.Count(); i++)
            {
                if (ListDrones[i].Id == collectionDrone.Id)
                {
                    DroneToList updateDrone = ListDrones[i];
                    updateDrone.Battery = Distance(collectionDrone.Location,
                        collectionDrone.ParcelByTransfer.PickUpLocation) * dAvailable;
                    updateDrone.Location = collectionDrone.ParcelByTransfer.PickUpLocation;
                    ListDrones[i] = updateDrone;
                }
            }

            IDAL.DO.Parcel updateparcel = dal.GetParcel(collectionDrone.ParcelByTransfer.Id);
            updateparcel.PickedUp = DateTime.Now;
            dal.UpdateParcel(updateparcel);
        }

        public void SupplyParcelByDrone(int idDrone)
        {
            DroneToList dronToList = new DroneToList();
            Drone drone = new Drone();
            try
            {
                drone = GetDrone(idDrone);
            }
            catch (DroneException e)
            {
                throw new DroneException("" + e);
            }

            foreach (var elementDroneToList in ListDrones)
            {
                if (elementDroneToList.Id == idDrone)
                    dronToList = elementDroneToList;
            }

            ParcelToList parcelToList = new ParcelToList();
            Parcel parcel = new Parcel();
            foreach (var elementParcelToList in GetParcels())
            {
                parcel = GetParcel(elementParcelToList.Id);

                if (elementParcelToList.ParcelStatuses != ParcelStatuses.PickedUp ||
                    parcel.DroneInParcel.Id == idDrone)
                    throw new ParcelException("ERROR: the parcel not pickup:\n");

                double distance = Distance(drone.Location, GetCustomer(parcel.Target.Id).Location);

                if (parcel.Weight == WeightCategories.Heavy)
                    drone.Battery = distance * dHeavyW;
                if (parcel.Weight == WeightCategories.Medium)
                    drone.Battery = distance * dMediumW;
                if (parcel.Weight == WeightCategories.Light)
                    drone.Battery = distance * dLightW;
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

        public Parcel NearParcelToDrone(IDAL.DO.Drone drone)
        {
            List<double> distancesList = new List<double>();
            Location parcelLocation = new Location();
            Location droneLocation = GetDrone(drone.Id).Location;

            foreach (var parcel in GetParcels())
                if (parcel.ParcelStatuses == ParcelStatuses.Requested)
                {
                    var senderParcel = GetCustomer(GetParcel(parcel.Id).Sender.Id);
                    distancesList.Add(Distance(senderParcel.Location, droneLocation));
                }

            double minDistance = distancesList.Min();

            Parcel nearparcel = new Parcel();
            foreach (var parcel in GetParcels())
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
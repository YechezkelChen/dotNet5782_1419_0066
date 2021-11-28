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
        /// <summary>
        /// add parcel with all fields to data source with checking 
        /// </summary>
        /// <param name="newParcel"></param>
        public void AddParcel(Parcel newParcel)
        {
            try
            {
                CheckParcel(newParcel);
            }
            catch (ParcelException e)
            {
                throw new ParcelException(e.Message, e);
            }
            IDAL.DO.Parcel parcel = new IDAL.DO.Parcel();
            parcel.SenderId = newParcel.Sender.Id;
            parcel.TargetId = newParcel.Target.Id;
            parcel.Weight = (IDAL.DO.WeightCategories)newParcel.Weight;
            parcel.Priority = (IDAL.DO.Priorities)newParcel.Priority;
            parcel.DroneId = 0;
            parcel.Requested = DateTime.Now;
            parcel.Scheduled = null;
            parcel.PickedUp = null;
            parcel.Delivered = null;
            dal.AddParcel(parcel);
        }

        /// <summary>
        /// send id of parcel and checking that it exist.
        /// make special entity and return it
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Parcel GetParcel(int id)
        {
            IDAL.DO.Parcel idalParcel = new IDAL.DO.Parcel();
            try
            {
                idalParcel = dal.GetParcel(id);
            }
            catch (DalObject.ParcelException e)
            {
                throw new ParcelException(e.Message, e);
            }

            Parcel parcel = new Parcel();
            parcel.Id = idalParcel.Id;
            parcel.Sender = new CustomerInParcel()
                {Id = idalParcel.SenderId, NameCustomer = GetCustomer(idalParcel.SenderId).Name};
            parcel.Target = new CustomerInParcel()
                { Id = idalParcel.TargetId, NameCustomer = GetCustomer(idalParcel.TargetId).Name };
            parcel.Weight = Enum.Parse<WeightCategories>(idalParcel.Weight.ToString());
            parcel.Priority = Enum.Parse<Priorities>(idalParcel.Priority.ToString());

            parcel.DroneInParcel = new DroneInParcel()
            { Id = 0, Battery = 0, Location = new Location() { Longitude = 0, Latitude = 0 } };

            foreach (var elementDrone in ListDrones)
                if (elementDrone.Id == idalParcel.DroneId)
                {
                    parcel.DroneInParcel = new DroneInParcel()
                        {Id = elementDrone.Id, Battery = elementDrone.Battery, Location = elementDrone.Location};
                }

            parcel.Requested = idalParcel.Requested;
            parcel.Scheduled = idalParcel.Scheduled;
            parcel.PickedUp = idalParcel.PickedUp;
            parcel.Delivered = idalParcel.Delivered;

            return parcel;
        }

        /// <summary>
        /// return the list of parcels in special entity for show
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ParcelToList> GetParcels(Predicate<IDAL.DO.Parcel> parcelPredicate)
        {
            List<ParcelToList> parcelToLists = new List<ParcelToList>();
            
            foreach (var idalParcel in dal.GetParcels(parcelPredicate))
            {
                ParcelToList newParcel = new ParcelToList();
                newParcel.Id = idalParcel.Id;
                newParcel.SenderName = GetCustomer(idalParcel.SenderId).Name;
                newParcel.TargetName = GetCustomer(idalParcel.TargetId).Name;
                newParcel.Weight = Enum.Parse<WeightCategories>(idalParcel.Weight.ToString());
                newParcel.Priority = Enum.Parse<Priorities>(idalParcel.Priority.ToString());

                if (idalParcel.Requested != null)
                    newParcel.ParcelStatuses = ParcelStatuses.Requested;
                if (idalParcel.Scheduled != null)
                    newParcel.ParcelStatuses = ParcelStatuses.Scheduled;
                if (idalParcel.PickedUp != null)
                    newParcel.ParcelStatuses = ParcelStatuses.PickedUp;
                if (idalParcel.Delivered != null)
                    newParcel.ParcelStatuses = ParcelStatuses.Delivered;

                parcelToLists.Add(newParcel);
            }

            return parcelToLists;
        }

        /// <summary>
        /// find parcel in the data conditions and connect it to the drone
        /// </summary>
        /// <param name="droneId"></param>
        public void ConnectParcelToDrone(int droneId)
        {
            Drone connectDrone = new Drone();
            try
            {
                connectDrone = GetDrone(droneId);
            }
            catch (DroneException e)
            {
                throw new DroneException(e.Message, e);
            }

            if (connectDrone.Status != DroneStatuses.Available)
                throw new DroneException("ERROR: The drone is not available:\n ");

            ParcelToList parcel = new ParcelToList();

            IEnumerable<ParcelToList> parcelNoDrones = GetParcels(parcel => parcel.Scheduled == null &&
                                                                  parcel.PickedUp == null && parcel.Delivered == null); // just parcels that dont have them drone.
            List<ParcelToList> prioritiesParcel = new List<ParcelToList>();
            List<ParcelToList> weightParcel = new List<ParcelToList>();
            Parcel parcelToConnect = new Parcel();
            for (int i = (int)Priorities.Emergency; i >= (int)Priorities.Normal; i--)
            {
                prioritiesParcel = parcelNoDrones.ToList().FindAll(parcel => parcel.Priority == (Priorities)i); // parcels with priority according to the brace
                for (int j = (int) connectDrone.Weight; j >= (int) WeightCategories.Light; j--)
                {
                    weightParcel = prioritiesParcel.ToList().FindAll(parcel => parcel.Weight == (WeightCategories) j); // parcels with weight according to the brace
                    while (weightParcel.Count() != 0)
                    {
                        parcelToConnect = new Parcel();
                        parcelToConnect = NearParcelToDrone(connectDrone, weightParcel); // find the close station

                        double distanceDelivery = Distance(connectDrone.Location,
                            GetCustomer(GetParcel(parcelToConnect.Id).Sender.Id).Location);
                        double batteryDelivery = 0;
                        batteryDelivery = distanceDelivery * BatteryAvailable;

                        distanceDelivery = Distance(GetCustomer(GetParcel(parcelToConnect.Id).Sender.Id).Location,
                            GetCustomer(GetParcel(parcelToConnect.Id).Target.Id)
                                .Location); // the distance between the drone and the target
                        if (parcelToConnect.Weight == WeightCategories.Heavy)
                            batteryDelivery += distanceDelivery * BatteryHeavyWeight;
                        if (parcelToConnect.Weight == WeightCategories.Medium)
                            batteryDelivery += distanceDelivery * BatteryMediumWeight;
                        if (parcelToConnect.Weight == WeightCategories.Light)
                            batteryDelivery += distanceDelivery * BatteryLightWeight;

                        distanceDelivery = Distance(GetCustomer(GetParcel(parcelToConnect.Id).Target.Id).Location,
                            NearStationToCustomer(dal.GetCustomer(GetParcel(parcelToConnect.Id).Target.Id)).Location);
                        batteryDelivery += distanceDelivery * BatteryAvailable;

                        if (connectDrone.Battery < batteryDelivery) // if there is no enough battery delete the parcel from list
                        {
                            ParcelToList parcelToRemove = new ParcelToList();
                            foreach (var parcelInWeightParcel in weightParcel)
                                if (parcelInWeightParcel.Id == parcelToConnect.Id)
                                    parcelToRemove = parcelInWeightParcel;

                            weightParcel.Remove(parcelToRemove);
                        }
                        else 
                            break;
                    }
                }
            }

            IDAL.DO.Parcel updateParcel = new IDAL.DO.Parcel();
            try
            {
                updateParcel = dal.GetParcel(parcelToConnect.Id);
            }
            catch (DalObject.ParcelException )//if there is not available drone to carry the parcel
            {
                throw new ParcelException("There are no packages that the available drone you entered can carry\n .. " +
                                          "Please wait for other drones to be available or enter the identity of another available drone.");
            }

            foreach (var drone in ListDrones)
                if (drone.Id == connectDrone.Id)
                {
                    drone.Status = DroneStatuses.Delivery;
                    drone.IdParcel = updateParcel.Id;
                }

            updateParcel.DroneId = connectDrone.Id;
            updateParcel.Scheduled = DateTime.Now;
             
            dal.UpdateParcel(updateParcel);
        }

        /// <summary>
        /// Collection the parcel by the drone
        /// </summary>
        /// <param name="idDrone"></param>
        public void CollectionParcelByDrone(int idDrone)
        {
            Drone collectionDrone = new Drone();
            try
            {
                collectionDrone = GetDrone(idDrone);
            }
            catch (DroneException e)
            {
                throw new DroneException(e.Message, e);
            }

            if (collectionDrone.Status != DroneStatuses.Delivery && collectionDrone.ParcelByTransfer.Status != true)
                throw new DroneException("ERROR: The drone is not in delivery so he can not collect any parcel. ");

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

        /// <summary>
        /// Supply parcel by drone
        /// </summary>
        /// <param name="idDrone"></the id of the drone>
        public void SupplyParcelByDrone(int idDrone)
        {
            Drone drone = new Drone();
            try
            {
                drone = GetDrone(idDrone);
            }
            catch (DroneException e)
            {
                throw new DroneException(e.Message, e);
            }

            Parcel parcel = new Parcel();
            parcel = GetParcel(drone.ParcelByTransfer.Id);

            foreach (var elementParcelToList in GetParcels(parcel => true))
            {
                if (elementParcelToList.Id == parcel.Id)
                {
                    if (elementParcelToList.ParcelStatuses != ParcelStatuses.PickedUp &&
                        parcel.DroneInParcel.Id == idDrone)
                        throw new DroneException("ERROR: the drone is not pick-up the parcel yet\n");

                    double distance = Distance(drone.Location, GetCustomer(parcel.Target.Id).Location);

                    bool fullBattery = false;
                    if (parcel.Weight == WeightCategories.Heavy)
                        if (distance * BatteryHeavyWeight > 100)
                            fullBattery = true;
                        else
                            drone.Battery = distance * BatteryHeavyWeight;
                    
                        
                    if (parcel.Weight == WeightCategories.Medium)
                        if (distance * BatteryHeavyWeight > 100)
                            fullBattery = true;
                        else
                            drone.Battery = distance * BatteryMediumWeight;

                    if (parcel.Weight == WeightCategories.Light)
                        if (distance * BatteryHeavyWeight > 100) // if the battery is logical
                            fullBattery = true;
                        else
                            drone.Battery = distance * BatteryLightWeight;
                    if (fullBattery)
                        drone.Battery = 100;

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

                    parcel.Delivered = DateTime.Now;
                    IDAL.DO.Parcel updateParcel = new IDAL.DO.Parcel();
                    updateParcel = dal.GetParcel(parcel.Id);
                    updateParcel.Delivered = parcel.Delivered;
                    dal.UpdateParcel(updateParcel);
                    return;
                }
              
            }
        }

        /// <summary>
        /// check the input in add parcel to list
        /// </summary>
        /// <param name="parcel"></param>
        private void CheckParcel(Parcel parcel)
        {
            try
            {
                dal.GetCustomer(parcel.Sender.Id);
            }
            catch (DalObject.CustomerException )
            {
                throw new ParcelException("ERROR: the Sender customer not found! ");
            }
            try
            {
                dal.GetCustomer(parcel.Target.Id);
            }
            catch (DalObject.CustomerException )
            {
                throw new ParcelException("ERROR: the Target customer not found! ");
            }
            if (parcel.Sender.Id == parcel.Target.Id)
                throw new ParcelException("ERROR: the Target ID and the Sender ID are equals! ");
        }

        /// <summary>
        /// find the near parcel from all parcels to drone
        /// </summary>
        /// <param name="drone"></param>
        /// <param name="parcels"></param>
        /// <returns></returns>
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
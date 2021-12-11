using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
using IDAL;
using Customer = IDAL.DO.Customer;


namespace IBL
{
    public partial class BL : IBL
    {
        List<DroneToList> ListDrones = new List<DroneToList>();

        IDal dal;

        double BatteryAvailable, BatteryLightWeight, BatteryMediumWeight, BatteryHeavyWeight, ChargingRateOfDrone;

        Random rand = new Random(DateTime.Now.Millisecond);

        /// <summary>
        /// constractur of the BL
        /// </summary>
        /// <returns></no returns >
        public BL()
        {
            dal = new DalObject.DalObject();

            // km per hour
            double[] powerConsumption = dal.GetRequestPowerConsumption();
            BatteryAvailable = powerConsumption[0];
            BatteryLightWeight = powerConsumption[1];
            BatteryMediumWeight = powerConsumption[2];
            BatteryHeavyWeight = powerConsumption[3];
            ChargingRateOfDrone = powerConsumption[4];

            IEnumerable<IDAL.DO.Drone> listDronesIdalDo = dal.GetDrones(drone => true);
            foreach (var elementDrone in listDronesIdalDo)
            {
                DroneToList newDrone = new DroneToList();
                newDrone.Id = elementDrone.Id;
                newDrone.Model = elementDrone.Model;
                newDrone.Weight = (WeightCategories)elementDrone.Weight;
                ListDrones.Add(newDrone);
            }

            IEnumerable<IDAL.DO.Parcel> ListParcelsIdalDo = dal.GetParcels(parcel => true);

            for (int i = 0; i < ListDrones.Count(); i++)
            {  
                DroneToList drone = new DroneToList();
                drone = ListDrones[i];
                foreach (var parcel in ListParcelsIdalDo)
                {
                    if (drone.Id == parcel.DroneId && parcel.Scheduled != null && parcel.Delivered == null) //if the parcel are not in deliver and the drone is connect
                    {
                        double batteryDelivery = 0;
                        if (parcel.PickedUp == null)
                        {
                            drone.Location = NearStationToCustomer(GetCustomer(parcel.SenderId)).Location;
                            batteryDelivery += Distance(drone.Location, GetCustomer(parcel.SenderId).Location) * BatteryAvailable;
                        }
                        else // if the parcel is pick up
                            drone.Location = new Location() { Longitude = dal.GetCustomer(parcel.SenderId).Longitude, Latitude = dal.GetCustomer(parcel.SenderId).Latitude }; //the location of the sender

                        // the distance between the sender and the target
                        if (parcel.Weight == IDAL.DO.WeightCategories.Heavy)
                            batteryDelivery += Distance(GetCustomer(parcel.SenderId).Location, GetCustomer(parcel.TargetId).Location) * BatteryHeavyWeight;
                        if (parcel.Weight == IDAL.DO.WeightCategories.Medium)
                            batteryDelivery += Distance(GetCustomer(parcel.SenderId).Location, GetCustomer(parcel.TargetId).Location) * BatteryMediumWeight;
                        if (parcel.Weight == IDAL.DO.WeightCategories.Light)
                            batteryDelivery += Distance(GetCustomer(parcel.SenderId).Location, GetCustomer(parcel.TargetId).Location) * BatteryLightWeight;

                        batteryDelivery += Distance(GetCustomer(parcel.TargetId).Location, NearStationToCustomer(GetCustomer(parcel.TargetId)).Location) * BatteryAvailable;

                        drone.Battery = (100 - batteryDelivery) * rand.NextDouble() + batteryDelivery;
                        drone.Battery = (double)System.Math.Round(drone.Battery, 3);

                        drone.Status = DroneStatuses.Delivery;
                        drone.IdParcel = parcel.Id;
                    }
                }

                if (drone.Status != DroneStatuses.Delivery)
                    drone.Status = (DroneStatuses) rand.Next(0, 2);

                if (drone.Status == DroneStatuses.Maintenance)
                {
                    drone.Status = DroneStatuses.Available; // for the charge, after he will be in Maintenance.
                    IEnumerable<IDAL.DO.Station> listStationsIdalDo = dal.GetStations(s => true);
                    int index = rand.Next(0, listStationsIdalDo.Count());
                    drone.Location = new Location() { Longitude = listStationsIdalDo.ElementAt(index).Longitude, Latitude = listStationsIdalDo.ElementAt(index).Latitude };

                    drone.Battery = 20 * rand.NextDouble();
                    drone.Battery = (double)System.Math.Round(drone.Battery, 3);

                    drone.IdParcel = 0;
                    try
                    {
                        SendDroneToDroneCharge(drone.Id);
                    }
                    catch (DroneException) // there is no throw, for the program continue the constructor.
                    { }
                }

                if (drone.Status == DroneStatuses.Available)
                {
                    IEnumerable<IDAL.DO.Customer> customersWithDelivery = new List<IDAL.DO.Customer>();
                    foreach (var parcel in dal.GetParcels(parcel => true))
                        customersWithDelivery = dal.GetCustomers(customer => customer.Id == parcel.TargetId && parcel.Delivered != null);

                    if (customersWithDelivery.Any())
                    {
                        int index = rand.Next(0, customersWithDelivery.Count());
                        drone.Location = new Location() { Longitude = customersWithDelivery.ElementAt(index).Longitude, Latitude = customersWithDelivery.ElementAt(index).Latitude };
                    }
                    else
                    {
                        int index = rand.Next(0, GetStations().Count());
                        Station station = GetStation(GetStations().ElementAt(index).Id);
                        drone.Location = new Location() { Longitude = station.Location.Longitude, Latitude = station.Location.Latitude }; // put in rand station
                    }

                    double batteryToNearStation = 0;

                    batteryToNearStation = Distance(drone.Location, NearStationToDrone(GetDrone(drone.Id)).Location) * BatteryAvailable;

                    drone.Battery = (100 - batteryToNearStation) * rand.NextDouble() + batteryToNearStation;
                    drone.Battery = (double)System.Math.Round(drone.Battery, 3);

                    drone.IdParcel = 0;
                }
                ListDrones[i] = drone;
            }
        }

        /// <summary>
        /// the distances from "FROM" to "TO"
        /// </summary>
        /// <returns></returns the distance on double type>
        private double Distance(Location from, Location to)
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


//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Net.Sockets;
//using System.Security.Cryptography.X509Certificates;
//using System.Text;
//using System.Threading.Tasks;
//using IBL.BO;
//using IDAL;


//namespace IBL
//{
//    public partial class BL : IBL
//    {
//        /// <summary>
//        /// add parcel with all fields to data source with checking 
//        /// </summary>
//        /// <param name="newParcel"></param>
//        public void AddParcel(Parcel newParcel)
//        {
//            try
//            {
//                CheckParcel(newParcel);
//            }
//            catch (ParcelException e)
//            {
//                throw new ParcelException(e.Message, e);
//            }
//            IDAL.DO.Parcel parcel = new IDAL.DO.Parcel();
//            parcel.SenderId = newParcel.Sender.Id;
//            parcel.TargetId = newParcel.Target.Id;
//            parcel.Weight = (IDAL.DO.WeightCategories)newParcel.Weight;
//            parcel.Priority = (IDAL.DO.Priorities)newParcel.Priority;
//            parcel.DroneId = 0;
//            parcel.Requested = DateTime.Now;
//            parcel.Scheduled = null;
//            parcel.PickedUp = null;
//            parcel.Delivered = null;
//            dal.AddParcel(parcel);
//        }

//        /// <summary>
//        /// send id of parcel and checking that it exist.
//        /// make special entity and return it
//        /// </summary>
//        /// <param name="id"></param>
//        /// <returns></returns>
//        public Parcel GetParcel(int id)
//        {
//            IDAL.DO.Parcel idalParcel = new IDAL.DO.Parcel();
//            try
//            {
//                idalParcel = dal.GetParcel(id);
//            }
//            catch (DalObject.ParcelException e)
//            {
//                throw new ParcelException(e.Message, e);
//            }

//            Parcel parcel = new Parcel();
//            parcel.Id = idalParcel.Id;
//            parcel.Sender = new CustomerInParcel()
//            { Id = idalParcel.SenderId, NameCustomer = GetCustomer(idalParcel.SenderId).Name };
//            parcel.Target = new CustomerInParcel()
//            { Id = idalParcel.TargetId, NameCustomer = GetCustomer(idalParcel.TargetId).Name };
//            parcel.Weight = (WeightCategories)idalParcel.Weight;
//            parcel.Priority = (Priorities)idalParcel.Priority;

//            parcel.DroneInParcel = new DroneInParcel()
//            { Id = 0, Battery = 0, Location = new Location() { Longitude = 0, Latitude = 0 } };

//            foreach (var elementDrone in ListDrones)
//                if (elementDrone.Id == idalParcel.DroneId)
//                {
//                    parcel.DroneInParcel = new DroneInParcel()
//                    { Id = elementDrone.Id, Battery = elementDrone.Battery, Location = elementDrone.Location };
//                }

//            parcel.Requested = idalParcel.Requested;
//            parcel.Scheduled = idalParcel.Scheduled;
//            parcel.PickedUp = idalParcel.PickedUp;
//            parcel.Delivered = idalParcel.Delivered;

//            return parcel;
//        }

//        /// <summary>
//        /// return the list of parcels in special entity for show
//        /// </summary>
//        /// <returns></returns>
//        public IEnumerable<ParcelToList> GetParcels()
//        {
//            List<ParcelToList> parcelToLists = new List<ParcelToList>();

//            foreach (var idalParcel in dal.GetParcels(parcel => true))
//            {
//                ParcelToList newParcel = new ParcelToList();
//                newParcel.Id = idalParcel.Id;
//                newParcel.SenderName = GetCustomer(idalParcel.SenderId).Name;
//                newParcel.TargetName = GetCustomer(idalParcel.TargetId).Name;
//                newParcel.Weight = (WeightCategories)idalParcel.Weight;
//                newParcel.Priority = (Priorities)idalParcel.Priority;

//                if (idalParcel.Requested != null)
//                    newParcel.ParcelStatuses = ParcelStatuses.Requested;
//                if (idalParcel.Scheduled != null)
//                    newParcel.ParcelStatuses = ParcelStatuses.Scheduled;
//                if (idalParcel.PickedUp != null)
//                    newParcel.ParcelStatuses = ParcelStatuses.PickedUp;
//                if (idalParcel.Delivered != null)
//                    newParcel.ParcelStatuses = ParcelStatuses.Delivered;

//                parcelToLists.Add(newParcel);
//            }

//            return parcelToLists;
//        }

//        /// <summary>
//        /// Returning the list of parcels with no drones in a special entity "Parcel to list".
//        /// </summary>
//        /// <returns></returns>
//        public IEnumerable<ParcelToList> GetParcelsNoDrones()
//        {
//            List<ParcelToList> parcelToLists = new List<ParcelToList>();

//            foreach (var idalParcel in dal.GetParcels(parcel => parcel.Scheduled == null && parcel.PickedUp == null && parcel.Delivered == null)) // just parcels that dont have them drone.
//            {
//                ParcelToList newParcel = new ParcelToList();
//                newParcel.Id = idalParcel.Id;
//                newParcel.SenderName = GetCustomer(idalParcel.SenderId).Name;
//                newParcel.TargetName = GetCustomer(idalParcel.TargetId).Name;
//                newParcel.Weight = (WeightCategories)idalParcel.Weight;
//                newParcel.Priority = (Priorities)idalParcel.Priority;

//                if (idalParcel.Requested != null)
//                    newParcel.ParcelStatuses = ParcelStatuses.Requested;
//                if (idalParcel.Scheduled != null)
//                    newParcel.ParcelStatuses = ParcelStatuses.Scheduled;
//                if (idalParcel.PickedUp != null)
//                    newParcel.ParcelStatuses = ParcelStatuses.PickedUp;
//                if (idalParcel.Delivered != null)
//                    newParcel.ParcelStatuses = ParcelStatuses.Delivered;

//                parcelToLists.Add(newParcel);
//            }

//            return parcelToLists;
//        }

//        /// <summary>
//        /// find parcel in the data conditions and connect it to the drone
//        /// </summary>
//        /// <param name="droneId"></param>
//        public void ConnectParcelToDrone(int droneId)
//        {
//            Drone connectDrone = new Drone();
//            try
//            {
//                connectDrone = GetDrone(droneId);
//            }
//            catch (DroneException e)
//            {
//                throw new DroneException(e.Message, e);
//            }

//            if (connectDrone.Status != DroneStatuses.Available)
//                throw new DroneException("ERROR: The drone is not available:\n ");

//            List<ParcelToList> ParcelsDroneCanCarry = new List<ParcelToList>();
//            double distanceDelivery, batteryDelivery;
//            foreach (var parcel in GetParcelsNoDrones())
//            {
//                distanceDelivery = Distance(connectDrone.Location, GetCustomer(GetParcel(parcel.Id).Sender.Id).Location);
//                batteryDelivery = distanceDelivery * BatteryAvailable;

//                distanceDelivery = Distance(GetCustomer(GetParcel(parcel.Id).Sender.Id).Location, GetCustomer(GetParcel(parcel.Id).Target.Id).Location); // the distance between the drone and the target
//                if (parcel.Weight == WeightCategories.Heavy)
//                    batteryDelivery += distanceDelivery * BatteryHeavyWeight;
//                if (parcel.Weight == WeightCategories.Medium)
//                    batteryDelivery += distanceDelivery * BatteryMediumWeight;
//                if (parcel.Weight == WeightCategories.Light)
//                    batteryDelivery += distanceDelivery * BatteryLightWeight;

//                distanceDelivery = Distance(GetCustomer(GetParcel(parcel.Id).Target.Id).Location, NearStationToCustomer(GetCustomer(GetParcel(parcel.Id).Target.Id)).Location);
//                batteryDelivery += distanceDelivery * BatteryAvailable;

//                if (connectDrone.Battery >= batteryDelivery) // if there is enough battery add the parcel to list
//                    ParcelsDroneCanCarry.Add(parcel);
//            }

//            ParcelsDroneCanCarry.OrderBy(parcel => parcel.Priority).ThenBy(parcel => parcel.Weight); // sort by priority and after by weight.
//            ParcelsDroneCanCarry.OrderByDescending(parcel => Distance(connectDrone.Location, GetCustomer(GetParcel(parcel.Id).Sender.Id).Location)); // sort by min distance.

//            ParcelToList parcelToConnect = new ParcelToList();
//            if (ParcelsDroneCanCarry.Count() != 0)
//                parcelToConnect = ParcelsDroneCanCarry.First();
//            else
//                throw new ParcelException("There are no packages that the available drone you entered can carry..\n" +
//                                          "Please wait for other drones to be available or enter the identity of another available drone.");

//            //IEnumerable<ParcelToList> parcelNoDrones = GetParcelsNoDrones();
//            //List<ParcelToList> prioritiesParcel = new List<ParcelToList>();
//            //List<ParcelToList> weightParcel = new List<ParcelToList>();
//            //Parcel parcelToConnect = new Parcel();
//            //double batteryDelivery = 0;

//            //for (int i = (int)Priorities.Emergency; i >= (int)Priorities.Normal; i--)
//            //{
//            //    prioritiesParcel = parcelNoDrones.ToList().FindAll(parcel => parcel.Priority == (Priorities)i); // parcels with priority according to the brace
//            //    for (int j = (int) connectDrone.Weight; j >= (int) WeightCategories.Light; j--)
//            //    {
//            //        weightParcel = prioritiesParcel.ToList().FindAll(parcel => parcel.Weight == (WeightCategories) j); // parcels with weight according to the brace
//            //        while (weightParcel.Count() != 0)
//            //        {
//            //            parcelToConnect = new Parcel();
//            //            parcelToConnect = NearParcelToDrone(connectDrone, weightParcel); // find the close station

//            //            double distanceDelivery = Distance(connectDrone.Location,
//            //                GetCustomer(GetParcel(parcelToConnect.Id).Sender.Id).Location);

//            //            batteryDelivery = distanceDelivery * BatteryAvailable;

//            //            distanceDelivery = Distance(GetCustomer(GetParcel(parcelToConnect.Id).Sender.Id).Location,
//            //                GetCustomer(GetParcel(parcelToConnect.Id).Target.Id).Location); // the distance between the drone and the target
//            //            if (parcelToConnect.Weight == WeightCategories.Heavy)
//            //                batteryDelivery += distanceDelivery * BatteryHeavyWeight;
//            //            if (parcelToConnect.Weight == WeightCategories.Medium)
//            //                batteryDelivery += distanceDelivery * BatteryMediumWeight;
//            //            if (parcelToConnect.Weight == WeightCategories.Light)
//            //                batteryDelivery += distanceDelivery * BatteryLightWeight;

//            //            distanceDelivery = Distance(GetCustomer(GetParcel(parcelToConnect.Id).Target.Id).Location,
//            //                NearStationToCustomer(dal.GetCustomer(GetParcel(parcelToConnect.Id).Target.Id)).Location);
//            //            batteryDelivery += distanceDelivery * BatteryAvailable;

//            //            if (connectDrone.Battery < batteryDelivery) // if there is no enough battery delete the parcel from list
//            //            {
//            //                ParcelToList parcelToRemove = new ParcelToList();
//            //                foreach (var parcelInWeightParcel in weightParcel)
//            //                    if (parcelInWeightParcel.Id == parcelToConnect.Id)
//            //                        parcelToRemove = parcelInWeightParcel;

//            //                weightParcel.Remove(parcelToRemove);
//            //            }
//            //            else 
//            //                break;
//            //        }
//            //    }
//            //}

//            IDAL.DO.Parcel updateParcel = new IDAL.DO.Parcel();
//            try
//            {
//                updateParcel = dal.GetParcel(parcelToConnect.Id);
//            }
//            catch (DalObject.ParcelException)//if there is not available drone to carry the parcel
//            {
//                throw new ParcelException("There are no packages that the available drone you entered can carry..\n" +
//                                          "Please wait for other drones to be available or enter the identity of another available drone.");
//            }

//            foreach (var drone in ListDrones)
//                if (drone.Id == connectDrone.Id)
//                {
//                    drone.Status = DroneStatuses.Delivery;
//                    drone.IdParcel = updateParcel.Id;
//                }

//            updateParcel.DroneId = connectDrone.Id;
//            updateParcel.Scheduled = DateTime.Now;

//            dal.UpdateParcel(updateParcel);
//        }

//        /// <summary>
//        /// Collection the parcel by the drone
//        /// </summary>
//        /// <param name="idDrone"></param>
//        public void CollectionParcelByDrone(int idDrone)
//        {
//            Drone collectionDrone = new Drone();
//            try
//            {
//                collectionDrone = GetDrone(idDrone);
//            }
//            catch (DroneException e)
//            {
//                throw new DroneException(e.Message, e);
//            }

//            if (collectionDrone.Status != DroneStatuses.Delivery && collectionDrone.ParcelByTransfer.Status != true)
//                throw new DroneException("ERROR: The drone is not in delivery so he can not collect any parcel. ");

//            for (int i = 0; i < ListDrones.Count(); i++)
//            {
//                if (ListDrones[i].Id == collectionDrone.Id)
//                {
//                    DroneToList updateDrone = ListDrones[i];
//                    updateDrone.Battery -= Distance(collectionDrone.Location,
//                        collectionDrone.ParcelByTransfer.PickUpLocation) * BatteryAvailable;
//                    updateDrone.Location = collectionDrone.ParcelByTransfer.PickUpLocation;
//                    ListDrones[i] = updateDrone;
//                }
//            }

//            IDAL.DO.Parcel updateParcel = dal.GetParcel(collectionDrone.ParcelByTransfer.Id);
//            updateParcel.PickedUp = DateTime.Now;
//            dal.UpdateParcel(updateParcel);
//        }

//        /// <summary>
//        /// Supply parcel by drone
//        /// </summary>
//        /// <param name="idDrone"></the id of the drone>
//        public void SupplyParcelByDrone(int idDrone)
//        {
//            Drone drone = new Drone();
//            try
//            {
//                drone = GetDrone(idDrone);
//            }
//            catch (DroneException e)
//            {
//                throw new DroneException(e.Message, e);
//            }

//            Parcel parcel = new Parcel();
//            parcel = GetParcel(drone.ParcelByTransfer.Id);

//            foreach (var elementParcel in GetParcels())
//            {
//                if (elementParcel.Id == parcel.Id)
//                {
//                    if (elementParcel.ParcelStatuses != ParcelStatuses.PickedUp &&
//                        parcel.DroneInParcel.Id == idDrone)
//                        throw new DroneException("ERROR: the drone is not pick-up the parcel yet\n");

//                    double distance = Distance(drone.Location, GetCustomer(parcel.Target.Id).Location);

//                    if (parcel.Weight == WeightCategories.Heavy)
//                        drone.Battery -= distance * BatteryMediumWeight;
//                    else
//                    {
//                        if (parcel.Weight == WeightCategories.Medium)
//                            drone.Battery -= distance * BatteryHeavyWeight;
//                        else // light
//                            drone.Battery -= distance * BatteryLightWeight;
//                    }

//                    drone.Location = GetCustomer(parcel.Target.Id).Location;
//                    drone.Status = DroneStatuses.Available;
//                    for (int i = 0; i < ListDrones.Count(); i++)
//                    {
//                        if (ListDrones[i].Id == drone.Id)
//                        {
//                            DroneToList updateDrone = ListDrones[i];
//                            updateDrone.Battery = drone.Battery;
//                            updateDrone.Location = drone.Location;
//                            updateDrone.Status = drone.Status;
//                            updateDrone.IdParcel = 0;
//                            ListDrones[i] = updateDrone;
//                        }
//                    }

//                    IDAL.DO.Parcel updateParcel = dal.GetParcel(parcel.Id);
//                    updateParcel.Delivered = DateTime.Now;
//                    dal.UpdateParcel(updateParcel);
//                }
//            }
//        }

//        /// <summary>
//        /// check the input in add parcel to list
//        /// </summary>
//        /// <param name="parcel"></param>
//        private void CheckParcel(Parcel parcel)
//        {
//            try
//            {
//                dal.GetCustomer(parcel.Sender.Id);
//            }
//            catch (DalObject.CustomerException)
//            {
//                throw new ParcelException("ERROR: the Sender customer not found! ");
//            }
//            try
//            {
//                dal.GetCustomer(parcel.Target.Id);
//            }
//            catch (DalObject.CustomerException)
//            {
//                throw new ParcelException("ERROR: the Target customer not found! ");
//            }
//            if (parcel.Sender.Id == parcel.Target.Id)
//                throw new ParcelException("ERROR: the Target ID and the Sender ID are equals! ");
//        }

//        /// <summary>
//        /// find the near parcel from all parcels to drone
//        /// </summary>
//        /// <param name="drone"></param>
//        /// <param name="parcels"></param>
//        /// <returns></returns>
//        private Parcel NearParcelToDrone(Drone drone, IEnumerable<ParcelToList> parcels)
//        {
//            List<double> distancesList = new List<double>();
//            Location parcelLocation = new Location();
//            Location droneLocation = drone.Location;


//            foreach (var parcel in parcels)
//                if (parcel.ParcelStatuses == ParcelStatuses.Requested)
//                {
//                    var senderParcel = GetCustomer(GetParcel(parcel.Id).Sender.Id);
//                    distancesList.Add(Distance(senderParcel.Location, droneLocation));
//                }

//            double minDistance = distancesList.Min();

//            Parcel nearparcel = new Parcel();
//            foreach (var parcel in parcels)
//                if (parcel.ParcelStatuses == ParcelStatuses.Requested)
//                {
//                    var senderParcel = GetCustomer(GetParcel(parcel.Id).Sender.Id);
//                    if (minDistance == Distance(senderParcel.Location, droneLocation))
//                        nearparcel = GetParcel(parcel.Id);
//                }

//            return nearparcel;
//        }
//    }
//}

using System;
using IDAL.DO;
using System.Collections.Generic;

namespace DalObject
{
    public class DalObject
    {
        public DalObject()
        {
            DataSource.Initialize();
        }

        /// <summary>
        /// add a staion to the list station
        /// </summary>
        /// <param name="newStation"></the new station the user whants to add to the station's list>
        public static void AddStation(Station newStation)
        {
            if(checkNotExistStation(newStation, DataSource.stations))
                DataSource.stations.Add(newStation);
        }

        /// <summary>
        /// add a drone to the drone list
        /// </summary>
        /// <param name="newDrone"></the new drone the user whants to add to the drone's list>
        public static void AddDrone(Drone newDrone)
        {
            if(checkNotExistDrone(newDrone, DataSource.drones))
                DataSource.drones.Add(newDrone);
        }

        /// <summary>
        /// add a parcel to the parcel list and return the new parcel id that was create
        /// </summary>
        /// <param name="newParcel"></the new parcel the user whants to add to the parcel's list>
        /// <returns></returns>
        public static int AddParcel(Parcel newParcel)
        {
            int tmp = DataSource.Config.ParcelsId;
            if (checkNotExistParcel(newParcel, DataSource.parcels))
            {
                newParcel.id = DataSource.Config.ParcelsId; // insert the Parcels new Id
                DataSource.Config.ParcelsId++; // new Id for the fautre parce Id
                DataSource.parcels.Add(newParcel);
            }
            return tmp; // return the new number created
        }

        /// <summary>
        /// add a customer to the fustomer list
        /// </summary>
        /// <param name="newCustomer"></the new customer the user whants to add to the customer's list>
        public static void AddCustomer(Customer newCustomer)
        {
            if (checkNotExistCustomer(newCustomer, DataSource.customers))
                DataSource.customers.Add(newCustomer);
        }

        /// <summary>
        /// return the spesifice station the user ask for
        /// </summary>
        /// <param name="stationId"></the id of the station the user ask for>
        /// <returns></returns>
        public static Station GetStation(int stationId)
        {
            Station newStation = new Station();
            foreach (Station elementStation in DataSource.stations)
            {
                if (elementStation.id == stationId)
                    newStation = elementStation;
            }
            return newStation;
        }

        /// <summary>
        /// return the spesifice drone the user ask for
        /// </summary>
        /// <param name="droneId"></the id of the drone the user ask for>
        /// <returns></returns>
        public static Drone GetDrone(int droneId)
        {
            Drone newDrone = new Drone();
            foreach (Drone elementDrone in DataSource.drones)
            {
                if (elementDrone.id == droneId)
                    newDrone = elementDrone;
            }
            return newDrone;
        }

        /// <summary>
        /// return the spesifice parcel the user ask for
        /// </summary>
        /// <param name="parcelId"></the id parcel the user ask for>
        /// <returns></returns>
        public static Parcel GetParcel(int parcelId)
        {
            Parcel newParcel = new Parcel();
            foreach (Parcel elementParcel in DataSource.parcels)
            {
                if (elementParcel.id == parcelId)
                    newParcel = elementParcel;
            }
            return newParcel;
        }

        /// <summary>
        /// return the spesifice customer the user ask for
        /// </summary>
        /// <param name="customerId"></the id of the customer the user ask for>
        /// <returns></returns>
        public static Customer GetCustomer(int customerId)
        {
            Customer newCustomer = new Customer();
            foreach (Customer elementCustomer in DataSource.customers)
            {
                if (elementCustomer.id == customerId)
                    newCustomer = elementCustomer;
            }
            return newCustomer;
        }

        /// <summary>
        /// return all the list of the station's
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Station> GetStations()
        {
            List<Station> newStations = new List<Station>(DataSource.stations);
            return newStations;
        }

        /// <summary>
        /// return all the drone's list
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Drone> GetDrones()
        {
            List<Drone> newDrones = new List<Drone>(DataSource.drones);
            return newDrones;
        }
        
        /// <summary>
        /// return all the list of the drone's that they are in charge sopt 
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<DroneCharge> GetDronesCharge()
        {
            List<DroneCharge> newDronesCharge = new List<DroneCharge>(DataSource.droneCharges);
            return newDronesCharge;
        }

        /// <summary>
        /// return all the parcel in the list
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Parcel> GetParcels()
        {
            List<Parcel> newParcels = new List<Parcel>(DataSource.parcels);
            return newParcels;
        }
  
        /// <summary>
        /// return all the customer list
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Customer> GetCustomers()
        {
            List<Customer> newCustomers = new List<Customer>(DataSource.customers);
            return newCustomers;
        }

        /// <summary>
        /// connecting between parcel and drone and chenge data in the object's according to the function
        /// </summary>
        /// <param name="p"></the parcel the user ask to connect with the drone he ask>
        /// <param name="d"></the drone the user ask to connect with the parcel he ask >
        public static void ConnectParcelToDrone(Parcel p, Drone d)
        {
            if (!checkNotExistParcel(p, DataSource.parcels) && !checkNotExistDrone(p, DataSource.drones))
            {
                Parcel newParcel = new Parcel();
                for (int i = 0; i < DataSource.parcels.Count; i++)
                {
                    if (DataSource.parcels[i].id == p.id)
                    {
                        newParcel = DataSource.parcels[i];
                        newParcel.requested = DateTime.Now;
                        newParcel.scheduled = DateTime.Now;
                        newParcel.droneId = d.id;
                        DataSource.parcels[i] = newParcel;
                    }
                }
            }
        }

        /// <summary>
        /// chenge the pick up statuse of the parcel the user ask for by searching the parcel in the list and update the pick up time statuse of the parcel
        /// </summary>
        /// <param name="p"></the spesific parcel the user ask to update as pick'd up>
        public static void CollectionParcelByDrone(Parcel p)
        {
            if (!checkNotExistParcel(p, DataSource.parcels))
            {
                Parcel newParcel = new Parcel();
                for (int i = 0; i < DataSource.parcels.Count; i++)
                {
                    
                }
                foreach (Parcel elementParcel in DataSource.parcels)
                {
                    if (elementParcel.id == p.id)
                    {
                        newParcel = elementParcel;
                        DataSource.parcels.Remove(elementParcel);
                        break;
                    }
                }
                newParcel.pickedUp = DateTime.Now;
                DataSource.parcels.Add(newParcel);
            }
        }

        /// <summary>
        /// chenge the deliversd statuse of the parcel the user ask for by searching the parcel in the list and update the deliverd time statuse of the parcel
        /// </summary>
        /// <param name="p"></the specific parcel the user ask to pdate as deliver'd >
        public static void SupplyParcelToCustomer(Parcel p)
        {
            if (!checkNotExistParcel(p, DataSource.parcels))
            {
                Parcel newParcel = new Parcel();
                foreach (Parcel elementParcel in DataSource.parcels)
                {
                    if (elementParcel.id == p.id)
                    {
                        newParcel = elementParcel;
                        DataSource.parcels.Remove(elementParcel);
                        break;
                    }
                }
                newParcel.delivered = DateTime.Now;
                DataSource.parcels.Add(newParcel);
            }
        }

        /// <summary>
        /// send a spesifice drone to a spesifice station that have a availble charge spots, and change the status of the drome to maintenance
        /// </summary>
        /// <param name="d"></the spesifice drone the user ask for to charge>
        /// <param name="s"></the spasifice station the user ask to charge the drone in>
        public static void SendDroneToDroneCharge(Station s, Drone d)
        {
            DroneCharge newDroneCharges = new DroneCharge();
            if (!checkNotExistStation(s, DataSource.stations))
            {
                Station newsStation = new Station();
                foreach (Station elementStation in DataSource.stations)
                {
                    if (elementStation.id == s.id)
                    {
                        newsStation = elementStation;
                        DataSource.stations.Remove(elementStation);
                        break;
                    }
                }
                newsStation.chargeSlots--;
                newDroneCharges.stationld = newsStation.id;
                DataSource.stations.Add(newsStation);
            }

            if (!checkNotExistDrone(d, DataSource.drones))
            {
                foreach (Drone elementDrone in DataSource.drones)
                {
                    if (elementDrone.id == d.id)
                    {
                        newDroneCharges.droneId = elementDrone.id;
                        break;
                    }
                }
            }
            DataSource.droneCharges.Add(newDroneCharges);
        }

        /// <summary>
        /// release the spesifice drone frome the station he located and update the battry and status to 100 and available
        /// </summary>
        /// <param name="s"></the spesifice station the user ask to release the drone frome>
        /// <param name="d"></the spesifice drone the user ask to release frome the station>
        public static void ReleaseDroneFromDroneCharge(Station s,Drone d)
        {
            DroneCharge newDroneCharges = new DroneCharge();
            if (!checkNotExistStation(s, DataSource.stations))
            {
                Station newsStation = new Station();
                foreach (Station elementStation in DataSource.stations)
                {
                    if (elementStation.id == s.id)
                    {
                        newsStation = elementStation;
                        DataSource.stations.Remove(elementStation);
                        break;
                    }
                }
                newsStation.chargeSlots++;
                newDroneCharges.stationld = newsStation.id;
                DataSource.stations.Add(newsStation);
            }

            if (!checkNotExistDrone(d, DataSource.drones))
            {
                foreach (Drone elementDrone in DataSource.drones)
                {
                    if (elementDrone.id == d.id)
                    {
                        newDroneCharges.droneId = elementDrone.id;
                        break;
                    }
                }
            }

            foreach (DroneCharge elementDroneCharge in DataSource.droneCharges)
            {
                if (elementDroneCharge.stationld == s.id && elementDroneCharge.droneId == d.id)
                    DataSource.droneCharges.Remove(newDroneCharges);
            }
        }
    }
}




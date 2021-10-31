using System;
using IDAL.DO;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

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
            else
                throw new stationExeption("the station exist!\n");
        }

        /// <summary>
        /// add a drone to the drone list
        /// </summary>
        /// <param name="newDrone"></the new drone the user whants to add to the drone's list>
        public static void AddDrone(Drone newDrone)
        {
            if (checkNotExistDrone(newDrone, DataSource.drones))
                DataSource.drones.Add(newDrone);
            else
                throw new droneExeption("the drone is exist!\n");
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
            else
                throw new parcelExeption("the parcel is exist!\n");
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
            else
                throw new customerExeption("the customer is exist!\n");
        }

        /// <summary>
        /// return the spesifice station the user ask for
        /// </summary>
        /// <param name="stationId"></the id of the station the user ask for>
        /// <returns></returns>
        public static Station GetStation(int stationId)
        {
            Station? newStation = null;
            foreach (Station elementStation in DataSource.stations)
            {
                if (elementStation.id == stationId)
                    newStation = elementStation;
            }

            if (newStation == null)
                throw new stationExeption("Id of station not found\n");
            return (Station)newStation;
        }

        /// <summary>
        /// return the spesifice drone the user ask for
        /// </summary>
        /// <param name="droneId"></the id of the drone the user ask for>
        /// <returns></returns>
        public static Drone GetDrone(int droneId)
        {
            Drone? newDrone = null;
            foreach (Drone elementDrone in DataSource.drones)
            {
                if (elementDrone.id == droneId)
                    newDrone = elementDrone;
            }

            if (newDrone == null)
                throw new droneExeption("Id of drone not found\n");
            return (Drone)newDrone;
        }

        /// <summary>
        /// return the spesifice parcel the user ask for
        /// </summary>
        /// <param name="parcelId"></the id parcel the user ask for>
        /// <returns></returns>
        public static Parcel GetParcel(int parcelId)
        {
            Parcel ?newParcel = null;
            foreach (Parcel elementParcel in DataSource.parcels)
            {
                if (elementParcel.id == parcelId)
                    newParcel = elementParcel;
            }

            if (newParcel == null)
                throw new parcelExeption("Id of parcel not found\n");
            return (Parcel)newParcel;
        }

        /// <summary>
        /// return the spesifice customer the user ask for
        /// </summary>
        /// <param name="customerId"></the id of the customer the user ask for>
        /// <returns></returns>
        public static Customer GetCustomer(int customerId)
        {
            Customer ?newCustomer = null;
            foreach (Customer elementCustomer in DataSource.customers)
            {
                if (elementCustomer.id == customerId)
                    newCustomer = elementCustomer;
            }

            if (newCustomer == null)
                throw new customerExeption("Id of customer not found\n");
            return (Customer)newCustomer;
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
            if (!checkNotExistParcel(p, DataSource.parcels))
                if (!checkNotExistDrone(d, DataSource.drones))
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
                else
                    throw new droneExeption("the drone isn't exist");
            else
                throw new parcelExeption("the parcel isn't exist");
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
                    if (DataSource.parcels[i].id == p.id)
                    {
                        newParcel = DataSource.parcels[i];
                        newParcel.pickedUp = DateTime.Now;
                        DataSource.parcels[i] = newParcel;
                    }
                }
            }
            else
                throw new parcelExeption("the parcel is not exist!\n");
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
                for (int i = 0; i < DataSource.parcels.Count; i++)
                {
                    if (DataSource.parcels[i].id == p.id)
                    {
                        newParcel = DataSource.parcels[i];
                        newParcel.delivered = DateTime.Now;
                        DataSource.parcels[i] = newParcel;
                    }
                }
            }
            else
                throw new parcelExeption("the parcel isn't exist");
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
                for (int i = 0; i < DataSource.stations.Count; i++)
                {
                    if (DataSource.stations[i].id == s.id)
                    {
                        newsStation = DataSource.stations[i];
                        newsStation.chargeSlots--;
                        newDroneCharges.stationld = newsStation.id;
                        DataSource.stations[i] = newsStation;
                    }
                }
            }
            else
                throw new stationExeption("the station is not exist!\n");

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
                DataSource.droneCharges.Add(newDroneCharges);
            }
            else
                throw new droneExeption("the drone is not exist!\n");
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
                for (int i = 0; i < DataSource.stations.Count; i++) 
                {
                    if (DataSource.stations[i].id == s.id)
                    {
                        newsStation = DataSource.stations[i];
                        newsStation.chargeSlots++;
                        newDroneCharges.stationld = newsStation.id;
                        DataSource.stations[i] = newsStation;
                    }
                }
            }
            else
                throw new stationExeption("the station isn't exist");

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
            else
                throw new droneExeption("the drone isn't exist");

            foreach (DroneCharge elementDroneCharge in DataSource.droneCharges)
            {
                if (elementDroneCharge.stationld == s.id && elementDroneCharge.droneId == d.id)
                    DataSource.droneCharges.Remove(newDroneCharges);
            }
        }

        public static double[] GetRequestPowerConsumption()
        {
           double[] powerConsumption= new double[5];
           powerConsumption[0] = DataSource.Config.dAvailable;
           powerConsumption[1] = DataSource.Config.dLightW;
           powerConsumption[2] = DataSource.Config.dMediumW;
           powerConsumption[3] = DataSource.Config.dHeavyW;
           powerConsumption[4] = DataSource.Config.chargingRateOfDrone;
           return powerConsumption;
        }

        /// <summary>
        /// the methode not need exeption becuse she use both sids(true and false)
        /// </summary>
        /// <param name="s"></the station that we chek if she exist>
        /// <param name="stations"></the list of all stations>
        /// <returns></returns>
        public static bool checkNotExistStation(Station s, List<Station> stations)
        {
            foreach (Station elementStation in stations)
                if (elementStation.id == s.id)
                    return false;
            return true; //the station not exist
        }

        /// <summary>
        /// the methode not need exeption becuse she use both sids(true and false)
        /// </summary>
        /// <param name="d"></the drone we check if she is exist>
        /// <param name="drones"></the list od drones>
        /// <returns></returns>
        public static bool checkNotExistDrone(Drone d, List<Drone> drones)
        {
            foreach (Drone elementDrone in drones)
                if (elementDrone.id == d.id)
                    return false;
            return true;//the drone not exist
        }

        /// <summary>
        /// the methode not need exeption becuse she use both sids(true and false)
        /// </summary>
        /// <param name="d"></the parcel we check if she is exist>
        /// <param name="drones"></the list of parcels>
        /// <returns></returns>
        public static bool checkNotExistParcel(Parcel p, List<Parcel> parcels)
        {
            foreach (Parcel elementParcel in parcels)
                if (elementParcel.id == p.id)
                    return false;
            return true;//the drone not exist
        }

        /// <summary>
        /// the methode not need exeption becuse she use both sids(true and false)
        /// </summary>
        /// <param name="d"></the customer we check if she is exist>
        /// <param name="drones"></the list od customers>
        /// <returns></returns>
        public static bool checkNotExistCustomer(Customer c, List<Customer> customers)
        {
            foreach (Customer elementCustomer in customers)
                if (elementCustomer.id == c.id)
                    return false;

            return true; //the customer not exist
        }
    }
}
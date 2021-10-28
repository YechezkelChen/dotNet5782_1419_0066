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
            if (DataSource.Config.VacantIndexS == DataSource.stations.Length) // if there is no place in array
            {
                Station[] newArrayStations = new Station[DataSource.Config.VacantIndexS * 2]; // increse the place in array double 2
                for (int i = 0; i < DataSource.stations.Length; i++)
                    newArrayStations[i] = DataSource.stations[i]; // copy the old array to new array
                DataSource.stations = newArrayStations;
            }
            DataSource.stations[DataSource.Config.VacantIndexS] = newStation; // insert to the last place in array
            DataSource.Config.VacantIndexS++; // the new spote of the empty index in array
        }

        /// <summary>
        /// add a drone to the drone list
        /// </summary>
        /// <param name="newDrone"></the new drone the user whants to add to the drone's list>
        public static void AddDrone(Drone newDrone)
        {
            if (DataSource.Config.VacantIndexD == DataSource.drones.Length) // if there is no place in array
            {
                Drone[] newArrayDrones = new Drone[DataSource.Config.VacantIndexD * 2]; // increse the place in array double 2
                for (int i = 0; i < DataSource.drones.Length; i++)
                    newArrayDrones[i] = DataSource.drones[i]; // copy the old array to new array
                DataSource.drones = newArrayDrones;
            }
            DataSource.drones[DataSource.Config.VacantIndexD] = newDrone; // insert to the last place in array
            DataSource.Config.VacantIndexD++; // the new spote of the empty index in array
        }

        /// <summary>
        /// add a parcel to the parcel list and return the new parcel id that was create
        /// </summary>
        /// <param name="newParcel"></the new parcel the user whants to add to the parcel's list>
        /// <returns></returns>
        public static int AddParcel(Parcel newParcel)
        {
            if (DataSource.Config.VacantIndexP == DataSource.parcels.Length) // if there is no place in array
            {
                Parcel[] newArrayParcels = new Parcel[DataSource.Config.VacantIndexP * 2]; // increse the place in array double 2
                for (int i = 0; i < DataSource.parcels.Length; i++)
                    newArrayParcels[i] = DataSource.parcels[i]; // copy the old array to new array
                DataSource.parcels = newArrayParcels;
            }

            DataSource.parcels[DataSource.Config.VacantIndexP] = newParcel; // insert to the last place in array
            DataSource.Config.VacantIndexP++; // the new spote of the empty index in array
            newParcel.id = DataSource.Config.ParcelsId; // insert the Parcels new Id
            int tmp = DataSource.Config.ParcelsId;
            DataSource.Config.ParcelsId++; // new Id for the fautre parce Id
            return tmp; // return the new number created
        }

        /// <summary>
        /// add a customer to the fustomer list
        /// </summary>
        /// <param name="newCustomer"></the new customer the user whants to add to the customer's list>
        public static void AddCustomer(Customer newCustomer)
        {
            if (DataSource.Config.VacantIndexC == DataSource.customers.Length) // if there is no place in array
            {
                Customer[] newArrayCustomers = new Customer[DataSource.Config.VacantIndexC * 2]; // increse the place in array double 2
                for (int i = 0; i < DataSource.customers.Length; i++)
                    newArrayCustomers[i] = DataSource.customers[i]; // copy the old array to new array
                DataSource.customers = newArrayCustomers;
            }
            DataSource.customers[DataSource.Config.VacantIndexC] = newCustomer; // insert to the last place in array
            DataSource.Config.VacantIndexC++;  // the new spote of the empty index in array
        }

        /// <summary>
        /// return the spesifice station the user ask for
        /// </summary>
        /// <param name="stationId"></the id of the station the user ask for>
        /// <returns></returns>
        public static Station GetStation(int stationId)
        {
            Station newStation = new Station();
            for (int i = 0; i < DataSource.Config.VacantIndexS; i++)
            {
                if (DataSource.stations[i].id == stationId)
                {
                    newStation = DataSource.stations[i];
                }
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
            for (int i = 0; i < DataSource.Config.VacantIndexD; i++)
            {
                if (DataSource.drones[i].id == droneId)
                {
                    newDrone = DataSource.drones[i];
                }
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
            for (int i = 0; i < DataSource.Config.VacantIndexP; i++)
            {
                if (DataSource.parcels[i].id == parcelId)
                {
                    newParcel = DataSource.parcels[i];
                }
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
            for (int i = 0; i < DataSource.Config.VacantIndexC; i++)
            {
                if (DataSource.customers[i].id == customerId)
                {
                    newCustomer = DataSource.customers[i];
                }
            }
            return newCustomer;
        }

        /// <summary>
        /// return all the list of the station's
        /// </summary>
        /// <returns></returns>
        public static List<Station> GetStations()
        {
            List<Station> newStations = new List<Station>(DataSource.stations);
            return newStations;
        }

        /// <summary>
        /// return all the drone's list
        /// </summary>
        /// <returns></returns>
        public static List<Drone> GetDrones()
        {
            List<Drone> newDrones = new List<Drone>(DataSource.drones);
            return newDrones;
        }
        
        /// <summary>
        /// return all the list of the drone's that they are in charge sopt 
        /// </summary>
        /// <returns></returns>
        public static List<DroneCharge> GetDronesCharge()
        {
            List<DroneCharge> newDronesCharge = new List<DroneCharge>(DataSource.droneCharges);
            return newDronesCharge;
        }

        /// <summary>
        /// return all the parcel in the list
        /// </summary>
        /// <returns></returns>
        public static List<Parcel> GetParcels()
        {
            List<Parcel> newParcels = new List<Parcel>(DataSource.parcels);
            return newParcels;
        }
  
        /// <summary>
        /// return all the customer list
        /// </summary>
        /// <returns></returns>
        public static List<Customer> GetCustomers()
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
            int index = -1;
            for (int i = 0; i < DataSource.Config.VacantIndexP; i++)
            {
                if (DataSource.parcels[i].id == p.id)
                    index = i; // found the place in array
            }
            DataSource.parcels[index].requested = DateTime.Now;//The time to create a package for delivery
            DataSource.parcels[index].droneId = d.id;//conect
            DataSource.parcels[index].scheduled = DateTime.Now;
            for (int i = 0; i < DataSource.Config.VacantIndexD; i++)
            {
                if (DataSource.drones[i].id == d.id)
                    index = i; // found the place in array
            }
            DataSource.drones[index].status = DroneStatuses.Delivery;
        }

        /// <summary>
        /// chenge the pick up statuse of the parcel the user ask for by searching the parcel in the list and update the pick up time statuse of the parcel
        /// </summary>
        /// <param name="p"></the spesific parcel the user ask to update as pick'd up>
        public static void CollectionParcelByDrone(Parcel p)
        {
            int index = -1;
            for (int i = 0; i < DataSource.Config.VacantIndexP; i++)
            {
                if (DataSource.parcels[i].id == p.id)
                    index = i; // found the place in array
            }
            DataSource.parcels[index].pickedUp = DateTime.Now;
        }

        /// <summary>
        /// chenge the deliversd statuse of the parcel the user ask for by searching the parcel in the list and update the deliverd time statuse of the parcel
        /// </summary>
        /// <param name="p"></the specific parcel the user ask to pdate as deliver'd >
        public static void SupplyParcelToCustomer(Parcel p)
        {
            int index = -1;
            for (int i = 0; i < DataSource.Config.VacantIndexP; i++)
            {
                if (DataSource.parcels[i].id == p.id)
                    index = i; // found the place in array
            }
            DataSource.parcels[index].delivered = DateTime.Now;
        }

        /// <summary>
        /// send a spesifice drone to a spesifice station that have a availble charge spots, and change the status of the drome to maintenance
        /// </summary>
        /// <param name="d"></the spesifice drone the user ask for to charge>
        /// <param name="s"></the spasifice station the user ask to charge the drone in>
        public static void SendDroneToDroneCharge(Drone d,Station s)
        {
            int index = -1;
            for (int i = 0; i < DataSource.Config.VacantIndexD; i++)
            {
                if (DataSource.drones[i].id == d.id)
                    index = i; // found the place in array
            }
            DataSource.drones[index].status = DroneStatuses.Maintenance;
            for (int i = 0; i < DataSource.Config.VacantIndexS; i++)
            {
                if (DataSource.stations[i].id == s.id)
                    index = i; // found the place in array
            }
            DataSource.stations[index].chargeSlots--;
            DataSource.droneCharges[DataSource.Config.VacantIndexDC].droneId = d.id;
            DataSource.droneCharges[DataSource.Config.VacantIndexDC].stationld = s.id;
            DataSource.Config.VacantIndexDC++;
        }

        /// <summary>
        /// release the spesifice drone frome the station he located and update the battry and status to 100 and available
        /// </summary>
        /// <param name="s"></the spesifice station the user ask to release the drone frome>
        /// <param name="d"></the spesifice drone the user ask to release frome the station>
        public static void ReleaseDroneFromDroneCharge(Station s,Drone d)
        {
            int index = -1;
            for (int i = 0; i < DataSource.Config.VacantIndexS; i++)
            {
                if (DataSource.stations[i].id == s.id)
                    index = i; // found the place in array
            }
            DataSource.stations[index].chargeSlots++;

            for (int i = 0; i < DataSource.Config.VacantIndexD; i++)
            {
                if (DataSource.drones[i].id == d.id)
                    index = i; // found the place in array
            }
            DataSource.drones[index].status = DroneStatuses.Available;
            DataSource.drones[index].battry = 100;

            DroneCharge[] newDroneCharges = new DroneCharge[100];
            for (int i = 0; i < DataSource.droneCharges.Length; i++)
            {
                if (DataSource.droneCharges[i].droneId == d.id && DataSource.droneCharges[i].stationld == s.id)
                {
                    for (int j = 0, k = 0; j < DataSource.droneCharges.Length; j++, k++)//to remove the elemnt
                    {
                        if (j != i)//copy all the array befor I
                            newDroneCharges[k] = DataSource.droneCharges[j];
                        else//keep the index of I to remove the elemnt
                            k--;
                    }
                }
            }
            DataSource.droneCharges = newDroneCharges;
            DataSource.Config.VacantIndexDC--;
        }
    }
}




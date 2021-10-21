using System;
using IDAL.DO;

namespace DalObject
{
    public class DalObject
    {
        public DalObject()
        {
            DataSource.Initialize();
        }

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

        public static Station[] GetStations()
        {
            Station[] newStations = new Station[DataSource.Config.VacantIndexS];
            for (int i = 0; i < DataSource.Config.VacantIndexS; i++)
                newStations[i] = DataSource.stations[i];
            return newStations;
        }

        public static Drone[] GetDrones()
        {
            Drone[] newDrones = new Drone[DataSource.Config.VacantIndexD];
            for (int i = 0; i < DataSource.Config.VacantIndexD; i++)
                newDrones[i] = DataSource.drones[i];
            return newDrones;
        }

        public static DroneCharge[] GetDronesCharge()
        {
            DroneCharge[] newDronesCharge = new DroneCharge[DataSource.Config.VacantIndexDC];
            for (int i = 0; i < DataSource.Config.VacantIndexDC; i++)
                newDronesCharge[i] = DataSource.droneCharges[i];
            return newDronesCharge;
        }

        public static Parcel[] GetParcels()
        {
            Parcel[] newParcels = new Parcel[DataSource.Config.VacantIndexP];
            for (int i = 0; i < DataSource.Config.VacantIndexP; i++)
                newParcels[i] = DataSource.parcels[i];
            return newParcels;
        }
  
        public static Customer[] GetCustomers()
        {
            Customer[] newCustomers = new Customer[DataSource.Config.VacantIndexC];
            for (int i = 0; i < DataSource.Config.VacantIndexC; i++)
                newCustomers[i] = DataSource.customers[i];
            return newCustomers;
        }
        public static void ConnectParcelToDrone(Parcel p, Drone d)//Assign a package to a skimmer
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

        public static void CollectionParcelByDrone(Parcel p)//Assign a package to a drone
        {
            p.pickedUp = DateTime.Now;
        }

        public static void SupplyParcelToCustomer(Parcel p)
        {
            p.delivered = DateTime.Now;
        }

        public static void SendDroneToDroneCharge(Drone d,Station s)
        {
            d.status = DroneStatuses.Maintenance;
            s.chargeSlots--;
            DataSource.droneCharges[DataSource.Config.VacantIndexDC].droneId = d.id;
            DataSource.droneCharges[DataSource.Config.VacantIndexDC].stationld = s.id;
            DataSource.Config.VacantIndexDC++;
            d.battry = 100;
        }

        public static void ReleaseDroneFromDroneCharge(Station s,Drone d)
        {
            s.chargeSlots++;
            d.status = DroneStatuses.Available;
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
            s.chargeSlots++;
        }
    }
}




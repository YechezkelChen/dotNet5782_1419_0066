using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL;
using IDAL.DO;

namespace IDAL
{
    interface IDal
    { 
        void AddStation(Station newStation);
        int AddParcel(Parcel newParcel);
        void AddCustomer(Customer newCustomer);
        void ConnectParcelToDrone(Parcel p, Drone d);
        void CollectionParcelByDrone(Parcel p);
        void SupplyParcelToCustomer(Parcel p);
        void SendDroneToDroneCharge(Station s, Drone d);
        void ReleaseDroneFromDroneCharge(Station s, Drone d);
        Station GetStation(int stationId);
        Drone GetDrone(int droneId);
        Parcel GetParcel(int parcelId);
        Customer GetCustomer(int customerId);
        IEnumerable<Station> GetStations();
        IEnumerable<Drone> GetDrones();
        IEnumerable<DroneCharge> GetDronesCharge();
        IEnumerable<Parcel> GetParcels();
        IEnumerable<Customer> GetCustomers();
        double[] GetRequestPowerConsumption();
    }
}
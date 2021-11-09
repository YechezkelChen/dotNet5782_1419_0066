using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL;
using IDAL.DO;

namespace IDAL
{
    public interface IDal
    {
        void AddStation(Station newStation);
        void AddDrone(Drone newDrone);
        int AddParcel(Parcel newParcel);
        void AddCustomer(Customer newCustomer);
        Station GetStation(int stationId);
        Drone GetDrone(int droneId);
        Parcel GetParcel(int parcelId);
        Customer GetCustomer(int customerId);
        IEnumerable<Station> GetStations();
        IEnumerable<Drone> GetDrones();
        IEnumerable<DroneCharge> GetDronesCharge();
        IEnumerable<Parcel> GetParcels();
        IEnumerable<Customer> GetCustomers();
        void ConnectParcelToDrone(Parcel p, Drone d);
        void CollectionParcelByDrone(Parcel p);
        void SupplyParcelToCustomer(Parcel p);
        void SendDroneToDroneCharge(Station s, Drone d);
        void ReleaseDroneFromDroneCharge(Station s, Drone d);
        double[] GetRequestPowerConsumption();
        bool CheckNotExistStation(Station s, List<Station> stations);
        bool CheckNotExistDrone(Drone d, List<Drone> drones);
        bool CheckNotExistParcel(Parcel p, List<Parcel> parcels);
        bool CheckNotExistCustomer(Customer c, List<Customer> customers);
        void UpdateDroneName(int dronId, string newModel);
    }
}
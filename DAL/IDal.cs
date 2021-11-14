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
        // General
        double[] GetRequestPowerConsumption();


        // Station
        void AddStation(Station newStation);
        Station GetStation(int stationId);
        IEnumerable<Station> GetStations();
        void UpdateStation(Station station);
        bool IsExistStation(Station s, IEnumerable<Station> stations);



        // Drone
        void AddDrone(Drone newDrone);
        Drone GetDrone(int droneId);
        IEnumerable<Drone> GetDrones();
        void UpdateDrone(Drone drone);
        bool IsExistDrone(Drone d, IEnumerable<Drone> drones);


        // Customer
        void AddCustomer(Customer newCustomer);
        Customer GetCustomer(int customerId);
        IEnumerable<Customer> GetCustomers();
        void UpdateCustomer(Customer customer);
        bool IsExistCustomer(Customer c, IEnumerable<Customer> customers);


        // Parcel
        int AddParcel(Parcel newParcel);
        Parcel GetParcel(int parcelId);
        IEnumerable<Parcel> GetParcels();
        void UpdateParcel(Parcel parcel);
        bool IsExistParcel(Parcel p, IEnumerable<Parcel> parcels);



        // DroneCharge
        void AddDroneCharge(DroneCharge newDroneCharge);
        void RemoveDroneCharge(DroneCharge DroneCharge);
        IEnumerable<DroneCharge> GetDronesCharge();
        bool IsExistDroneCharge(DroneCharge droneCharge, IEnumerable<DroneCharge> droneCharges);





        void ConnectParcelToDrone(Parcel p, Drone d);
        void CollectionParcelByDrone(Parcel p);
        void SupplyParcelToCustomer(Parcel p);
        void SendDroneToDroneCharge(Station s, Drone d);
        void ReleaseDroneFromDroneCharge(Station s, Drone d);






 
    }
}
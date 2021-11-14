using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;

namespace IBL
{
    public interface IBL
    {
        //constructor functions
        double Distance(Location from, Location to);
        IEnumerable<IDAL.DO.Customer> ListCustomersWithDelivery(IEnumerable<IDAL.DO.Customer> customers,
            IEnumerable<IDAL.DO.Parcel> Parcels);
        
        // Station
        void AddStation(Station newStation);
        Station GetStation(int id);
        IEnumerable<StationToList> GetStations();
        Station NearStationToDrone(IDAL.DO.Drone drone);
        Station NearStationToCustomer(IDAL.DO.Customer customer);
        IEnumerable<StationToList> GetStationsCharge();
        void UpdateDataStation(int id, int name, int chargeSlots);
        void CheckStation(Station station);

        // Drone
        void AddDrone(Drone newDrone, int idStation);
        Drone GetDrone(int id);
        IEnumerable<DroneToList> GetDrones();
        void UpdateDroneModel(int droneId, string newModel);
        void CheckDrone(Drone drone);
        int CheckDroneAndParcel(int droneId, IEnumerable<IDAL.DO.Parcel> parcels);
        void SendDroneToDroneCharge(int id);
        void ReleaseDroneFromDroneCharge(int id, int chargeTime);

        // Customer
        void AddCustomer(Customer newCustomer);
        Customer GetCustomer(int id);
        IEnumerable<CustomerToList> GetCustomers();
        void UpdateDataCustomer(int id, string name, string phone);
        void CheckCustomer(Customer customer);

        // Parcel
        void AddParcel(Parcel newParcel);
        Parcel GetParcel(int id);
        IEnumerable<ParcelToList> GetParcels();
        IEnumerable<ParcelToList> GetParcelsNoDrones();
        void CheckParcel(Parcel parcel);
        void ConnectParcelToDrone(int idDrone);
        void CollectionParcelByDrone(int idDrone);
        void SupplyParcelByDrone(int idDrone);
        Parcel NearParcelToDrone(IDAL.DO.Drone drone)
    }
}

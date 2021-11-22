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
        // Station
        void AddStation(Station newStation);
        Station GetStation(int id);
        IEnumerable<StationToList> GetStations(Predicate<IDAL.DO.Station> stationPredicate);
        void UpdateDataStation(int id, int name, int chargeSlots);


        // Drone
        void AddDrone(Drone newDrone, int idStation);
        Drone GetDrone(int id);
        IEnumerable<DroneToList> GetDrones();
        void UpdateDroneModel(int droneId, string newModel);
        void SendDroneToDroneCharge(int id);
        void ReleaseDroneFromDroneCharge(int id, int chargeTime);


        // Customer
        void AddCustomer(Customer newCustomer);
        Customer GetCustomer(int id);
        IEnumerable<CustomerToList> GetCustomers();
        void UpdateDataCustomer(int id, string name, string phone);


        // Parcel
        void AddParcel(Parcel newParcel);
        Parcel GetParcel(int id);
        IEnumerable<ParcelToList> GetParcels(Predicate<IDAL.DO.Parcel> parcelPredicate);
        void ConnectParcelToDrone(int idDrone);
        void CollectionParcelByDrone(int idDrone);
        void SupplyParcelByDrone(int idDrone);
    }
}

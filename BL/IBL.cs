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
        Location NearStationToCustomer(IDAL.DO.Customer customer, IEnumerable<IDAL.DO.Station> stations);
        double Distance(Location from, Location to);
        IEnumerable<IDAL.DO.Customer> ListCustomersWithDelivery(IEnumerable<IDAL.DO.Customer> customers,
            IEnumerable<IDAL.DO.Parcel> Parcels);
        Location NearStationToDrone(Location droneLocation, IEnumerable<IDAL.DO.Station> stations);
        
        // Station
        void AddStation(Station newStation);
        Station GetStation(int id);
        IEnumerable<StationToList> GetStations();
        IEnumerable<StationToList> GetStationsCharge();
        void UpdateStation(int id, int name, int chargeSlots);
        void CheckStation(Station station);

        // Drone
        void AddDrone(Drone newDrone, int idStation);
        Drone GetDrone(int id);
        IEnumerable<DroneToList> GetDrones();
        void UpdateDrone(int droneId, string newModel);
        void CheckDrone(Drone drone);
        int CheckDroneAndParcel(int droneId, IEnumerable<IDAL.DO.Parcel> parcels);

        // Customer
        void AddCustomer(Customer newCustomer);
        Customer GetCustomer(int id);
        IEnumerable<CustomerToList> GetCustomers();
        void UpdateCustomer(int id, string name, string phone);
        void CheckCustomer(Customer customer);

        // Parcel
        void AddParcel(Parcel newParcel);
        Parcel GetParcel(int id);
        IEnumerable<ParcelToList> GetParcels();
        IEnumerable<ParcelToList> GetParcelsNoDrones();
        void CheckParcel(Parcel parcel);
    }
}

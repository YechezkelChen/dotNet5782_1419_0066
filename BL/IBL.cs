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
        Location NearStationToCustomer(IDAL.DO.Customer customer, IEnumerable<IDAL.DO.Station> stations);
        double Distance(Location from, Location to);
        IEnumerable<IDAL.DO.Customer> ListCustomersWithDelivery(IEnumerable<IDAL.DO.Customer> customers,
            IEnumerable<IDAL.DO.Parcel> Parcels);
        Location NearStationToDrone(Location droneLocation, IEnumerable<IDAL.DO.Station> stations);
        void AddStation(Station newStation);
        void AddDrone(Drone newDrone, int idStation);
        void AddCustomer(Customer newCustomer);
        void AddParcel(Parcel newParcel);
        public int CheckDroneAndParcel(int droneId, IEnumerable<IDAL.DO.Parcel> parcels);
        Station GetStation(int Id);
        Drone GetDrone(int Id);
        Customer GetCustomer(int id);
        Parcel GetParcel(int id);
        void PrintStations();
        void PrintDrones();
        void PrintCustomers();
        void PrintParcels();
        void PrintParcelsNoDrones();
        void PrintStationsCharge();
        void UpdateDrone(int droneId, string newModel);
    }
}

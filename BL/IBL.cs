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
        void AddStation(Station newStation); // Adds a station to the list of stations.
        Station GetStation(int id); // Returning a station according to the id of the station.
        IEnumerable<StationToList> GetStations(); // Returning the list of stations in a special entity "Station to list".
        IEnumerable<StationToList> GetStationsCharge(); // Returning the list of stations with available charging position in a special entity "Station to list".
        void UpdateDataStation(int id, string name, int chargeSlots); // Update station data according to user request.


        // Drone
        void AddDrone(Drone newDrone, int idStation); // Adds a drone to the list of drones.
        Drone GetDrone(int id); // Returning a drone according to the id of the drone.
        IEnumerable<DroneToList> GetDrones(); // Returning the list of drones in a special entity "Drone to list".
        IEnumerable<DroneToList> GetDronesByStatus(DroneStatuses status); // Returning the list of drones with filtering of status
        IEnumerable<DroneToList> GetDronesByMaxWeight(WeightCategories weight); // Returning the list of drones with filtering of max weight
        void UpdateDroneModel(int droneId, string newModel); // Update drone data according to user request.
        void SendDroneToDroneCharge(int id);// Sending a drone for charging at a vacant charging station.
        void ReleaseDroneFromDroneCharge(int id); // Release drone from charging station.


        // Customer
        void AddCustomer(Customer newCustomer); // Adds a customer to the list of customers.
        Customer GetCustomer(int id); // Returning a customer according to the id of the customer.
        IEnumerable<CustomerToList> GetCustomers(); // Returning the list of customers in a special entity "Customer to list".
        void UpdateDataCustomer(int id, string name, string phone); // Update customer data according to user request.


        // Parcel 
        void AddParcel(Parcel newParcel); // Adds a parcel to the list of parcels.
        Parcel GetParcel(int id); // Returning a parcel according to the id of the parcel.
        IEnumerable<ParcelToList> GetParcels(); // Returning the list of parcels in a special entity "Parcel to list".
        IEnumerable<ParcelToList> GetParcelsNoDrones(); // Returning the list of parcels with no drones in a special entity "Parcel to list".
        void ConnectParcelToDrone(int idDrone); // Selecting a parcel according to criteria and sending a drone according to the id that will be associated with the parcel.
        void CollectionParcelByDrone(int idDrone); // Sending the desired drone (according to id) to collect the parcel from its place.
        void SupplyParcelByDrone(int idDrone); // Sending the drone (according to id) deliver the parcel to the destination of the parcel.
    }
}

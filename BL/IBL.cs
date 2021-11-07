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
        public void AddStation(Station newStation);
        public void AddDrone(Drone newDrone, int idStation);
    }
}

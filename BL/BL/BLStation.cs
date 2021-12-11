using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
using IDAL;
using IDAL.DO;
using Parcel = IDAL.DO.Parcel;
using Station = IBL.BO.Station;
using Drone = IBL.BO.Drone;
using Customer = IBL.BO.Customer;

namespace IBL
{
    public partial class BL : IBL
    {
        /// <summary>
        ///  add station with all fields to data source with checking 
        /// </summary>
        /// <param name="newStation"></param>
        public void AddStation(Station newStation)
        {
            try
            {
                CheckStation(newStation);
            }
            catch (StationException e)
            {
                throw new StationException(e.Message, e);
            }
            IDAL.DO.Station station = new IDAL.DO.Station();
            station.Id = newStation.Id;
            station.Name = newStation.Name;
            station.Longitude = newStation.Location.Longitude;
            station.Latitude = newStation.Location.Latitude;
            station.ChargeSlots = newStation.ChargeSlots;
            try
            {
                dal.AddStation(station);
            }
            catch (DalObject.stationException e)
            {
                throw new StationException(e.Message, e);
            }
        }

        /// <summary>
        /// send id of station and checking that it exist.
        /// make special entity and return it
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Station GetStation(int id)
        {
            IDAL.DO.Station idalStation = new IDAL.DO.Station();
            try
            {
                idalStation = dal.GetStation(id);
            }
            catch (DalObject.stationException e)
            {
                throw new StationException(e.Message, e);
            }

            Station station = new Station();
            station.Id = idalStation.Id;
            station.Name = idalStation.Name;
            station.Location = new Location() {Longitude = idalStation.Longitude, Latitude = idalStation.Latitude};
            station.ChargeSlots = idalStation.ChargeSlots;
            station.InCharges = new List<DroneCharge>();
            foreach (var elementDroneCharge in dal.GetDronesCharge(droneCharge => true))
                if(elementDroneCharge.StationId == station.Id)
                    station.InCharges.Add(elementDroneCharge);

            return station;
        }

        /// <summary>
        ///  return the list of stations in special entity for show
        /// </summary>
        /// <returns></returns>
        public IEnumerable<StationToList> GetStations()
        {
            List<StationToList> stationsToList = new List<StationToList>();

            foreach (var idalStation in dal.GetStations(station => true))
            {
                Station station = new Station();
                station = GetStation(idalStation.Id);
                StationToList newStationToList = new StationToList();
                newStationToList.Id = idalStation.Id;
                newStationToList.Name = idalStation.Name;
                newStationToList.ChargeSlotsAvailable = idalStation.ChargeSlots;
                newStationToList.ChargeSlotsNotAvailable = station.InCharges.Count();

                stationsToList.Add(newStationToList);
            }

            return stationsToList;
        }

        /// <summary>
        /// Returning the list of stations with available charging position in a special entity "Station to list".
        /// </summary>
        /// <returns></returns>
        public IEnumerable<StationToList> GetStationsCharge()
        {
            List<StationToList> stationsToList = new List<StationToList>();

            foreach (var idalStation in dal.GetStations(station => station.ChargeSlots > 0))
            {
                Station station = new Station();
                station = GetStation(idalStation.Id);
                StationToList newStationToList = new StationToList();
                newStationToList.Id = idalStation.Id;
                newStationToList.Name = idalStation.Name;
                newStationToList.ChargeSlotsAvailable = idalStation.ChargeSlots;
                newStationToList.ChargeSlotsNotAvailable = station.InCharges.Count();

                stationsToList.Add(newStationToList);
            }

            return stationsToList;
        }

        /// <summary>
        /// update the parameters that user want to update(name, chargeSlots)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="chargeSlots"></param>
        public void UpdateDataStation(int id, string name, int chargeSlots)
        {
            IDAL.DO.Station station = new IDAL.DO.Station();
            try
            {
                station = dal.GetStation(id);
            }
            catch (DalObject.stationException e)
            {
                throw new StationException(e.Message, e);
            }

            if (name == "" && chargeSlots == -1) // if he don't want to update nothing
                throw new StationException("ERROR: you must Enter at least one of the following data!\n");

            if (name != "") // if he want to update the name
                station.Name = name;
            if (chargeSlots != -1) // if he want to update the number of charge slots
                station.ChargeSlots = chargeSlots;

            dal.UpdateStation(station);
        }

        /// <summary>
        ///  find the near station from all stations to drone
        /// </summary>
        /// <param name="drone"></param>
        /// <returns></returns>
        private Station NearStationToDrone(Drone drone)
        {
            IEnumerable<StationToList> nearStation = new List<StationToList>();
            nearStation = GetStations().OrderByDescending(station => Distance(GetStation(station.Id).Location, drone.Location));
            return GetStation(nearStation.First().Id);

            //List<double> distancesList = new List<double>();
            //Location stationLocation = new Location();
            //Location droneLocation = new Location();
            //try
            //{
            //    droneLocation = GetDrone(drone.Id).Location;
            //}
            //catch (DroneException e)
            //{
            //    throw new DroneException(e.Message, e);
            //}

            //foreach (var stationCharge in GetStationsCharge())
            //{
            //    foreach (var station in dal.GetStations(s => true)) // for the location
            //    {
            //        if (stationCharge.Id == station.Id)
            //        {
            //            stationLocation = new Location() { Longitude = station.Longitude, Latitude = station.Latitude };
            //            distancesList.Add(Distance(stationLocation, droneLocation));
            //        }
            //    }
            //}

            //if (distancesList.Count() == 0)
            //    throw new DroneException("ERROR: There is no station to charge your drone! :(");

            //double minDistance = distancesList.Min();
            //Station nearStation = new Station();
            //foreach (var station in dal.GetStations(s => true))
            //{
            //    stationLocation.Longitude = station.Longitude;
            //    stationLocation.Latitude = station.Latitude;

            //    if (minDistance == Distance(stationLocation, droneLocation))
            //        nearStation = GetStation(station.Id);
            //}

            //return nearStation;
        }

        /// <summary>
        ///   find the near station from all stations to customer
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        private Station NearStationToCustomer(Customer customer)
        {
            IEnumerable<StationToList> nearStation = new List<StationToList>();
            nearStation = GetStations().OrderByDescending(station => Distance(GetStation(station.Id).Location, customer.Location));
            return GetStation(nearStation.First().Id);

            //List<double> distancesList = new List<double>();
            //Location stationLocation = new Location();
            //Location customerLocation = new Location() { Longitude = customer.Longitude, Latitude = customer.Latitude };

            //foreach (var stationCharge in GetStationsCharge())
            //{
            //    foreach (var station in dal.GetStations(s => true))
            //    {
            //        if (stationCharge.Id == station.Id)
            //        {
            //            stationLocation = new Location() { Longitude = station.Longitude, Latitude = station.Latitude };
            //            distancesList.Add(Distance(stationLocation, customerLocation));
            //        }
            //    }
            //}

            //double minDistance = distancesList.Min();
            //Station nearStation = new Station();
            //foreach (var station in dal.GetStations(s => true))
            //{
            //    stationLocation.Longitude = station.Longitude;
            //    stationLocation.Latitude = station.Latitude;

            //    if (minDistance == Distance(stationLocation, customerLocation))
            //        nearStation = GetStation(station.Id);
            //}

            //return nearStation;
        }

        /// <summary>
        ///  check the input in add station to list
        /// </summary>
        /// <param name="station"></param>
        private void CheckStation(Station station)
        {
            if (station.Id < 100000 || station.Id > 999999)//Check that it's 6 digits.
                throw new CustomerException("ERROR: the ID is illegal! ");

            if (station.ChargeSlots < 0)
                throw new StationException("ERROR: the charge slots must have positive or 0 value! ");

            if (station.Location.Longitude < -1 || station.Location.Longitude > 1)
                throw new StationException("ERROR: Longitude must to be between -1 to 1");

            if (station.Location.Latitude < -1 || station.Location.Latitude > 1)
                throw new StationException("ERROR: Latitude must to be between -1 to 1");

            foreach (var elementStation in dal.GetStations(s => true))
                if (elementStation.Latitude == station.Location.Latitude &&
                    elementStation.Longitude == station.Location.Longitude)
                    throw new StationException("ERROR: the location is catch! ");
        }
    }
}

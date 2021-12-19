using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using BO;


namespace BL
{
    partial class BL : BlApi.IBL
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
            catch (IdException e)
            {
                throw new IdException(e.Message, e);
            }
            catch (ChargeSlotsException e)
            {
                throw new ChargeSlotsException(e.Message, e);
            }
            catch (LocationException e)
            {
                throw new LocationException(e.Message, e);
            }

            DO.Station station = new DO.Station();
            station.Id = newStation.Id;
            station.Name = newStation.Name;
            station.Longitude = newStation.Location.Longitude;
            station.Latitude = newStation.Location.Latitude;
            station.ChargeSlots = newStation.ChargeSlots;
            station.deleted = false;
            try
            {
                dal.AddStation(station);
            }
            catch (DO.IdExistException e)
            {
                throw new IdException(e.Message, e);
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
            DO.Station idalStation = new DO.Station();
            try
            {
                idalStation = dal.GetStation(id);
            }
            catch (DO.IdNotFoundException e)
            {
                throw new IdException(e.Message, e);
            }

            Station station = new Station();
            station.Id = idalStation.Id;
            station.Name = idalStation.Name;
            station.Location = new Location() {Longitude = idalStation.Longitude, Latitude = idalStation.Latitude};
            station.ChargeSlots = idalStation.ChargeSlots;
            station.InCharges = new List<DroneInCharge>();
            foreach (var elementDroneCharge in dal.GetDronesCharge(droneCharge => true))
                if(elementDroneCharge.StationId == station.Id)
                {
                    DroneInCharge droneInCharge = new DroneInCharge();
                    droneInCharge.Id = elementDroneCharge.DroneId;
                    droneInCharge.Battery = GetDrone(elementDroneCharge.DroneId).Battery;
                    station.InCharges.ToList().Add(droneInCharge);
                }
                    
            return station;
        }

        /// <summary>
        ///  return the list of stations in special entity for show
        /// </summary>
        /// <returns></returns>
        public IEnumerable<StationToList> GetStations()
        {
            List<StationToList> stationsToList = new List<StationToList>();

            foreach (var idalStation in dal.GetStations(station => true && station.deleted == false))
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

            foreach (var idalStation in dal.GetStations(station => station.ChargeSlots > 0 && station.deleted == false ))
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
            DO.Station station = new DO.Station();
            try
            {
                station = dal.GetStation(id);
            }
            catch (DO.IdNotFoundException e)
            {
                throw new IdException(e.Message, e);
            }

            if (name == "" && chargeSlots == -1) // if he don't want to update nothing
                throw new NameException("ERROR: you must Enter at least one of the following data!\n");

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
        }

        /// <summary>
        ///  check the input in add station to list
        /// </summary>
        /// <param name="station"></param>
        private void CheckStation(Station station)
        {
            if (station.Id < 100000 || station.Id > 999999)//Check that it's 6 digits.
                throw new IdException("ERROR: the ID is illegal! ");

            if (station.ChargeSlots < 0)
                throw new ChargeSlotsException("ERROR: the charge slots must have positive or 0 value! ");

            if (station.Location.Longitude < -1 || station.Location.Longitude > 1)
                throw new LocationException("ERROR: longitude must to be between -1 to 1");

            if (station.Location.Latitude < -1 || station.Location.Latitude > 1)
                throw new LocationException("ERROR: latitude must to be between -1 to 1");

            foreach (var elementStation in dal.GetStations(s => true))
                if (elementStation.Latitude == station.Location.Latitude &&
                    elementStation.Longitude == station.Location.Longitude)
                    throw new LocationException("ERROR: the location is catch! ");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
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
            catch (NameException e)
            {
                throw new NameException(e.Message, e);
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
            station.AvailableChargeSlots = newStation.AvalibleChargeSlots;
            station.Deleted = false;
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
        /// <param name="stationId"></param>
        /// <returns></returns>
        public Station GetStation(int stationId)
        {
            DO.Station dalStation = new DO.Station();
            try
            {
                dalStation = dal.GetStation(stationId);
            }
            catch (DO.IdNotFoundException e)
            {
                throw new IdException(e.Message, e);
            }

            Station station = new Station();
            station.Id = dalStation.Id;
            station.Name = dalStation.Name;
            station.Location = new Location() {Longitude = dalStation.Longitude, Latitude = dalStation.Latitude};
            station.AvalibleChargeSlots = dalStation.AvailableChargeSlots;

            station.DronesInCharges = from droneCharge in dal.GetDronesCharge(droneCharge => droneCharge.Deleted == false)
                                      where droneCharge.StationId == station.Id
                                      let drone = GetDrone(droneCharge.DroneId)
                                      select new DroneInCharge
                                      {
                                          Id = droneCharge.DroneId,
                                          Battery = drone.Battery
                                      };                   
            return station;
        }

        /// <summary>
        ///  return the list of stations in special entity for show
        /// </summary>
        /// <returns></returns>
        public IEnumerable<StationToList> GetStations()
        {
            return from dalStation in dal.GetStations(station => station.Deleted == false)
                   let station = GetStation(dalStation.Id)
                   select new StationToList
                   {
                       Id = dalStation.Id,
                       Name = dalStation.Name,
                       AvailableChargeSlots = dalStation.AvailableChargeSlots,
                       NotAvailableChargeSlots = station.DronesInCharges.Count()
                   };
        }

        /// <summary>
        /// Returning the list of stations with available charging position in a special entity "Station to list".
        /// </summary>
        /// <returns></returns>
        public IEnumerable<StationToList> GetStationsWithAvailableCharge()
        {
            return from station in GetStations()
                   where station.AvailableChargeSlots > 0
                   select station;
        }

        public IEnumerable<IGrouping<int, StationToList>> GetStationsByGroupAvailableStations()
        {
            return GetStations().GroupBy(station => station.AvailableChargeSlots);
        }

        /// <summary>
            /// update the parameters that user want to update(name, chargeSlots)
            /// </summary>
            /// <param name="id"></param>
            /// <param name="name"></param>
            /// <param name="chargeSlots"></param>
        public void UpdateDataStation(int id, string name, int chargeSlots)
        {
            DO.Station updateStation = new DO.Station();
            try
            {
                updateStation = dal.GetStation(id);
            }
            catch (DO.IdNotFoundException e)
            {
                throw new IdException(e.Message, e);
            }

            if (name == "" && chargeSlots == -1) // if he don't want to update nothing
                throw new NameException("ERROR: you must Enter at least one of the following data!\n");

            if (name != "") // if he want to update the name
                updateStation.Name = name;
            if (chargeSlots != -1) // if he want to update the number of charge slots
                updateStation.AvailableChargeSlots = chargeSlots;

            dal.UpdateStation(updateStation);
        }

        /// <summary>
        ///  check the input in add station to list
        /// </summary>
        /// <param name="station"></param>
        private void CheckStation(Station station)
        {
            if (station.Id < 100000 || station.Id > 999999) // Check that it's 6 digits.
                throw new IdException("ERROR: the ID is illegal! ");
            if (station.Name == "")
                throw new NameException("ERROR: the name is illegal!");
            if (station.AvalibleChargeSlots < 0)
                throw new ChargeSlotsException("ERROR: the charge slots must have positive or 0 value! ");
            if (station.Location.Longitude < -1 || station.Location.Longitude > 1)
                throw new LocationException("ERROR: longitude must to be between -1 to 1");
            if (station.Location.Latitude < -1 || station.Location.Latitude > 1)
                throw new LocationException("ERROR: latitude must to be between -1 to 1");
        }

        public void RemoveDroneInCharge(Station station , DroneInCharge droneInCharge)
        {
            DO.DroneCharge droneCharge = new DO.DroneCharge();
            droneCharge.DroneId = station.DronesInCharges.FirstOrDefault(droneCharge => droneCharge.Id == droneInCharge.Id).Id;
            droneCharge.StationId = station.Id;
            droneCharge.Deleted = true;
            try
            {
                dal.RemoveDroneCharge(droneCharge);
            }
            catch (DO.IdNotFoundException e)
            {
                throw new IdException(e.Message, e);
            }
        }
    }
}

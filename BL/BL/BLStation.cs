using System;
using System.Collections.Generic;
using System.Linq;
using BO;
using System.Runtime.CompilerServices;

namespace BL
{
    partial class BL : BlApi.IBL
    {
        /// <summary>
        ///  add station with all fields to data source with checking 
        /// </summary>
        /// <param name="newStation"></param>
      
        [MethodImpl(MethodImplOptions.Synchronized)]
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

            lock (dal)
            {
                DO.Station station = new DO.Station();

                station.Id = newStation.Id;
                station.Name = newStation.Name;
                station.Longitude = newStation.Location.Longitude;
                station.Latitude = newStation.Location.Latitude;
                station.AvailableChargeSlots = newStation.AvailableChargeSlots;
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
        }

        /// <summary>
        ///  Removes a parcel from the list of parcels.
        /// </summary>
        /// <param name="stationId"></param>
      
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void RemoveStation(int stationId)
        {
            lock (dal)
            {
                try
                {
                    dal.RemoveStation(stationId); // Remove the station
                }
                catch (DO.IdExistException e)
                {
                    throw new IdException(e.Message, e);
                }
                catch (DO.IdNotFoundException e)
                {
                    throw new IdException(e.Message, e);
                }
            }
        }

        /// <summary>
        /// send id of station and checking that it exist.
        /// make special entity and return it
        /// </summary>
        /// <param name="stationId"></param>
        /// <returns></returns>
     
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Station GetStation(int stationId)
        {
            Station station = new Station();

            lock (dal)
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

                station.Id = dalStation.Id;
                station.Name = dalStation.Name;
                station.Location = new Location() {Longitude = dalStation.Longitude, Latitude = dalStation.Latitude};
                station.AvailableChargeSlots = dalStation.AvailableChargeSlots;

                station.DronesInCharges =
                    from droneCharge in dal.GetDronesCharge(droneCharge => droneCharge.Deleted == false)
                    where droneCharge.StationId == station.Id
                    let drone = GetDrone(droneCharge.DroneId)
                    select new DroneInCharge
                    {
                        Id = droneCharge.DroneId,
                        Battery = drone.Battery
                    };
            }

            return station;
        }

        /// <summary>
        ///  return the list of stations in special entity for show
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<StationToList> GetStations()
        {
            lock (dal)
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
        }

        /// <summary>
        /// Returning the list of stations with available charging position in a special entity "Station to list".
        /// </summary>
        /// <returns></returns>
      
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<StationToList> GetStationsWithAvailableCharge()
        {
            return from station in GetStations()
                where station.AvailableChargeSlots > 0
                select station;
        }

        /// <summary>
        /// return stations bu groping
        /// </summary>
        /// <returns></returns>
       
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<IGrouping<int, StationToList>> GetStationsByGroupAvailableStations()
        {
            IEnumerable<IGrouping<int, StationToList>> stations = GetStations().GroupBy(station => station.AvailableChargeSlots);
            return stations;
        }

        /// <summary>
        /// update the parameters that user want to update(name, chargeSlots)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="chargeSlots"></param>
       
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateDataStation(int id, string name, int chargeSlots)
        {
            lock (dal)
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
            if (station.AvailableChargeSlots < 0)
                throw new ChargeSlotsException("ERROR: the charge slots must have positive or 0 value! ");
            if (station.Location.Longitude < 29 || station.Location.Longitude > 33)
                throw new LocationException("ERROR: longitude must to be between 29 to 33");
            if (station.Location.Latitude < 33 || station.Location.Latitude > 37)
                throw new LocationException("ERROR: latitude must to be between 33 to 37");
        }

        /// <summary>
        ///  remove the drone that is in charge
        /// </summary>
        /// <param name="station"></the station the contain the drone>
        /// <param name="droneInCharge"></the drone to remove>
     
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void RemoveDroneInCharge(Station station , DroneInCharge droneInCharge)
        {
            lock (dal)
            {
                DO.DroneCharge droneCharge = new DO.DroneCharge();

                droneCharge.DroneId = station.DronesInCharges
                    .FirstOrDefault(droneCharge => droneCharge.Id == droneInCharge.Id).Id;
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
}

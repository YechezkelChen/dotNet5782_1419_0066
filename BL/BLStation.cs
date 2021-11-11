using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
using IDAL;


namespace IBL
{
    public partial class BL : IBL
    {
        public void AddStation(Station newStation)
        {
            try
            {
                CheckStation(newStation);
            }
            catch (StationException e)
            {
                throw new StationException("" + e);
            }
            IDAL.DO.Station station = new IDAL.DO.Station();
            station.Id = newStation.Id;
            station.Name = newStation.Name;
            station.Longitude = newStation.Location.Longitude;
            station.Latitude = newStation.Location.Latitude;
            station.ChargeSlots = newStation.ChargeSlots;
            dal.AddStation(station);
        }

        public Station GetStation(int id)
        {
            IDAL.DO.Station idalStation = new IDAL.DO.Station();
            try
            {
                idalStation = dal.GetStation(id);
            }
            catch (DalObject.StationExeption e)
            {
                throw new StationException("" + e);
            }

            Station station = new Station();
            station.Id = idalStation.Id;
            station.Name = idalStation.Name;
            station.Location.Longitude = idalStation.Longitude;
            station.Location.Latitude = idalStation.Latitude;
            station.ChargeSlots = idalStation.ChargeSlots;
            foreach (var elementDroneCharge in dal.GetDronesCharge())
                if(elementDroneCharge.Stationld == station.Id)
                    station.InCharges.Add(elementDroneCharge);

            return station;
        }

        public IEnumerable<StationToList> GetStations()
        {
            IEnumerable<IDAL.DO.Station> idalStations = dal.GetStations();
            List<StationToList> stationsToList = new List<StationToList>();
            StationToList newStationToList = new StationToList();
            Station station = new Station();

            foreach (var idalStation in idalStations)
            {
                station = GetStation(idalStation.Id);

                newStationToList.Id = idalStation.Id;
                newStationToList.Name = idalStation.Name;
                newStationToList.ChargeSlotsAvailable = idalStation.ChargeSlots - station.InCharges.Count();
                newStationToList.ChargeSlotsNotAvailable = idalStation.ChargeSlots -
                                                           newStationToList.ChargeSlotsAvailable;

                stationsToList.Add(newStationToList);
            }

            return stationsToList;
        }

        public Station NearStationToDrone(IDAL.DO.Drone drone)
        {
            List<double> distancesList = new List<double>();
            Location stationLocation = new Location();
            Location droneLocation = GetDrone(drone.Id).Location;

            foreach (var stationCharge in GetStationsCharge())
            {
                foreach (var station in dal.GetStations())
                {
                    if (stationCharge.Id == station.Id)
                    {
                        stationLocation = new Location() { Longitude = station.Longitude, Latitude = station.Latitude };
                        distancesList.Add(Distance(stationLocation, droneLocation));
                    }
                }
            }

            double minDistance = distancesList.Min();
            Station nearStation = new Station();
            foreach (var station in dal.GetStations())
            {
                stationLocation.Longitude = station.Longitude;
                stationLocation.Latitude = station.Latitude;

                if (minDistance == Distance(stationLocation, droneLocation))
                    nearStation = GetStation(station.Id);
            }

            return nearStation;
        }

        public Station NearStationToCustomer(IDAL.DO.Customer customer)
        {
            List<double> distancesList = new List<double>();
            Location stationLocation = new Location();
            Location customerLocation = new Location() { Longitude = customer.Longitude, Latitude = customer.Latitude };

            foreach (var stationCharge in GetStationsCharge())
            {
                foreach (var station in dal.GetStations())
                {
                    if (stationCharge.Id == station.Id)
                    {
                        stationLocation = new Location() { Longitude = station.Longitude, Latitude = station.Latitude };
                        distancesList.Add(Distance(stationLocation, customerLocation));
                    }
                }
            }

            double minDistance = distancesList.Min();
            Station nearStation = new Station();
            foreach (var station in dal.GetStations())
            {
                stationLocation.Longitude = station.Longitude;
                stationLocation.Latitude = station.Latitude;

                if (minDistance == Distance(stationLocation, customerLocation))
                    nearStation = GetStation(station.Id);
            }

            return nearStation;
        }

        public IEnumerable<StationToList> GetStationsCharge()
        {
            IEnumerable<StationToList> stationsToList = GetStations();
            List<StationToList> stationChargeSlotsAvailable= new List<StationToList>();

            foreach (var elementStationToList in stationsToList)
            {
                if (elementStationToList.ChargeSlotsAvailable > 0)
                    stationChargeSlotsAvailable.Add(elementStationToList);
            }

            return stationChargeSlotsAvailable;
        }

        public void UpdateStation(int id, int name, int chargeSlots)
        {
            if (id < 0)
                throw new StationException("ERROR: the ID is illegal!");

            if (dal.CheckNotExistStation(dal.GetStation(id), dal.GetStations()))
                throw new StationException("ERROR: the drone not exist:");
            if (name == -1 && chargeSlots == -1)
                throw new StationException("ERROR: you must Enter at least one of the following data!\n");
            dal.UpdateDataStation(id, name, chargeSlots);
        }


        public void CheckStation(Station station)
        {
            if (station.Id < 0)
                throw new StationException("ERROR: the ID is illegal! ");

            if (station.ChargeSlots < 0)
                throw new StationException("ERROR: the charge slots must have positive or 0 value! ");

            if (station.Location.Latitude < 0 || station.Location.Longitude < 0)
                throw new StationException("ERROR: the location is not exist! ");

            foreach (var elementStation in dal.GetStations())
                if (elementStation.Latitude == station.Location.Latitude &&
                    elementStation.Longitude == station.Location.Longitude)
                    throw new StationException("ERROR: the location is catch! ");
        }
    }
}

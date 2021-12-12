using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using DO;

namespace DalObject
{
    public partial class DalObject : DalApi.IDal
    {
        /// <summary>
        /// add a staion to the list station
        /// </summary>
        /// <param Name="newStation"></the new station the user whants to add to the station's list>
        public void AddStation(Station newStation)
        {
            if(!IsExistStation(newStation, DataSource.Stations))
                DataSource.Stations.Add(newStation);
            else
                throw new stationException("ERROR: the station exist!\n");
        }

        /// <summary>
        /// return the spesifice station the user ask for
        /// </summary>
        /// <param name="stationId"></param>
        /// <param Name="stationId"></the Id of the station the user ask for>
        /// <returns></returns>
        public Station GetStation(int stationId)
        {
            Station? newStation = null;
            foreach (Station elementStation in DataSource.Stations)
            {
                if (elementStation.Id == stationId)
                    newStation = elementStation;
            }

            if (newStation == null)
                throw new stationException("ERROR: Id of station not found\n");
            return (Station)newStation;
        }

        /// <summary>
        /// return all the list of the station's
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Station> GetStations(Predicate<Station> stationPredicate)
        {
            IEnumerable<Station> stations = DataSource.Stations.FindAll(stationPredicate);
            return stations;
        }

        public void UpdateStation(Station station)
        {
            for (int i = 0; i < DataSource.Stations.Count(); i++)
                if (DataSource.Stations[i].Id == station.Id)
                    DataSource.Stations[i] = station;
        }

        /// <summary>
        /// the methode not need exeption becuse she use both sids(true and false)
        /// </summary>
        /// <param Name="s"></the station that we chek if she exist>
        /// <param Name="Stations"></the list of all Stations>
        /// <returns></returns>
        private bool IsExistStation(Station station, IEnumerable<Station> stations)
        {
            foreach (Station elementStation in stations)
                if (elementStation.Id == station.Id)
                    return true;
            return false; //the station not exist
        }
    }
}
using System;
using IDAL.DO;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using IDAL;

namespace DalObject
{
    public partial class DalObject : IDal
    {
        public DalObject()
        {
            DataSource.Initialize();
        }

        /// <summary>
        /// add a staion to the list station
        /// </summary>
        /// <param Name="newStation"></the new station the user whants to add to the station's list>
        public void AddStation(Station newStation)
        {
            if(CheckNotExistStation(newStation, DataSource.stations))
                DataSource.stations.Add(newStation);
            else
                throw new StationExeption("ERROR: the station exist!\n");
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
            foreach (Station elementStation in DataSource.stations)
            {
                if (elementStation.Id == stationId)
                    newStation = elementStation;
            }

            if (newStation == null)
                throw new StationExeption("ERROR: Id of station not found\n");
            return (Station)newStation;
        }

        /// <summary>
        /// return all the list of the station's
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Station> GetStations()
        {
            List<Station> newStations = new List<Station>(DataSource.stations);
            return newStations;
        }

        /// <summary>
        /// the methode not need exeption becuse she use both sids(true and false)
        /// </summary>
        /// <param Name="s"></the station that we chek if she exist>
        /// <param Name="stations"></the list of all stations>
        /// <returns></returns>
        public bool CheckNotExistStation(Station s, List<Station> stations)
        {
            foreach (Station elementStation in stations)
                if (elementStation.Id == s.Id)
                    return false;
            return true; //the station not exist
        }
    }
}
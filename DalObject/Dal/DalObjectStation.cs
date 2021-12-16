﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using DO;

namespace Dal
{
    partial class DalObject : DalApi.IDal
    {
        /// <summary>
        /// add a staion to the list station
        /// </summary>
        /// <param Name="newStation"></the new station the user whants to add to the station's list>
        public void AddStation(Station newStation)
        {
            string check = IsExistStation(newStation.Id);
            if (check == "not exists")
                DataSource.Stations.Add(newStation);
            if (check == "exists")
                throw new IdExistException("ERROR: the Station is exist");
            if (check == "was exists")
                throw new IdExistException("ERROR: the Station was exist");
        }


        /// <summary>
        /// return the spesifice station the user ask for
        /// </summary>
        /// <param name="stationId"></param>
        /// <param Name="stationId"></the Id of the station the user ask for>
        /// <returns></returns>
        public Station GetStation(int stationId)
        {
            string check = IsExistStation(stationId);
            Station station = new Station();
            if (check == "exists")
            {
                station = DataSource.Stations.Find(elementstation => elementstation.Id == stationId);
            }
            if (check == "not exists")
                throw new IdNotFoundException("ERROR: the station is not found.");
            if (check == "was exists")
                throw new IdExistException("ERROR: the station was exist");

            return station;
        }
        //public Station GetStation(int stationId)
        //{
        //    if (IsExistStation(stationId))
        //    {
        //        Station station = DataSource.Stations.Find(elementStation => elementStation.Id == stationId);
        //        return station;
        //    }
        //    else
        //        throw new IdNotFoundException("ERROR: the station is not found.");
        //}

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
        private string IsExistStation(int stationId)
        {
            foreach (Station elementStation in DataSource.Stations)
            {
                if (elementStation.Id == stationId && elementStation.deleted == false)
                    return "exists";
                if (elementStation.Id == stationId && elementStation.deleted == true)
                    return "was exists";
            }
            return "not exists"; // the customer not exist
        }
    }
}
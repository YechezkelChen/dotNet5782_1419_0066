using System;
using System.Collections.Generic;
using System.Linq;
using DO;
using System.Runtime.CompilerServices;

namespace Dal
{
    partial class DalObject : DalApi.IDal
    {
        /// <summary>
        /// add a staion to the list station
        /// </summary>
        /// <param Name="newStation"></the new station the user whants to add to the station's list>
        [MethodImpl(MethodImplOptions.Synchronized)]
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
        ///  Removes a parcel from the list of parcels.
        /// </summary>
        /// <param name="stationId"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void RemoveStation(int stationId)
        {
            string check = IsExistStation(stationId);

            if (check == "not exists")
                throw new IdNotFoundException("ERROR: the station is not found!\n");
            if (check == "was exists")
                throw new IdExistException("ERROR: the station was exist");

            if (check == "exists")
            {
                for (int i = 0; i < DataSource.Stations.Count(); i++)
                {
                    Station elementStation = DataSource.Stations[i];
                    if (elementStation.Id == stationId)
                    {
                        elementStation.Deleted = true;
                        DataSource.Stations[i] = elementStation;
                    }
                }
            }
        }

        /// <summary>
        /// return the spesifice station the user ask for
        /// </summary>
        /// <param name="stationId"></param>
        /// <param Name="stationId"></the Id of the station the user ask for>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
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

        /// <summary>
        /// return all the list of the station's
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Station> GetStations(Predicate<Station> stationPredicate)
        {
            IEnumerable<Station> stations = DataSource.Stations.Where(station => stationPredicate(station));
            return stations;
        }
        /// <summary>
        /// updata the station the user ask for
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
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
                if (elementStation.Id == stationId && elementStation.Deleted == false)
                    return "exists";
                if (elementStation.Id == stationId && elementStation.Deleted == true)
                    return "was exists";
            }
            return "not exists"; // the customer not exist
        }

    }
}
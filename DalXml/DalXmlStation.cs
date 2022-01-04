using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using DalXml;
using DO;

namespace Dal
{
    partial class DalXml : DalApi.IDal
    {
        /// <summary>
        /// add a station to the stations list
        /// </summary>
        /// <param Name="newStation"></the new station the user whants to add to the station's list>
        public void AddStation(Station newStation)
        {
            XElement stations = XMLTools.LoadListFromXmlElement(stationsPath);

            var addStation = (from s in stations.Elements()
                               where Convert.ToInt32(s.Element("Id").Value) == newStation.Id
                               select s).FirstOrDefault();

            if (!(addStation is null))
            {
                if (addStation.Element("Deleted").Value == "true")
                    throw new IdExistException("ERROR: the station is deleted!\n");

                throw new IdExistException("ERROR: the station is found!\n");
            }


            XElement id = new XElement("Id", newStation.Id);
            XElement name = new XElement("Name", newStation.Name);
            XElement longitude = new XElement("Longitude", newStation.Longitude);
            XElement latitude = new XElement("Latitude", newStation.Latitude);
            XElement availableChargeSlots = new XElement("AvailableChargeSlots", newStation.AvailableChargeSlots);
            XElement deleted = new XElement("Deleted", newStation.Deleted);

            stations.Add(new XElement("Station", id, name, longitude, latitude, availableChargeSlots , deleted));

            XMLTools.SaveListToXmlElement(stations, stationsPath);
        }

        /// <summary>
        ///  Removes a parcel from the list of parcels.
        /// </summary>
        /// <param name="stationId"></param>
        public void RemoveStation(int stationId)
        {
            XElement stations = XMLTools.LoadListFromXmlElement(stationsPath);

            var deletedStation = (from s in stations.Elements()
                where Convert.ToInt32(s.Element("Id").Value) == stationId
                select s).FirstOrDefault();

            if (deletedStation is null)
                throw new IdNotFoundException("ERROR: the station is not found!\n");
            if (deletedStation.Element("Deleted").Value == "true")
                throw new IdExistException("ERROR: the station was exist");

            deletedStation.Element("Deleted").Value = "true";

            XMLTools.SaveListToXmlElement(stations, stationsPath);
        }

        /// <summary>
        /// return the specific station the user ask for
        /// </summary>
        /// <param Name="stationId"></the Id of the station the user ask for>
        /// <returns></returns>
        public Station GetStation(int stationId)
        {
            Station getStation = new Station();
            XElement stations = XMLTools.LoadListFromXmlElement(stationsPath);
            try
            {
                getStation = (from station in stations.Elements()
                               where Convert.ToInt32(station.Element("Id").Value) == stationId
                              select new Station()
                               {
                                   Id = Convert.ToInt32(station.Element("Id").Value),
                                   Name = station.Element("Name").Value,
                                   Longitude = Convert.ToDouble(station.Element("Longitude").Value),
                                   Latitude = Convert.ToDouble(station.Element("Latitude").Value),
                                   AvailableChargeSlots = Convert.ToInt32(station.Element("AvailableChargeSlots").Value),
                                   Deleted = Convert.ToBoolean(station.Element("Deleted").Value)
                               }).FirstOrDefault();
            }
            catch
            {
                throw new IdNotFoundException("ERROR: the station is not found.");
            }

            if (getStation.Deleted == true)
                throw new IdExistException("ERROR: the station was exist");

            return getStation;
        }

        /// <summary>
        /// return all the station's list
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Station> GetStations(Predicate<Station> stationPredicate)
        {
            //var stationsXml = XMLTools.LoadListFromXmlSerializer<Station>(stationsPath);
            //IEnumerable<Station> stations = stationsXml.Where(station => stationPredicate(station));


            XElement stationsXml = XMLTools.LoadListFromXmlElement(stationsPath);
            IEnumerable<Station> stations = (from station in stationsXml.Elements()
                select new Station()
                {
                    Id = Convert.ToInt32(station.Element("Id").Value),
                    Name = station.Element("Name").Value,
                    Longitude = Convert.ToDouble(station.Element("Longitude").Value),
                    Latitude = Convert.ToDouble(station.Element("Latitude").Value),
                    AvailableChargeSlots = Convert.ToInt32(station.Element("AvailableChargeSlots").Value),
                    Deleted = Convert.ToBoolean(station.Element("Deleted").Value)
                });
            return stations;
        }

        /// <summary>
        /// update the specific station the user ask for
        /// </summary>
        /// <param name="updateStation"></param>
        public void UpdateStation(Station updateStation)
        {
            XElement stations = XMLTools.LoadListFromXmlElement(stationsPath);

            XElement station = (from s in stations.Elements()
                                 where Convert.ToInt32(s.Element("Id").Value) == updateStation.Id
                                 select s).FirstOrDefault();

            station.Element("Id").Value = updateStation.Id.ToString();
            station.Element("Name").Value = updateStation.Name;
            station.Element("Longitude").Value = updateStation.Longitude.ToString();
            station.Element("Latitude").Value = updateStation.Latitude.ToString();
            station.Element("AvailableChargeSlots").Value = updateStation.AvailableChargeSlots.ToString();
            station.Element("Deleted").Value = updateStation.Deleted.ToString();
            XMLTools.SaveListToXmlElement(stations, stationsPath);
        }
    }
}

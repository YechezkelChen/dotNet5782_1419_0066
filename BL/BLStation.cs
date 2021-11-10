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
                station.InCharges.Add(elementDroneCharge);

            return station;
        }

        public void PrintStations()
        {
            foreach (IDAL.DO.Station elementStation in dal.GetStations())
                Console.WriteLine(elementStation.ToString());
        }

        public void PrintStationsCharge()
        {
            foreach (IDAL.DO.Station elementStation in dal.GetStations())
                if (elementStation.ChargeSlots > 0)
                    Console.WriteLine(elementStation.ToString());
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

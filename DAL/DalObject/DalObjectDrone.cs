using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;
using IDAL;
using DalObject;

namespace DalObject
{
    public partial class DalObject : IDal
    {
        /// <summary>
        /// add a drone to the drone list
        /// </summary>
        /// <param Name="newDrone"></the new drone the user whants to add to the drone's list>
        public void AddDrone(Drone newDrone)
        {
            if (!IsExistDrone(newDrone, DataSource.Drones))
                DataSource.Drones.Add(newDrone);
            else
                throw new DroneException("ERROR: the drone is exist!\n");
        }

        /// <summary>
        /// return the spesifice drone the user ask for
        /// </summary>
        /// <param Name="DdroneId"></the Id of the drone the user ask for>
        /// <returns></returns>
        public Drone GetDrone(int droneId)
        {
            Drone? newDrone = null;
            foreach (Drone elementDrone in DataSource.Drones)
            {
                if (elementDrone.Id == droneId)
                    newDrone = elementDrone;
            }

            if (newDrone == null)
                throw new DroneException("ERROR: Id of drone not found\n");
            return (Drone)newDrone;
        }

        /// <summary>
        /// return all the drone's list
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Drone> GetDrones()
        {
            return DataSource.Drones;
        }

        public void UpdateDrone(Drone drone)
        {
            for (int i = 0; i < DataSource.Drones.Count(); i++)
                if (DataSource.Drones[i].Id == drone.Id)
                    DataSource.Drones[i] = drone;
        }

        /// <summary>
        /// the methode not need exeption becuse she use both sids(true and false)
        /// </summary>
        /// <param Name="d"></the drone we check if she is exist>
        /// <param Name="Drones"></the list od Drones>
        /// <returns></returns>
        public bool IsExistDrone(Drone d, IEnumerable<Drone> drones)
        {
            foreach (Drone elementDrone in drones)
                if (elementDrone.Id == d.Id)
                    return true;
            return false;//the drone not exist
        }








        /// <summary>
        /// send a spesifice drone to a spesifice station that have a availble charge spots, and change the status of the drome to maintenance
        /// </summary>
        /// <param Name="d"></the spesifice drone the user ask for to charge>
        /// <param Name="s"></the spasifice station the user ask to charge the drone in>
        public void SendDroneToDroneCharge(Station s, Drone d)
        {
            DroneCharge newDroneCharges = new DroneCharge();
            if (IsExistStation(s, DataSource.Stations))
            {
                Station newsStation = new Station();
                for (int i = 0; i < DataSource.Stations.Count; i++)
                {
                    if (DataSource.Stations[i].Id == s.Id)
                    {
                        newsStation = DataSource.Stations[i];
                        newsStation.ChargeSlots--;
                        newDroneCharges.StationId = newsStation.Id;
                        DataSource.Stations[i] = newsStation;
                    }
                }
            }
            else
                throw new stationException("ERROR: the station is not exist!\n");

            if (IsExistDrone(d, DataSource.Drones))
            {
                foreach (Drone elementDrone in DataSource.Drones)
                {
                    if (elementDrone.Id == d.Id)
                    {
                        newDroneCharges.DroneId = elementDrone.Id;
                        break;
                    }
                }
                DataSource.DroneCharges.Add(newDroneCharges);
            }
            else
                throw new DroneException("the drone is not exist!\n");
        }

        /// <summary>
        /// release the spesifice drone frome the station he located and update the battry and status to 100 and available
        /// </summary>
        /// <param Name="s"></the spesifice station the user ask to release the drone frome>
        /// <param Name="d"></the spesifice drone the user ask to release frome the station>
        public void ReleaseDroneFromDroneCharge(Station s, Drone d)
        {
            DroneCharge newDroneCharges = new DroneCharge();
            if (IsExistStation(s, DataSource.Stations))
            {
                Station newsStation = new Station();
                for (int i = 0; i < DataSource.Stations.Count; i++)
                {
                    if (DataSource.Stations[i].Id == s.Id)
                    {
                        newsStation = DataSource.Stations[i];
                        newsStation.ChargeSlots++;
                        newDroneCharges.StationId = newsStation.Id;
                        DataSource.Stations[i] = newsStation;
                    }
                }
            }
            else
                throw new stationException("ERROR: the station isn't exist");

            if (IsExistDrone(d, DataSource.Drones))
            {
                foreach (Drone elementDrone in DataSource.Drones)
                {
                    if (elementDrone.Id == d.Id)
                    {
                        newDroneCharges.DroneId = elementDrone.Id;
                        break;
                    }
                }
            }
            else
                throw new DroneException("the drone isn't exist");

            foreach (DroneCharge elementDroneCharge in DataSource.DroneCharges)
            {
                if (elementDroneCharge.StationId == s.Id && elementDroneCharge.DroneId == d.Id)
                    DataSource.DroneCharges.Remove(newDroneCharges);
            }
        }
    }
}
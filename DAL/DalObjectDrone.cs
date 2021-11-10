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
            if (CheckNotExistDrone(newDrone, DataSource.drones))
                DataSource.drones.Add(newDrone);
            else
                throw new DroneExeption("ERROR: the drone is exist!\n");
        }

        /// <summary>
        /// return the spesifice drone the user ask for
        /// </summary>
        /// <param Name="DdroneId"></the Id of the drone the user ask for>
        /// <returns></returns>
        public Drone GetDrone(int droneId)
        {
            Drone? newDrone = null;
            foreach (Drone elementDrone in DataSource.drones)
            {
                if (elementDrone.Id == droneId)
                    newDrone = elementDrone;
            }

            if (newDrone == null)
                throw new DroneExeption("ERROR: Id of drone not found\n");
            return (Drone)newDrone;
        }

        /// <summary>
        /// return all the drone's list
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Drone> GetDrones()
        {
            List<Drone> newDrones = new List<Drone>(DataSource.drones);
            return newDrones;
        }

        /// <summary>
        /// send a spesifice drone to a spesifice station that have a availble charge spots, and change the status of the drome to maintenance
        /// </summary>
        /// <param Name="d"></the spesifice drone the user ask for to charge>
        /// <param Name="s"></the spasifice station the user ask to charge the drone in>
        public void SendDroneToDroneCharge(Station s, Drone d)
        {
            DroneCharge newDroneCharges = new DroneCharge();
            if (!CheckNotExistStation(s, DataSource.stations))
            {
                Station newsStation = new Station();
                for (int i = 0; i < DataSource.stations.Count; i++)
                {
                    if (DataSource.stations[i].Id == s.Id)
                    {
                        newsStation = DataSource.stations[i];
                        newsStation.ChargeSlots--;
                        newDroneCharges.Stationld = newsStation.Id;
                        DataSource.stations[i] = newsStation;
                    }
                }
            }
            else
                throw new StationExeption("ERROR: the station is not exist!\n");

            if (!CheckNotExistDrone(d, DataSource.drones))
            {
                foreach (Drone elementDrone in DataSource.drones)
                {
                    if (elementDrone.Id == d.Id)
                    {
                        newDroneCharges.DdroneId = elementDrone.Id;
                        break;
                    }
                }
                DataSource.droneCharges.Add(newDroneCharges);
            }
            else
                throw new DroneExeption("the drone is not exist!\n");
        }

        /// <summary>
        /// release the spesifice drone frome the station he located and update the battry and status to 100 and available
        /// </summary>
        /// <param Name="s"></the spesifice station the user ask to release the drone frome>
        /// <param Name="d"></the spesifice drone the user ask to release frome the station>
        public void ReleaseDroneFromDroneCharge(Station s, Drone d)
        {
            DroneCharge newDroneCharges = new DroneCharge();
            if (!CheckNotExistStation(s, DataSource.stations))
            {
                Station newsStation = new Station();
                for (int i = 0; i < DataSource.stations.Count; i++)
                {
                    if (DataSource.stations[i].Id == s.Id)
                    {
                        newsStation = DataSource.stations[i];
                        newsStation.ChargeSlots++;
                        newDroneCharges.Stationld = newsStation.Id;
                        DataSource.stations[i] = newsStation;
                    }
                }
            }
            else
                throw new StationExeption("ERROR: the station isn't exist");

            if (!CheckNotExistDrone(d, DataSource.drones))
            {
                foreach (Drone elementDrone in DataSource.drones)
                {
                    if (elementDrone.Id == d.Id)
                    {
                        newDroneCharges.DdroneId = elementDrone.Id;
                        break;
                    }
                }
            }
            else
                throw new DroneExeption("the drone isn't exist");

            foreach (DroneCharge elementDroneCharge in DataSource.droneCharges)
            {
                if (elementDroneCharge.Stationld == s.Id && elementDroneCharge.DdroneId == d.Id)
                    DataSource.droneCharges.Remove(newDroneCharges);
            }
        }

        public double[] GetRequestPowerConsumption()
        {
            double[] powerConsumption = new double[5];
            powerConsumption[0] = DataSource.Config.dAvailable;
            powerConsumption[1] = DataSource.Config.dLightW;
            powerConsumption[2] = DataSource.Config.dMediumW;
            powerConsumption[3] = DataSource.Config.dHeavyW;
            powerConsumption[4] = DataSource.Config.chargingRateOfDrone;
            return powerConsumption;
        }

        /// <summary>
        /// the methode not need exeption becuse she use both sids(true and false)
        /// </summary>
        /// <param Name="d"></the drone we check if she is exist>
        /// <param Name="drones"></the list od drones>
        /// <returns></returns>
        public bool CheckNotExistDrone(Drone d, IEnumerable<Drone> drones)
        {
            foreach (Drone elementDrone in drones)
                if (elementDrone.Id == d.Id)
                    return false;
            return true;//the drone not exist
        }

        public void UpdateDroneModel(int droneId, string newModel)
        {
            for (int i = 0; i < DataSource.drones.Count(); i++)
            {
                if (DataSource.drones[i].Id == droneId)
                {
                    Drone newDrone = DataSource.drones[i];
                    newDrone.Model = newModel;
                    DataSource.drones[i] = newDrone;
                }
            }
        }
    }
}
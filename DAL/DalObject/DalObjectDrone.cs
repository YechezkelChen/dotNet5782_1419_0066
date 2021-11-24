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
        public IEnumerable<Drone> GetDrones(Predicate<Drone> dronePredicate)
        {
            List<Drone> drones = DataSource.Drones.FindAll(dronePredicate);
            return drones;
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
        private bool IsExistDrone(Drone drone, IEnumerable<Drone> drones)
        {
            foreach (Drone elementDrone in drones)
                if (elementDrone.Id == drone.Id)
                    return true;
            return false;//the drone not exist
        }
    }
}
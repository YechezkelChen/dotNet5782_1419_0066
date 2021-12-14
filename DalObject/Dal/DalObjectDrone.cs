using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;


namespace Dal
{
    partial class DalObject : DalApi.IDal
    {
        /// <summary>
        /// add a drone to the drones list
        /// </summary>
        /// <param Name="newDrone"></the new drone the user whants to add to the drone's list>
        public void AddDrone(Drone newDrone)
        {
            if (!IsExistDrone(newDrone.Id))
                DataSource.Drones.Add(newDrone);
            else
                throw new IdExistException("ERROR: the drone is exist!\n");
        }

        /// <summary>
        /// return the spesifice drone the user ask for
        /// </summary>
        /// <param Name="DdroneId"></the Id of the drone the user ask for>
        /// <returns></returns>
        public Drone GetDrone(int droneId)
        {
            if (IsExistDrone(droneId))
            {
                Drone drone = DataSource.Drones.Find(elementDrone => elementDrone.Id == droneId);
                return drone;
            }
            else
                throw new IdNotFoundException("ERROR: the drone is not found.");
        }

        /// <summary>
        /// return all the drone's list
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Drone> GetDrones(Predicate<Drone> dronePredicate)
        {
            IEnumerable<Drone> drones = DataSource.Drones.FindAll(dronePredicate);
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
        private bool IsExistDrone(int droneId)
        {
            foreach (Drone elementDrone in DataSource.Drones)
                if (elementDrone.Id == droneId)
                    return true;
            return false; // the drone not exist
        }
    }
}
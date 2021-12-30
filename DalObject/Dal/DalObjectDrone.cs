using System;
using System.Collections.Generic;
using System.Linq;
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
            string check = IsExistDrone(newDrone.Id);
            if (check == "not exists")
                DataSource.Drones.Add(newDrone);
            if (check == "exists")
                throw new IdExistException("ERROR: the drone is exist");
            if (check == "was exists")
                throw new IdExistException("ERROR: the drone was exist");
        }

        /// <summary>
        /// Removes a drone from the list of drones.
        /// </summary>
        /// <param name="droneId"></param>
        public void RemoveDrone(int droneId)
        {

        }

        /// <summary>
        /// return the specific drone the user ask for
        /// </summary>
        /// <param Name="DdroneId"></the Id of the drone the user ask for>
        /// <returns></returns>
        public Drone GetDrone(int droneId)
        {
            string check = IsExistDrone(droneId);
            Drone drone = new Drone();
            if (check == "exists")
            {
                drone = DataSource.Drones.Find(elementDrone => elementDrone.Id == droneId);
            }
            if(check == "not exists")
                throw new IdNotFoundException("ERROR: the drone is not found.");
            if (check == "was exists")
                throw new IdExistException("ERROR: the drone was exist");

            return drone;
        }

        /// <summary>
        /// return all the drone's list
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Drone> GetDrones(Predicate<Drone> dronePredicate)
        {
            IEnumerable<Drone> drones = DataSource.Drones.Where(drone => dronePredicate(drone));
            return drones;
        }

        public void UpdateDrone(Drone drone)
        {
            for (int i = 0; i < DataSource.Drones.Count(); i++)
                if (DataSource.Drones[i].Id == drone.Id)
                    DataSource.Drones[i] = drone;
        }

        /// <summary>
        /// the method not need exception because she use both sids(true and false)
        /// </summary>
        /// <param Name="d"></the drone we check if she is exist>
        /// <param Name="Drones"></the list od Drones>
        /// <returns></returns>
        private string IsExistDrone(int droneId)
        {
            foreach (Drone elementDrone in DataSource.Drones)
            {
                if (elementDrone.Id == droneId && elementDrone.Deleted == false)
                    return "exists";
                if (elementDrone.Id == droneId && elementDrone.Deleted == true)
                    return "was exists";
            }
            return "not exists"; // the drone not exist
        }
    }
}
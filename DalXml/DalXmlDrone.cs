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
        /// add a drone to the drones list
        /// </summary>
        /// <param Name="newDrone"></the new drone the user whants to add to the drone's list>
        public void AddDrone(Drone newDrone)
        {
            XElement drones = XMLTools.LoadListFromXmlElement(dronesPath);

            var addDrone = (from d in drones.Elements()
                where Convert.ToInt32(d.Element("Id").Value) == newDrone.Id
                select d).FirstOrDefault();

            if (!(addDrone is null))
            {
                if (addDrone.Element("Deleted").Value == "true")
                    throw new IdNotFoundException("ERROR: the drone is deleted!\n");

                throw new IdNotFoundException("ERROR: the drone is found!\n");
            }

            XElement id = new XElement("Id", newDrone.Id);
            XElement model = new XElement("Model", newDrone.Model);
            XElement weight = new XElement("Weight", newDrone.Weight);
            XElement deleted = new XElement("Deleted", newDrone.Deleted);
            
            drones.Add(new XElement("Drone", id, model, weight, deleted));

            XMLTools.SaveListToXmlElement(drones, dronesPath);
        }

        /// <summary>
        /// Removes a drone from the list of drones.
        /// </summary>
        /// <param name="droneId"></param>
        public void RemoveDrone(int droneId)
        {
            XElement drones = XMLTools.LoadListFromXmlElement(dronesPath);

            var deletedDrone = (from d in drones.Elements()
                where Convert.ToInt32(d.Element("Id").Value) == droneId
                select d).FirstOrDefault();

            if (deletedDrone is null)
                throw new IdNotFoundException("ERROR: the drone is not found!\n");
            if (deletedDrone.Element("Deleted").Value == "true")
                throw new IdExistException("ERROR: the drone was exist");

            deletedDrone.Element("Deleted").Value = "true";

            XMLTools.SaveListToXmlElement(drones, dronesPath);
        }

        /// <summary>
        /// return the specific drone the user ask for
        /// </summary>
        /// <param Name="DdroneId"></the Id of the drone the user ask for>
        /// <returns></returns>
        public Drone GetDrone(int droneId)
        {
            Drone getDrone = new Drone();
            XElement drones = XMLTools.LoadListFromXmlElement(dronesPath);
            try
            {
                getDrone = (from drone in drones.Elements()
                    where Convert.ToInt32(drone.Element("Id").Value) == droneId
                    select new Drone()
                    {
                        Id = Convert.ToInt32(drone.Element("Id").Value),
                        Model = drone.Element("Model").Value,
                        Weight = (WeightCategories) Enum.Parse(typeof(WeightCategories), drone.Element("Weight").Value.ToString()),
                        Deleted = Convert.ToBoolean(drone.Element("Deleted").Value)
                    }).FirstOrDefault();
            }
            catch
            {
                throw new IdNotFoundException("ERROR: the drone is not found.");
            }
     
            if(getDrone.Deleted == true)
                throw new IdExistException("ERROR: the drone was exist");

            return getDrone;
        }
        
        /// <summary>
        /// return all the drone's list
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Drone> GetDrones(Predicate<Drone> dronePredicate)
        {
            var dronesXml = XMLTools.LoadListFromXmlSerializer<Drone>(dronesPath);
            IEnumerable<Drone> drones = dronesXml.Where(drone => dronePredicate(drone));
            return drones;
        }

        /// <summary>
        /// update the specific drone the user ask for
        /// </summary>
        /// <param name="updateDrone"></param>
        public void UpdateDrone(Drone updateDrone)
        {
            XElement drones = XMLTools.LoadListFromXmlElement(dronesPath);

            XElement drone = (from d in drones.Elements()
                where Convert.ToInt32(d.Element("Id").Value) == updateDrone.Id
                select d).FirstOrDefault();

            drone.Element("Id").Value = updateDrone.Id.ToString();
            drone.Element("Model").Value = updateDrone.Model;
            drone.Element("Weight").Value = updateDrone.Weight.ToString();
            drone.Element("Deleted").Value = updateDrone.Deleted.ToString();

            XMLTools.SaveListToXmlElement(drones, dronesPath);
        }
    }
}

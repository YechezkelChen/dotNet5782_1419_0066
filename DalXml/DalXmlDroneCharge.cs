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
        /// add a drone charge to the drone charge list
        /// </summary>
        /// <param Name="newDroneCharge"></the new drone charge the user whants to add to the drone's list>
        public void AddDroneCharge(DroneCharge newDroneCharge)
        {
            XElement dronesCharge = XMLTools.LoadListFromXmlElement(dronesChargePath);
            
            var addDroneCharge = (from d in dronesCharge.Elements()
                where Convert.ToInt32(d.Element("DroneId").Value) == newDroneCharge.DroneId &&
                      Convert.ToInt32(d.Element("StationId").Value) == newDroneCharge.StationId
                select d).FirstOrDefault();

            if (!(addDroneCharge is null))
            {
                if (addDroneCharge.Element("Deleted").Value == "true")
                    throw new IdExistException("ERROR: the drone charge is deleted!\n");

                throw new IdExistException("ERROR: the drone charge is found!\n");
            }

            XElement droneId = new XElement("DroneId", newDroneCharge.DroneId);
            XElement stationId = new XElement("StationId", newDroneCharge.StationId);
            XElement startCharging = new XElement("StartCharging", newDroneCharge.StartCharging);
            XElement deleted = new XElement("Deleted", newDroneCharge.Deleted);

            dronesCharge.Add(new XElement("DroneCharge", droneId, stationId, startCharging, deleted));

            XMLTools.SaveListToXmlElement(dronesCharge, dronesChargePath);
        }

        /// <summary>
        /// remove a drone charge from the drone charge list
        /// </summary>
        /// <param Name="newDroneCharge"></the new drone charge the user whants to add to the drone's list>
        public void RemoveDroneCharge(DroneCharge droneCharge)
        {
            XElement dronesCharge = XMLTools.LoadListFromXmlElement(dronesChargePath);

            var deletedDroneCharge = (from d in dronesCharge.Elements()
                where Convert.ToInt32(d.Element("DroneId").Value) == droneCharge.DroneId &&
                      Convert.ToInt32(d.Element("StationId").Value) == droneCharge.StationId
                select d).FirstOrDefault();

            if(deletedDroneCharge is null)
                throw new IdNotFoundException("ERROR: the drone charge is not found!\n");
            if(deletedDroneCharge.Element("Deleted").Value == "true")
                throw new IdNotFoundException("ERROR: the drone charge is deleted!\n");

            deletedDroneCharge.Element("Deleted").Value = "true";

            XMLTools.SaveListToXmlElement(dronesCharge, dronesChargePath);
        }

        /// <summary>
        /// return all the list of the drone's that they are in charge sopt 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DroneCharge> GetDronesCharge(Predicate<DroneCharge> droneChargePredicate)
        {
            //var dronesChargeXml = XMLTools.LoadListFromXmlSerializer<DroneCharge>(dronesChargePath);
            //IEnumerable<DroneCharge> dronesCharge = dronesChargeXml.Where(dronesCharge => droneChargePredicate(dronesCharge));

            XElement dronesChargeXml = XMLTools.LoadListFromXmlElement(dronesChargePath);
            IEnumerable<DroneCharge> dronesCharge = (from droneCharge in dronesChargeXml.Elements()
                select new DroneCharge()
                {
                    DroneId = Convert.ToInt32(droneCharge.Element("DroneId").Value),
                    StationId = Convert.ToInt32(droneCharge.Element("StationId").Value),
                    StartCharging = DateTime.Parse(droneCharge.Element("StartCharging").Value),
                    Deleted = Convert.ToBoolean(droneCharge.Element("Deleted").Value)
                });
            return dronesCharge;
        }
    }
}
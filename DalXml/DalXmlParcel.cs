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
        /// add a parcel to the parcel list and return the new parcel Id that was create
        /// </summary>
        /// <param Name="newParcel"></the new parcel the user whants to add to the parcel's list>
        /// <returns></returns>
        public int AddParcel(Parcel newParcel)
        {
            XElement parcels = XMLTools.LoadListFromXmlElement(parcelsPath);

            XElement id = new XElement("Id", 1); //??????????????????????????????????????????????????????????
            XElement senderId = new XElement("SenderId", newParcel.SenderId);
            XElement targetId = new XElement("TargetId", newParcel.TargetId);
            XElement weight = new XElement("Weight", newParcel.Weight);
            XElement priority = new XElement("Priority", newParcel.Priority);
            XElement droneId = new XElement("DroneId", newParcel.DroneId);
            XElement requested = new XElement("Requested", newParcel.Requested);
            XElement scheduled = new XElement("Scheduled", newParcel.Scheduled);
            XElement pickedUp = new XElement("PickedUp", newParcel.PickedUp);
            XElement delivered = new XElement("Delivered", newParcel.Delivered);
            XElement deleted = new XElement("Deleted", newParcel.Deleted);

            parcels.Add(new XElement("Parcel", id, senderId, targetId, weight, priority, droneId, requested, scheduled,
                pickedUp, delivered, deleted));

            XMLTools.SaveListToXmlElement(parcels, parcelsPath);
            return 1; //?????????????????????????????????????????????????????????????????????????????????????????????
        }

        /// <summary>
        /// Removes a parcel from the list of parcels.
        /// </summary>
        /// <param name="parcelId"></param>
        public void RemoveParcel(int parcelId)
        {
            XElement parcels = XMLTools.LoadListFromXmlElement(parcelsPath);

            var deletedParcel = (from p in parcels.Elements()
                where Convert.ToInt32(p.Element("Id").Value) == parcelId
                select p).FirstOrDefault();

            if (!(deletedParcel is null))
                throw new IdNotFoundException("ERROR: the parcel is found!\n");
            if (deletedParcel.Element("Deleted").Value == "true")
                throw new IdExistException("ERROR: the parcel was exist");

            deletedParcel.Element("Deleted").Value = "true";

            XMLTools.SaveListToXmlElement(parcels, parcelsPath);
        }

        /// <summary>
        /// return the specific parcel the user ask for
        /// </summary>
        /// <param Name="parcelId"></the Id parcel the user ask for>
        /// <returns></returns>
        public Parcel GetParcel(int parcelId)
        {
            Parcel getParcel = new Parcel();
            XElement parcels = XMLTools.LoadListFromXmlElement(parcelsPath);
            try
            {
                getParcel = (from parcel in parcels.Elements()
                    where Convert.ToInt32(parcel.Element("Id").Value) == parcelId
                    select new Parcel()
                    {
                        Id = Convert.ToInt32(parcel.Element("Id").Value),
                        SenderId = Convert.ToInt32(parcel.Element("SenderId").Value),
                        TargetId = Convert.ToInt32(parcel.Element("TargetId").Value),
                        Weight = (WeightCategories) Enum.Parse(typeof(WeightCategories),
                            parcel.Element("Weight").Value.ToString()),
                        Priority = (Priorities) Enum.Parse(typeof(Priorities),
                            parcel.Element("Priority").Value.ToString()),
                        DroneId = Convert.ToInt32(parcel.Element("DroneId").Value),
                        Requested = DateTime.Parse(parcel.Element("Requested").Value),
                        Scheduled = DateTime.Parse(parcel.Element("Scheduled").Value),
                        PickedUp = DateTime.Parse(parcel.Element("PickedUp").Value),
                        Delivered = DateTime.Parse(parcel.Element("Delivered").Value),
                        Deleted = Convert.ToBoolean(parcel.Element("Deleted").Value)
                    }).FirstOrDefault();
            }
            catch
            {
                throw new IdNotFoundException("ERROR: the parcel is not found.");
            }

            if (getParcel.Deleted == true)
                throw new IdExistException("ERROR: the parcel was exist");

            return getParcel;
        }

        /// <summary>
        /// return all the parcel in the list
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Parcel> GetParcels(Predicate<Parcel> parcelPredicate)
        {
            var parcelsXml = XMLTools.LoadListFromXmlSerializer<Parcel>(parcelsPath);
            IEnumerable<Parcel> parcels = parcelsXml.Where(parcel => parcelPredicate(parcel));
            return parcels;
        }

        /// <summary>
        /// update the specific parcel the user ask for
        /// </summary>
        /// <param name="updateDrone"></param>
        public void UpdateParcel(Parcel updateParcel)
        {
            XElement parcels = XMLTools.LoadListFromXmlElement(parcelsPath);

            XElement parcel = (from p in parcels.Elements()
                where Convert.ToInt32(p.Element("Id").Value) == updateParcel.Id
                select p).FirstOrDefault();

            parcel.Element("Id").Value = updateParcel.Id.ToString();
            parcel.Element("SenderId").Value = updateParcel.SenderId.ToString();
            parcel.Element("TargetId").Value = updateParcel.TargetId.ToString();
            parcel.Element("Weight").Value = updateParcel.Weight.ToString();
            parcel.Element("Priority").Value = updateParcel.Priority.ToString();
            parcel.Element("DroneId").Value = updateParcel.DroneId.ToString();
            parcel.Element("Requested").Value = updateParcel.Requested.ToString();
            parcel.Element("Scheduled").Value = updateParcel.Scheduled.ToString();
            parcel.Element("PickedUp").Value = updateParcel.PickedUp.ToString();
            parcel.Element("Delivered").Value = updateParcel.Delivered.ToString();
            parcel.Element("Deleted").Value = updateParcel.Deleted.ToString();

            XMLTools.SaveListToXmlElement(parcels, parcelsPath);
        }
    }
}

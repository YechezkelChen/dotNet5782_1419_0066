using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
        /// add a parcel to the parcel list and return the new parcel Id that was create
        /// </summary>
        /// <param Name="newParcel"></the new parcel the user whants to add to the parcel's list>
        /// <returns></returns>
        public int AddParcel(Parcel newParcel)
        {
            int tmp = DataSource.Config.ParcelsId;
            if (CheckNotExistParcel(newParcel, DataSource.parcels))
            {
                newParcel.Id = DataSource.Config.ParcelsId; // insert the Parcels new Id
                DataSource.Config.ParcelsId++; // new Id for the fautre parce Id
                DataSource.parcels.Add(newParcel);
            }
            else
                throw new ParcelExeption("ERROR: the parcel is exist!\n");
            return tmp; // return the new number created
        }

        /// <summary>
        /// return the spesifice parcel the user ask for
        /// </summary>
        /// <param Name="parcelId"></the Id parcel the user ask for>
        /// <returns></returns>
        public Parcel GetParcel(int parcelId)
        {
            Parcel? newParcel = null;
            foreach (Parcel elementParcel in DataSource.parcels)
            {
                if (elementParcel.Id == parcelId)
                    newParcel = elementParcel;
            }

            if (newParcel == null)
                throw new ParcelExeption("ERROR: Id of parcel not found\n");
            return (Parcel)newParcel;
        }

        /// <summary>
        /// return all the parcel in the list
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Parcel> GetParcels()
        {
            List<Parcel> newParcels = new List<Parcel>(DataSource.parcels);
            return newParcels;
        }

        /// <summary>
        /// connecting between parcel and drone and chenge data in the object's according to the function
        /// </summary>
        /// <param Name="p"></the parcel the user ask to connect with the drone he ask>
        /// <param Name="d"></the drone the user ask to connect with the parcel he ask >
        public void ConnectParcelToDrone(Parcel p, Drone d)
        {
            if (!CheckNotExistParcel(p, DataSource.parcels))
                if (!CheckNotExistDrone(d, DataSource.drones))
                {
                    Parcel newParcel = new Parcel();
                    for (int i = 0; i < DataSource.parcels.Count; i++)
                    {
                        if (DataSource.parcels[i].Id == p.Id)
                        {
                            newParcel = DataSource.parcels[i];
                            newParcel.Requested = DateTime.Now;
                            newParcel.Scheduled = DateTime.Now;
                            newParcel.DroneId = d.Id;
                            DataSource.parcels[i] = newParcel;
                        }
                    }
                }
                else
                    throw new DroneExeption("ERROR: the drone isn't exist");
            else
                throw new ParcelExeption("ERROR: the parcel isn't exist");
        }

        /// <summary>
        /// chenge the pick up statuse of the parcel the user ask for by searching the parcel in the list and update the pick up time statuse of the parcel
        /// </summary>
        /// <param Name="p"></the spesific parcel the user ask to update as pick'd up>
        public void CollectionParcelByDrone(Parcel p)
        {
            if (!CheckNotExistParcel(p, DataSource.parcels))
            {
                Parcel newParcel = new Parcel();
                for (int i = 0; i < DataSource.parcels.Count; i++)
                {
                    if (DataSource.parcels[i].Id == p.Id)
                    {
                        newParcel = DataSource.parcels[i];
                        newParcel.PickedUp = DateTime.Now;
                        DataSource.parcels[i] = newParcel;
                    }
                }
            }
            else
                throw new ParcelExeption("ERROR: the parcel is not exist!\n");
        }

        /// <summary>
        /// chenge the deliversd statuse of the parcel the user ask for by searching the parcel in the list and update the deliverd time statuse of the parcel
        /// </summary>
        /// <param Name="p"></the specific parcel the user ask to pdate as deliver'd >
        public void SupplyParcelToCustomer(Parcel p)
        {
            if (!CheckNotExistParcel(p, DataSource.parcels))
            {
                Parcel newParcel = new Parcel();
                for (int i = 0; i < DataSource.parcels.Count; i++)
                {
                    if (DataSource.parcels[i].Id == p.Id)
                    {
                        newParcel = DataSource.parcels[i];
                        newParcel.Delivered = DateTime.Now;
                        DataSource.parcels[i] = newParcel;
                    }
                }
            }
            else
                throw new ParcelExeption("ERROR: the parcel isn't exist");
        }

        /// <summary>
        /// the methode not need exeption becuse she use both sids(true and false)
        /// </summary>
        /// <param Name="d"></the parcel we check if she is exist>
        /// <param Name="drones"></the list of parcels>
        /// <returns></returns>
        public bool CheckNotExistParcel(Parcel p, IEnumerable<Parcel> parcels)
        {
            foreach (Parcel elementParcel in parcels)
                if (elementParcel.Id == p.Id)
                    return false;
            return true;//the drone not exist
        }
    }
}

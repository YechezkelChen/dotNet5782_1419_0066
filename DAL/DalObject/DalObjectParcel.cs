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
            if (!IsExistParcel(newParcel, DataSource.Parcels))
            {
                newParcel.Id = DataSource.Config.ParcelsId; // insert the Parcels new Id
                DataSource.Config.ParcelsId++; // new Id for the fautre parce Id
                DataSource.Parcels.Add(newParcel);
            }
            else
                throw new ParcelException("ERROR: the parcel is exist!\n");
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
            foreach (Parcel elementParcel in DataSource.Parcels)
            {
                if (elementParcel.Id == parcelId)
                    newParcel = elementParcel;
            }

            if (newParcel == null)
                throw new ParcelException("ERROR: Id of parcel not found\n");
            return (Parcel)newParcel;
        }

        /// <summary>
        /// return all the parcel in the list
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Parcel> GetParcels(Predicate<Parcel> parcelPredicate)
        {
            List<Parcel> parcels = DataSource.Parcels.FindAll(parcelPredicate);
            return parcels;
        }

        public void UpdateParcel(Parcel parcel)
        {
            for (int i = 0; i < DataSource.Parcels.Count(); i++)
                if (DataSource.Parcels[i].Id == parcel.Id)
                    DataSource.Parcels[i] = parcel;
        }

        /// <summary>
        /// the methode not need exeption becuse she use both sids(true and false)
        /// </summary>
        /// <param Name="d"></the parcel we check if she is exist>
        /// <param Name="Drones"></the list of Parcels>
        /// <returns></returns>
        public bool IsExistParcel(Parcel parcel, IEnumerable<Parcel> parcels)
        {
            foreach (Parcel elementParcel in parcels)
                if (elementParcel.Id == parcel.Id)
                    return true;
            return false;//the drone not exist
        }
    }
}

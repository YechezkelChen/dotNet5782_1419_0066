using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using DO;


namespace Dal
{
    partial class DalObject : DalApi.IDal
    {
        /// <summary>
        /// add a parcel to the parcel list and return the new parcel Id that was create
        /// </summary>
        /// <param Name="newParcel"></the new parcel the user whants to add to the parcel's list>
        /// <returns></returns>
        public int AddParcel(Parcel newParcel)
        {
            int runNumber = DataSource.Config.ParcelsId;
            string check = IsExistParcel(newParcel.Id);
            if (check == "not exists")
            {
                newParcel.Id = DataSource.Config.ParcelsId; // insert the Parcels new Id
                DataSource.Config.ParcelsId++;
                DataSource.Parcels.Add(newParcel);
            }
            if (check == "exists")
                throw new IdExistException("ERROR: the parcel is exist");
            if (check == "was exists")
                throw new IdExistException("ERROR: the parcel was exist");
            return runNumber;
        }


        /// <summary>
        /// return the spesifice parcel the user ask for
        /// </summary>
        /// <param Name="parcelId"></the Id parcel the user ask for>
        /// <returns></returns>
        public Parcel GetParcel(int parcelId)
        {
            string check = IsExistParcel(parcelId);
            Parcel parcel = new Parcel();
            if (check == "exists")
            {
                parcel = DataSource.Parcels.Find(elementParcel => elementParcel.Id == parcelId);
            }
            if (check == "not exists")
                throw new IdNotFoundException("ERROR: the parcel is not found.");
            if (check == "was exists")
                throw new IdExistException("ERROR: the parcel was exist");

            return parcel;
        }

        //public Parcel GetParcel(int parcelId)
        //{

        //    if (IsExistParcel(parcelId))
        //    {
        //        Parcel parcel = DataSource.Parcels.Find(elementParcel => elementParcel.Id == parcelId);
        //        return parcel;
        //    }
        //    else
        //        throw new IdNotFoundException("ERROR: the parcel is not found.");
        //}

        /// <summary>
        /// return all the parcel in the list
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Parcel> GetParcels(Predicate<Parcel> parcelPredicate)
        {
            IEnumerable<Parcel> parcels = DataSource.Parcels.Where(parcel => parcelPredicate(parcel));
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
        private string IsExistParcel(int parcelId)
        {
            foreach (Parcel elementParcel in DataSource.Parcels)
            {
                if (elementParcel.Id == parcelId && elementParcel.Deleted == false)
                    return "exists";
                if (elementParcel.Id == parcelId && elementParcel.Deleted == true)
                    return "was exists";
            }
            return "not exists"; // the customer not exist
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using DO;
using System.Runtime.CompilerServices;


namespace Dal
{
    partial class DalObject : DalApi.IDal
    {
        /// <summary>
        /// add a parcel to the parcel list and return the new parcel Id that was create
        /// </summary>
        /// <param Name="newParcel"></the new parcel the user whants to add to the parcel's list>
        /// <returns></returns>
       
        [MethodImpl(MethodImplOptions.Synchronized)]
        public int AddParcel(Parcel newParcel)
        {
            int runNumber = DataSource.Config.NewParcelId;
            string check = IsExistParcel(newParcel.Id);
            if (check == "not exists")
            {
                newParcel.Id = DataSource.Config.NewParcelId; // insert the Parcels new Id
                DataSource.Config.NewParcelId++;
                DataSource.Parcels.Add(newParcel);
            }
            if (check == "exists")
                throw new IdExistException("ERROR: the parcel is exist");
            if (check == "was exists")
                throw new IdExistException("ERROR: the parcel was exist");
            return runNumber;
        }

        /// <summary>
        /// Removes a parcel from the list of parcels.
        /// </summary>
        /// <param name="parcelId"></param>
       
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void RemoveParcel(int parcelId)
        {
            string check = IsExistParcel(parcelId);

            if (check == "not exists")
                throw new IdNotFoundException("ERROR: the parcel is not found!\n");
            if (check == "was exists")
                throw new IdExistException("ERROR: the parcel was exist");

            if (check == "exists")
            {
                for (int i = 0; i < DataSource.Parcels.Count(); i++)
                {
                    Parcel elementParcel = DataSource.Parcels[i];
                    if (elementParcel.Id == parcelId)
                    {
                        elementParcel.Deleted = true;
                        DataSource.Parcels[i] = elementParcel;
                    }
                }
            }
        }

        /// <summary>
        /// return the specific parcel the user ask for
        /// </summary>
        /// <param Name="parcelId"></the Id parcel the user ask for>
        /// <returns></returns>
      
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Parcel GetParcel(int parcelId)
        {
            string check = IsExistParcel(parcelId);

            if (check == "not exists")
                throw new IdNotFoundException("ERROR: the parcel is not found.");
            if (check == "was exists")
                throw new IdExistException("ERROR: the parcel was exist");

            Parcel parcel = new Parcel();
            if (check == "exists") 
                parcel = DataSource.Parcels.Find(elementParcel => elementParcel.Id == parcelId);

            return parcel;
        }

        /// <summary>
        /// return all the parcel in the list
        /// </summary>
        /// <returns></returns>
        
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Parcel> GetParcels(Predicate<Parcel> parcelPredicate)
        {
            IEnumerable<Parcel> parcels = DataSource.Parcels.Where(parcel => parcelPredicate(parcel));
            return parcels;
        }

        /// <summary>
        /// update the specific parcel the user ask for
        /// </summary>
        /// <param name="parcel"></the parcek to updata>
      
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateParcel(Parcel parcel)
        {
            for (int i = 0; i < DataSource.Parcels.Count(); i++)
                if (DataSource.Parcels[i].Id == parcel.Id)
                    DataSource.Parcels[i] = parcel;
        }

        /// <summary>
        /// the method not need exception because she use both sids(true and false)
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
using IDAL;


namespace IBL
{
    public partial class BL : IBL
    {
        public void AddParcel(Parcel newParcel)
        {
            try
            {
                CheckParcel(newParcel);
            }
            catch (ParcelException e)
            {
                throw new ParcelException("" + e);
            }
            IDAL.DO.Parcel parcel = new IDAL.DO.Parcel();
            parcel.SenderId = newParcel.SenderId;
            parcel.TargetId = newParcel.TargetId;
            parcel.Weight = (IDAL.DO.WeightCategories)newParcel.Weight;
            parcel.Priority = (IDAL.DO.Priorities)newParcel.Priority;
            parcel.DroneId = 0;
            parcel.Requested = DateTime.Now;
            parcel.Scheduled = DateTime.MinValue;
            parcel.PickedUp = DateTime.MinValue;
            parcel.Delivered = DateTime.MinValue;
            dal.AddParcel(parcel);
        }

        public Parcel GetParcel(int id)
        {
            IDAL.DO.Parcel idalParcel = new IDAL.DO.Parcel();
            try
            {
                idalParcel = dal.GetParcel(id);
            }
            catch (DalObject.ParcelExeption e)
            {
                throw new ParcelException("" + e);
            }

            Parcel parcel = new Parcel();
            parcel.Id = idalParcel.Id;
            parcel.SenderId = idalParcel.SenderId;
            parcel.TargetId = idalParcel.TargetId;
            parcel.Weight = Enum.Parse<WeightCategories>(idalParcel.Weight.ToString());
            parcel.Priority = Enum.Parse<Priorities>(idalParcel.Priority.ToString());

            foreach (var elementDrone in ListDrones)
                if (elementDrone.Id == idalParcel.DroneId)
                {
                    parcel.DroneInParcel.Id = elementDrone.Id;
                    parcel.DroneInParcel.Battery = elementDrone.Battery;
                    parcel.DroneInParcel.Location = elementDrone.Location;
                }

            parcel.Requested = idalParcel.Requested;
            parcel.Scheduled = idalParcel.Scheduled;
            parcel.PickedUp = idalParcel.PickedUp;
            parcel.Delivered = idalParcel.Delivered;

            return parcel;
        }

        public IEnumerable<ParcelToList> GetParcels()
        {
            IEnumerable<IDAL.DO.Parcel> idalParcels = dal.GetParcels();
            List<ParcelToList> parcelToLists = new List<ParcelToList>();
            ParcelToList newParcel = new ParcelToList();

            foreach (var idalParcel in idalParcels)
            {
                newParcel.Id = idalParcel.Id;
                newParcel.SenderId = idalParcel.SenderId;
                newParcel.TargetId = idalParcel.TargetId;
                newParcel.Weight = Enum.Parse<WeightCategories>(idalParcel.Weight.ToString());
                newParcel.Priority = Enum.Parse<Priorities>(idalParcel.Priority.ToString());

                if (idalParcel.Requested != DateTime.MinValue)
                    newParcel.ParcelStatuses = ParcelStatuses.Requested;
                if (idalParcel.Scheduled != DateTime.MinValue)
                    newParcel.ParcelStatuses = ParcelStatuses.Scheduled;
                if (idalParcel.PickedUp != DateTime.MinValue)
                    newParcel.ParcelStatuses = ParcelStatuses.PickedUp;
                if (idalParcel.Delivered != DateTime.MinValue)
                    newParcel.ParcelStatuses = ParcelStatuses.Delivered;

                parcelToLists.Add(newParcel);
            }

            return parcelToLists;
        }

        public IEnumerable<ParcelToList> GetParcelsNoDrones()
        {
            IEnumerable<IDAL.DO.Parcel> idalParcels = dal.GetParcels();
            List<ParcelToList> parcelToLists = new List<ParcelToList>();
            ParcelToList newParcel = new ParcelToList();

        }

        public void CheckParcel(Parcel parcel)
        {
            if (parcel.SenderId < 0)
                throw new ParcelException("ERROR: the Sender ID is illegal! ");
            if (parcel.TargetId < 0)
                throw new ParcelException("ERROR: the Target ID is illegal! ");
            if (parcel.SenderId == parcel.TargetId)
                throw new ParcelException("ERROR: the Target ID and the Sender ID are equals! ");
        }
    }
}
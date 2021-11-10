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
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Parcel parcel = new Parcel();

            return parcel;
        }

        public void PrintParcels()
        {
            foreach (IDAL.DO.Parcel elementParcel in dal.GetParcels())
                Console.WriteLine(elementParcel.ToString());
        }

        public void PrintParcelsNoDrones()
        {
            foreach (IDAL.DO.Parcel elementParcel in dal.GetParcels())
                if (elementParcel.DroneId == -1)//the Id drone is not exist
                    Console.WriteLine(elementParcel.ToString());
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
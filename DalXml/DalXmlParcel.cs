using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;

namespace Dal
{
    partial class DalXml : DalApi.IDal
    {
        public int AddParcel(Parcel newParcel)
        {
            throw new NotImplementedException();
        }

        public Parcel GetParcel(int parcelId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Parcel> GetParcels(Predicate<Parcel> parcelPredicate)
        {
            throw new NotImplementedException();
        }

        public void UpdateParcel(Parcel parcel)
        {
            throw new NotImplementedException();
        }
    }
}

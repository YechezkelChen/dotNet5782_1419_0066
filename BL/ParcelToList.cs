using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class ParcelToList
        {
            public string Id { get; set; }
            public string SenderId { get; set; }
            public int TargetId { get; set; }
            public WeightCategories Weight { get; set; }
            public Priorities Priority { get; set; }
            public ParcelStatuses ParcelStatuses { get; set; }
            public override string ToString()
            {
                return $"Id #{Id}: Sender id = {SenderId},Target id = {TargetId}," +
                       $"Weight = {Weight},  Priority = {Priority}, Parcel statuses = {ParcelStatuses}";
            }
        }
    }
}

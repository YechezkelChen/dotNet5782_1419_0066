﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class Parcel
        {
            public int Id { get; set; }
            public int SenderId { get; set; }
            public int TargetId { get; set; }
            public WeightCategories Weight { get; set; }
            public Priorities Priority { get; set; }
            public int DroneId { get; set; }
            public DateTime Requested { get; set; }//יצירה
            public DateTime Scheduled { get; set; }//שיוך
            public DateTime PickedUp { get; set; }//איסוף
            public DateTime Delivered { get; set; }//אספקה
        }
    }
}

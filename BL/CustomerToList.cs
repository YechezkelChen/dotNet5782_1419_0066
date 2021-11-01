﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        class CustomerToList
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Phone { get; set; }
            public int SenderParcelPickedUp { get; set; }
            public int SenderParcelScheduled { get; set; }
            public int TargetParcelDelivered { get; set; }
            public int TargetParcelPickedUp { get; set; }
        }
    }
}

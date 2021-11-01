using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class ParcelInTransfer
        {
            public int id { get; set; }
            public Priorities priority { get; set; }
            public CustomerInDelivery senderInDelivery { get; set; }//the sender
            public CustomerInDelivery receiverInDelivery { get; set; }//the receiver
        }
    }
}

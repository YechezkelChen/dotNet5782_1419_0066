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
            public int Id { get; set; }
            public Priorities Priority { get; set; }
            public CustomerInDelivery SenderInDelivery { get; set; }//the sender
            public CustomerInDelivery ReceiverInDelivery { get; set; }//the receiver
        }
    }
}

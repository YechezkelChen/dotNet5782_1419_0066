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
        public void AddDroneCharge(DroneCharge newDroneCharge)
        {
            throw new NotImplementedException();
        }

        public void RemoveDroneCharge(DroneCharge DroneCharge)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DroneCharge> GetDronesCharge(Predicate<DroneCharge> droneChargePredicate)
        {
            throw new NotImplementedException();
        }
    }
}
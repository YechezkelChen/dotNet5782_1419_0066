using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;


namespace Dal
{
    partial class DalObject : DalApi.IDal
    {
        /// <summary>
        /// add a drone charge to the drone list
        /// </summary>
        /// <param Name="newDroneCharge"></the new drone charge the user whants to add to the drone's list>
        public void AddDroneCharge(DroneCharge newDroneCharge)
        {
            if (!IsExistDroneCharge(newDroneCharge, DataSource.DroneCharges))
                DataSource.DroneCharges.Add(newDroneCharge);
            else
                throw new IdExistException("ERROR: the drone charge is exist!\n");
        }

        public void RemoveDroneCharge(DroneCharge DroneCharge)
        {
            if (IsExistDroneCharge(DroneCharge, DataSource.DroneCharges))
                DataSource.DroneCharges.Remove(DroneCharge);
            else
                throw new IdNotFoundException("ERROR: the drone charge is not found!\n");
        }

        /// <summary>
        /// return all the list of the drone's that they are in charge sopt 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DroneCharge> GetDronesCharge(Predicate<DroneCharge> droneChargePredicate)
        {
            IEnumerable<DroneCharge> dronesCharge = DataSource.DroneCharges.FindAll(droneChargePredicate);
            return dronesCharge;
        }

        private bool IsExistDroneCharge(DroneCharge droneCharge, IEnumerable<DroneCharge> droneCharges)
        {
            foreach (DroneCharge elementDroneCharge in droneCharges)
                if (elementDroneCharge.DroneId == droneCharge.DroneId && elementDroneCharge.StationId == droneCharge.StationId) 
                    return true;
            return false;//the drone not exist
        }
    }
}
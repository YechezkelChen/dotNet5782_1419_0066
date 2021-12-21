using System;
using System.Collections.Generic;
using System.Linq;
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
            string check = IsExistDroneCharge(newDroneCharge);
            if (check == "not exists")
                DataSource.DroneCharges.Add(newDroneCharge);
            if (check == "exists")
                throw new IdExistException("ERROR: the drone charge is exist");
            if (check == "was exists")
                throw new IdExistException("ERROR: the drone charge was exist");
        }

        public void RemoveDroneCharge(DroneCharge DroneCharge)
        {
            string check = IsExistDroneCharge(DroneCharge);
            if (check == "not exists")
                throw new IdNotFoundException("ERROR: the drone charge is not found!\n");
            if (check == "exists")
            {
                for (int i = 0; i < DataSource.DroneCharges.Count(); i++)
                {
                    DroneCharge elementDroneCharge = DataSource.DroneCharges[i];
                    if (elementDroneCharge.DroneId == DroneCharge.DroneId && elementDroneCharge.StationId == DroneCharge.StationId)
                    {
                        elementDroneCharge.Deleted = true;
                        DataSource.DroneCharges[i] = elementDroneCharge;
                    }

                }

            }   
        }

        /// <summary>
        /// return all the list of the drone's that they are in charge sopt 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DroneCharge> GetDronesCharge(Predicate<DroneCharge> droneChargePredicate)
        {
            IEnumerable<DroneCharge> dronesCharge = DataSource.DroneCharges.Where(dronesCharge => droneChargePredicate(dronesCharge));
            return dronesCharge;
        }

        private string IsExistDroneCharge(DroneCharge droneCharge)
        {
            foreach (DroneCharge elementDroneCharge in DataSource.DroneCharges)
            {
                if (elementDroneCharge.DroneId == droneCharge.DroneId &&
                    elementDroneCharge.StationId == droneCharge.StationId &&
                    elementDroneCharge.Deleted == false)
                    return "exists";
                if (elementDroneCharge.DroneId == droneCharge.DroneId &&
                   elementDroneCharge.StationId == droneCharge.StationId &&
                   elementDroneCharge.Deleted == true)
                    return "was exists";
            }
                
            return "not exists";//the drone charge not exist
        }
    }
}
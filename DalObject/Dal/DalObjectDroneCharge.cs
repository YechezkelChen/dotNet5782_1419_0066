using System;
using System.Collections.Generic;
using System.Linq;
using DO;
using System.Runtime.CompilerServices;


namespace Dal
{
    partial class DalObject : DalApi.IDal
    {
        /// <summary>
        /// add a drone charge to the drone charge list
        /// </summary>
        /// <param Name="newDroneCharge"></the new drone charge the user whants to add to the drone's list>
       
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddDroneCharge(DroneCharge newDroneCharge)
        {
            string check = IsExistDroneCharge(newDroneCharge);
            if (check == "not exists" || check == "was exists")
                DataSource.DroneCharges.Add(newDroneCharge);
            if (check == "exists")
                throw new IdExistException("ERROR: the drone charge is exist");
        }

        /// <summary>
        /// remove a drone charge from the drone charge list
        /// </summary>
        /// <param Name="newDroneCharge"></the new drone charge the user whants to add to the drone's list>
       
        [MethodImpl(MethodImplOptions.Synchronized)]
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
      
        [MethodImpl(MethodImplOptions.Synchronized)]
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
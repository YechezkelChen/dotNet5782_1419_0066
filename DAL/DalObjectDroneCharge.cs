using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;
using IDAL;
using DalObject;

namespace DalObject
{
    public partial class DalObject : IDal
    {
        /// <summary>
        /// add a drone charge to the drone list
        /// </summary>
        /// <param Name="newDroneCharge"></the new drone charge the user whants to add to the drone's list>
        public void AddDroneCharge(DroneCharge newDroneCharge)
        {
            if (CheckNotExistDroneCharge(newDroneCharge, DataSource.droneCharges))
                DataSource.droneCharges.Add(newDroneCharge);
            else
                throw new DroneChargeExeption("ERROR: the drone charge is exist!\n");
        }

        public void RemoveDroneCharge(DroneCharge DroneCharge)
        {
            if (!CheckNotExistDroneCharge(DroneCharge, DataSource.droneCharges))
                DataSource.droneCharges.Remove(DroneCharge);
            else
                throw new DroneChargeExeption("ERROR: the drone charge is not exist!\n");
        }


        /// <summary>
        /// return all the list of the drone's that they are in charge sopt 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DroneCharge> GetDronesCharge()
        {
            return DataSource.droneCharges;
        }

        public bool CheckNotExistDroneCharge(DroneCharge droneCharge, IEnumerable<DroneCharge> droneCharges)
        {
            foreach (DroneCharge elementDroneCharge in droneCharges)
                if (elementDroneCharge.DdroneId == droneCharge.DdroneId && elementDroneCharge.Stationld == droneCharge.Stationld) 
                    return false;
            return true;//the drone not exist
        }
    }
}
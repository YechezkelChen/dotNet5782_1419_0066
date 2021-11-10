using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
using IDAL;


namespace IBL
{
    public partial class BL : IBL
    {
        public void AddDrone(Drone newDrone, int idStation)
        {
            try
            {
                CheckDrone(newDrone);
            }
            catch (DroneException e)
            {
                throw new DroneException("" + e);
            }
            IDAL.DO.Drone drone = new IDAL.DO.Drone();
            DroneToList newDroneToList = new DroneToList();

            drone.Id = newDrone.Id;
            drone.Model = newDrone.Model;
            drone.Weight = (IDAL.DO.WeightCategories)newDrone.Weight;
            newDrone.Battery = rand.Next(20, 40);
            newDrone.Status = DroneStatuses.Maintenance;
            try
            {
                newDrone.Location.Longitude = dal.GetStation(idStation).Longitude;
                newDrone.Location.Latitude = dal.GetStation(idStation).Latitude;
            }
            catch (DalObject.StationExeption e)
            {
                throw new DroneException("Try again you make a " + e);
            }

            newDroneToList.Id = newDrone.Id;
            newDroneToList.Model = newDrone.Model;
            newDroneToList.Weight = newDrone.Weight;
            newDroneToList.Battery = rand.Next(20, 40);
            newDroneToList.Status = DroneStatuses.Maintenance;
            try
            {
                newDroneToList.Location.Longitude = dal.GetStation(idStation).Longitude;
                newDroneToList.Location.Latitude = dal.GetStation(idStation).Latitude;
            }
            catch (DalObject.StationExeption e)
            {
                throw new DroneException("Try again you make a " + e);
            }

            try
            {
                int foundDrone = CheckDroneAndParcel(newDroneToList.Id, dal.GetParcels());
                newDroneToList.IdParcel = foundDrone;
            }
            catch (DroneException e)
            {
                throw new DroneException("" + e);
            }

            ListDrones.Add(newDroneToList);
            dal.AddDrone(drone);
        }

        public Drone GetDrone(int id)
        {
            IDAL.DO.Station idalStation = new IDAL.DO.Station();
            try
            {
                idalStation = dal.GetStation(id);
            }
            catch (DalObject.StationExeption e)
            {
                throw new StationException("" + e);
            }

            Station station = new Station();
            station.Id = idalStation.Id;
            station.Name = idalStation.Name;
            station.Location.Longitude = idalStation.Longitude;
            station.Location.Latitude = idalStation.Latitude;
            station.ChargeSlots = idalStation.ChargeSlots;
            foreach (var elementDroneCharge in dal.GetDronesCharge())
                station.InCharges.Add(elementDroneCharge);

            return station;
        }

        public void PrintDrones()
        {
            foreach (IDAL.DO.Drone elementDrone in dal.GetDrones())
                Console.WriteLine(elementDrone.ToString());
        }

        public void UpdateDrone(int droneId, string newModel)
        {
            try
            {
                if (dal.CheckNotExistDrone(dal.GetDrone(droneId), dal.GetDrones()))
                    throw new DroneException("ERROR: the drone not exist:");

            }
            catch (DroneException e)
            {
                throw new DroneException("" + e);
            }

            dal.UpdateDroneModel(droneId, newModel);
        }

        public void CheckDrone(Drone drone)
        {
            if (drone.Id < 0)
                throw new DroneException("ERROR: the ID is illegal! ");
        }

        /// <summary>
        /// check if the drone have parcel for the DroneToList in bl
        /// </summary>
        /// <param name="droneId"></the drone we search for>
        /// <param name="parcels"></all the parcels in dal>
        /// <returns></return the parcel id if the drone coneccted to some parcel else -1 (not conected)>
        public int CheckDroneAndParcel(int droneId, IEnumerable<IDAL.DO.Parcel> parcels)
        {
            foreach (IDAL.DO.Parcel elementParcel in parcels)
                if (elementParcel.DroneId == droneId)
                    return elementParcel.Id;
            throw new DroneException("ERROR: the drone not exist! ");
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
using IDAL;

namespace IBL
{
    public partial class BL : IBL
    {
        public IEnumerable<IDAL.DO.Drone> listDrones = new List<IDAL.DO.Drone>();
        public IEnumerable<IDAL.DO.Parcel> ListParcels = new List<IDAL.DO.Parcel>();

        void UpdateDrone(BO.Drone drone)
        {
            drone.Status = DroneStatuses.Delivery;
        }

        public BL()
        {
            IDal dal = new DalObject.DalObject();

            double[] powerConsumption = dal.GetRequestPowerConsumption();
            double dAvailable = powerConsumption[0];
            double dLightW = powerConsumption[1];
            double dMediumW = powerConsumption[2];
            double dHeavyW = powerConsumption[3];
            double chargingRateOfDrone = powerConsumption[4]; //Percent per hour

            listDrones = dal.GetDrones();
            ListParcels = dal.GetParcels();
            var r = from d in listDrones
                from p in ListParcels
                where d.Id == p.DroneId && p.PickedUp == DateTime.MinValue
                select new BO.Drone()
                {
                    Id = d.Id,
                    Model = d.Model,
                    Weight = Enum.Parse<WeightCategories>(d.Weight.ToString())
                };

            r.ToList().ForEach(UpdateDrone);








        }
    }
}
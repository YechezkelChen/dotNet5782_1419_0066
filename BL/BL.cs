using System;
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
        public IEnumerable<BO.Drone> listBoDrones = new List<BO.Drone>();

        void UpdateDrone(BO.Drone drone)
        {
            drone.status = DroneStatuses.Delivery;
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
                where d.Id == p.droneId && p.pickedUp == DateTime.MinValue
                select new BO.Drone()
                {
                    id = d.Id,
                    model = d.model,
                    weight = Enum.Parse<WeightCategories>(d.maxWeight.ToString())
                };

            r.ToList().ForEach(UpdateDrone);








        }
    }
}

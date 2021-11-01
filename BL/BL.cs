using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
using IDAL;

namespace BL
{
    public partial class BL : IBL
    {
        public IEnumerable<IDAL.DO.Drone> listDrones = new List<IDAL.DO.Drone>();
        public IEnumerable<IDAL.DO.Parcel> ListParcels = new List<IDAL.DO.Parcel>();
        public IEnumerable<IBL.BO.Drone> listBoDrones = new List<IBL.BO.Drone>();

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
            //צריך לראות איך אפשר להעתיק את השדות הרלוונטים לרשימה של רחפנים מסוג בו מרחפנים מסוג דו ואז נוכל לבצע השוואה נורמלית ולעדכן את הסטטוס בהתתאם
            foreach (IDAL.DO.Drone elementDrone in listDrones)
            {
                foreach (IDAL.DO.Parcel elmentParcel in ListParcels)
                {
                    if (elmentParcel.pickedUp == DateTime.MinValue && elmentParcel.droneId == elementDrone.id)//ther are parcel that not pickup but drone that connected to the parcel
                        
                }

            }
            
            




        }
    }
}

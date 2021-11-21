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
        public List<DroneToList> ListDrones = new List<DroneToList>();

        private IDal dal;

        double BatteryAvailable, BatteryLightWeight, BatteryMediumWeight, BatteryHeavyWeight, ChargingRateOfDrone;

        Random rand = new Random(DateTime.Now.Millisecond);

        /// <summary>
        /// constractur of the BL
        /// </summary>
        /// <returns></no returns >
        public BL()
        {
            dal = new DalObject.DalObject();

            // km per hour
            double[] powerConsumption = dal.GetRequestPowerConsumption();
            BatteryAvailable = powerConsumption[0];
            BatteryLightWeight = powerConsumption[1];
            BatteryMediumWeight = powerConsumption[2];
            BatteryHeavyWeight = powerConsumption[3];
            ChargingRateOfDrone = powerConsumption[4];

            IEnumerable<IDAL.DO.Drone> listDronesIdalDo = dal.GetDrones();
            foreach (var elementDrone in listDronesIdalDo)
            {
                DroneToList newDrone = new DroneToList();
                newDrone.Id = elementDrone.Id;
                newDrone.Model = elementDrone.Model;
                newDrone.Weight = Enum.Parse<WeightCategories>(elementDrone.Weight.ToString());
                ListDrones.Add(newDrone);
            }

            IEnumerable<IDAL.DO.Parcel> ListParcelsIdalDo = dal.GetParcels();

            for (int i = 0; i < ListDrones.Count(); i++)
            {
                DroneToList newDroneToList = new DroneToList();
                newDroneToList = ListDrones[i];
                foreach (var elementParcel in ListParcelsIdalDo)
                {
                    if (newDroneToList.Id == elementParcel.DroneId &&
                        elementParcel.Scheduled != DateTime.MinValue &&
                        elementParcel.Delivered == DateTime.MinValue) //if the parcel are not in deliver
                        //and have drone to connect
                    {
                        newDroneToList.Status = DroneStatuses.Delivery;
                        if (elementParcel.Scheduled != DateTime.MinValue &&
                            elementParcel.PickedUp == DateTime.MinValue) //we write the the bool condition
                            //elementParcel.Scheduled != DateTime.MinValue for the understanding of the code
                            newDroneToList.Location =
                                NearStationToCustomer(dal.GetCustomer(elementParcel.SenderId)).Location;
                        if (elementParcel.Delivered == DateTime.MinValue &&
                            elementParcel.PickedUp != DateTime.MinValue)
                            newDroneToList.Location = new Location()
                            {
                                Longitude = dal.GetCustomer(elementParcel.SenderId).Longitude,
                                Latitude = dal.GetCustomer(elementParcel.SenderId).Latitude
                            }; //the location of the customer

                        Location targetLocation = new Location()
                        {
                            Longitude = dal.GetCustomer(elementParcel.TargetId).Longitude,
                            Latitude = dal.GetCustomer(elementParcel.TargetId).Latitude
                        };

                        double distanceDelivery =
                            Distance(newDroneToList.Location,
                                targetLocation); // the distance between the drone and the target
                        if (elementParcel.Weight == IDAL.DO.WeightCategories.Heavy)
                            distanceDelivery *= BatteryHeavyWeight;
                        if (elementParcel.Weight == IDAL.DO.WeightCategories.Medium)
                            distanceDelivery *= BatteryMediumWeight;
                        if (elementParcel.Weight == IDAL.DO.WeightCategories.Light)
                            distanceDelivery *= BatteryLightWeight;

                        distanceDelivery += Distance(targetLocation,
                                                NearStationToCustomer(dal.GetCustomer(elementParcel.TargetId))
                                                    .Location) *
                                            BatteryAvailable;

                        newDroneToList.Battery = (100 - distanceDelivery) * rand.NextDouble() + distanceDelivery;
                        if (newDroneToList.Battery > 100)
                            newDroneToList.Battery = 100;

                        newDroneToList.IdParcel = elementParcel.Id;
                    }
                }

                if ((newDroneToList.Status != DroneStatuses.Delivery))
                    newDroneToList.Status = (DroneStatuses) rand.Next(0, 2);

                if (newDroneToList.Status == DroneStatuses.Maintenance)
                {
                    newDroneToList.Status =
                        DroneStatuses.Available; // for the charge after he will be in Maintenance.
                    IEnumerable<IDAL.DO.Station> listStationsIdalDo = dal.GetStations();
                    int index = rand.Next(0, listStationsIdalDo.Count());
                    newDroneToList.Location = new Location()
                    {
                        Longitude = listStationsIdalDo.ElementAt(index).Longitude,
                        Latitude = listStationsIdalDo.ElementAt(index).Latitude
                    };

                    newDroneToList.Battery = 20 * rand.NextDouble();
                    newDroneToList.IdParcel = 0;
                    SendDroneToDroneCharge(newDroneToList.Id);
                }

                if (newDroneToList.Status == DroneStatuses.Available)
                {
                    IEnumerable<IDAL.DO.Customer> customersWithDelivery =
                        ListCustomersWithDelivery(dal.GetCustomers(), dal.GetParcels());
                    if (customersWithDelivery.Count() == 0)
                        newDroneToList.Location = new Location() {Longitude = 0, Latitude = 0};
                    else
                    {
                        int index = rand.Next(0, customersWithDelivery.Count());
                        newDroneToList.Location = new Location()
                        {
                            Longitude = customersWithDelivery.ElementAt(index).Longitude,
                            Latitude = customersWithDelivery.ElementAt(index).Latitude
                        };
                    }

                    double distanceFromNearStation = Distance(newDroneToList.Location,
                        NearStationToDrone(dal.GetDrone(newDroneToList.Id)).Location);

                    distanceFromNearStation *= BatteryAvailable;

                    newDroneToList.Battery = (100 - distanceFromNearStation) * rand.NextDouble() +
                                             distanceFromNearStation;
                    if (newDroneToList.Battery > 100)
                        newDroneToList.Battery = 100;
                    newDroneToList.IdParcel = 0;
                }

                ListDrones[i] = newDroneToList;
            }
        }

        /// <summary>
        /// the distances from "FROM" to "TO"
        /// </summary>
        /// <returns></returns the distance on double type>
        private double Distance(Location from, Location to)
        {
            int R = 6371 * 1000; // metres -- radius of the earth
            double phi1 = from.Latitude * Math.PI / 180; // φ, λ in radians
            double phi2 = to.Latitude * Math.PI / 180;
            double deltaPhi = (to.Latitude - from.Latitude) * Math.PI / 180;
            double deltaLambda = (to.Longitude - from.Longitude) * Math.PI / 180;

            double a = Math.Sin(deltaPhi / 2) * Math.Sin(deltaPhi / 2) +
                       Math.Cos(phi1) * Math.Cos(phi2) *
                       Math.Sin(deltaLambda / 2) * Math.Sin(deltaLambda / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double d = R * c / 1000; // in kilometres
            return d;
        }
    }
}

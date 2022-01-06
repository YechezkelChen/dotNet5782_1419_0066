using System;
using System.Collections.Generic;
using BO;
using System.Threading;
using static BL.BL;
using System.Linq;


namespace BL
{
    class Simulator
    {
        private const int DELAY = 500; // milliseconds
        private const double SPEED = 40; // km per hour
        private Drone drone;
        private Location location;
        private double timeDrive;

        public Simulator(BlApi.IBL bl, int droneId, Action action, Func<bool> stopSimulatorMod)
        {
            while (!stopSimulatorMod())
            {
                lock (bl)
                {
                    drone = bl.GetDrone(droneId);
                }
                Thread.Sleep(DELAY);
                if (drone.Status == DroneStatuses.Available)
                {
                    try
                    {
                        lock (bl)
                        {
                            bl.ConnectParcelToDrone(drone.Id);
                        }
                    }
                    catch (NoPackagesToDroneException)
                    {
                        IEnumerable<ParcelToList> parcels;
                        lock (bl)
                        {
                            parcels = bl.GetParcelsNoDrones();
                        }
                        if (parcels.Any()) // if there is no parcels in requested.
                            Thread.Sleep(DELAY);
                        else // if there is no battery to drone to take parcels.
                        {
                            location = drone.Location; // the place before the charge
                            lock (bl)
                            {
                                bl.SendDroneToDroneCharge(drone.Id);
                                timeDrive = bl.Distance(location, drone.Location) / SPEED;
                            }

                            Thread.Sleep(Convert.ToInt32(timeDrive) * 1000); // the place after the charge
                        }
                    }
                }
                Thread.Sleep(DELAY);

                if (drone.Status == DroneStatuses.Maintenance)
                {
                    lock (bl)
                    {
                        bl.ReleaseDroneFromDroneCharge(drone.Id); // release, to check how much battery there is to drone
                        if (drone.Battery != 100) // if there is no to drone full battery.
                            bl.SendDroneToDroneCharge(drone.Id);
                    }
                }
                Thread.Sleep(DELAY);

                if (drone.Status == DroneStatuses.Delivery)
                {
                    if (drone.ParcelByTransfer.OnTheWay == false) // if the parcel just connect, don't collect
                    {
                        lock (bl)
                        {
                            bl.CollectionParcelByDrone(drone.Id);
                            timeDrive = drone.ParcelByTransfer.DistanceOfTransfer / SPEED;
                        }

                        Thread.Sleep(Convert.ToInt32(timeDrive) * 1000);
                    }
                    else // if the parcel collect
                    {
                        lock (bl)
                        {
                            bl.SupplyParcelByDrone(drone.Id);
                            timeDrive = drone.ParcelByTransfer.DistanceOfTransfer / SPEED;
                        }

                        Thread.Sleep(Convert.ToInt32(timeDrive) * 1000);
                    }
                }
                Thread.Sleep(DELAY);
            }
        }
    }
}



using System;
using System.Collections.Generic;
using BO;
using System.Threading;
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

        public Simulator(BlApi.IBL bl, int droneId, Action updateView, Func<bool> stopSimulator)
        {
            while (!stopSimulator())
            {
                lock (bl)
                {
                    drone = bl.GetDrone(droneId);
                }

                switch (drone.Status)
                {
                    case DroneStatuses.Available:
                    {
                        try
                        {
                            lock (bl)
                            {
                                bl.ConnectParcelToDrone(drone.Id);
                            }
                        }
                        catch (NoParcelsToDroneException)
                        {
                            IEnumerable<ParcelToList> parcels;
                            lock (bl)
                            {
                                parcels = bl.GetParcelsNoDrones();
                            }

                            if (parcels.Any()) // if there is no parcels in requested.
                            {
                                throw new NoParcelsToDroneException("No more parcels that can be assigned to the drone, please click the button: 'Regular'");
                            }

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
                        catch (StatusDroneException) { }

                        break;
                    }

                    case DroneStatuses.Maintenance:
                    {
                        lock (bl)
                        {
                            try
                            {
                                bl.ReleaseDroneFromDroneCharge(drone.Id); // release, to check how much battery there is to drone
                                drone = bl.GetDrone(droneId);
                                if (drone.Battery != 100) // if there is no to drone full battery.
                                    bl.SendDroneToDroneCharge(drone.Id);
                            }
                            catch (StatusDroneException) { }
                        }
                        break;
                    }

                    case DroneStatuses.Delivery:
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
                        break;
                    }
                }

                Thread.Sleep(DELAY);
                updateView();
            }
        }
    }
}



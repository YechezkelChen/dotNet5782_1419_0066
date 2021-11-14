using System;
using System.Collections.Generic;
using IBL;
using IBL.BO;
using IDAL.DO;
using Customer = IBL.BO.Customer;
using Drone = IBL.BO.Drone;
using Parcel = IBL.BO.Parcel;
using Station = IBL.BO.Station;

namespace ConsoleUI_BL
{
    class Program
    {
        public enum Option
        {
            Add = 1,
            Update,
            View,
            ListView,
            Exit
        };

        public enum EntityOption
        {
            Station = 1,
            Drone,
            Customer,
            Parcel,
            Exit
        };

        public enum OptionListView
        {
            ListStations = 1,
            ListDrones,
            ListCustomers,
            ListParcels,
            ListParcelsNoDrones,
            ListStationsCharge,
            Exit
        };

        public enum OptionUpdate
        {
            ModelDrone = 1,
            DataStation,
            DataCustomer,
            SendDroneToDroneCharge,
            ReleaseDroneFromDroneCharge,
            ConnectParcelToDrone,
            CollectionParcelByDrone,
            SupplyParcelByDrone,
            Exit
        };

        public static IBL.IBL bl = new BL();

        static void Main(string[] args)
        {
            int c, myId;
            Option op;
            EntityOption ep;
            OptionListView olv;
            OptionUpdate ou;
            do
            {
                do
                {
                    Console.WriteLine("\nHELLO\n" + "Choose one of the following:\n" + "1: Add\n" + "2: Update\n" +
                                      "3: View\n" + "4: List View\n" + "5: Exit\n");
                } while (!int.TryParse(Console.ReadLine(), out c));

                op = (Option) c;
                switch (op)
                {
                    case Option.Add:
                        do
                        {
                            Console.WriteLine("Choose one of the entity:\n" + "1: Station\n" + "2: Drone\n" +
                                              "3: Customer\n" + "4: Parcel\n" + "5: Exit\n");
                        } while (!int.TryParse(Console.ReadLine(), out c));

                        ep = (EntityOption) c;
                        switch (ep)
                        {
                            case EntityOption.Station:
                                AddStation();
                                break;
                            case EntityOption.Drone:
                                AddDrone();
                                break;
                            case EntityOption.Customer:
                                AddCustomer();
                                break;
                            case EntityOption.Parcel:
                                AddParcel();
                                break;
                            case EntityOption.Exit:
                                break;
                            default:
                                break;
                        }

                        break;
                    case Option.Update:
                        do
                        {
                            Console.WriteLine("Choose one of to update:\n" + "1: ModelDrone:\n" +
                                              "2: DataStation\n" + "3: DataCustomer\n" +
                                              "4: Send Drone To Drone Charge\n" +
                                              "5: Release Drone From Drone Charge\n" + "6: ConnectParcelToDrone\n" +
                                              "7: CollectionParcelByDrone\n" + "8: SupplyParcelByDrone\n" +
                                              "9: Exit\n");
                        } while (!int.TryParse(Console.ReadLine(), out c));

                        ou = (OptionUpdate) c;
                        switch (ou)
                        {
                            case OptionUpdate.ModelDrone:
                                UpdateDroneModel();
                                break;
                            case OptionUpdate.DataStation:
                                UpdateDataStation();
                                break;
                            case OptionUpdate.DataCustomer:
                                UpdateDataCustomer();
                                break;
                            case OptionUpdate.SendDroneToDroneCharge:
                                SendDroneToCharge();
                                break;
                            case OptionUpdate.ReleaseDroneFromDroneCharge:
                                ReleaseDroneFromCharge();
                                break;
                            case OptionUpdate.ConnectParcelToDrone:
                                ConnectParcelToDrone();
                                break;
                            case OptionUpdate.CollectionParcelByDrone:
                                CollectionParcelByDrone();
                                break;
                            case OptionUpdate.SupplyParcelByDrone:
                                SupplyParcelByDrone();
                                break;
                            case OptionUpdate.Exit:
                                break;
                            default:
                                break;
                        }

                        break;
                    case Option.View:
                        do
                        {
                            Console.WriteLine("Choose one of the entity:\n" + "1: Station\n" + "2: Drone\n" +
                                              "3: Customer\n" + "4: Parcel\n" + "5: Exit\n");
                        } while (!int.TryParse(Console.ReadLine(), out c));

                        ep = (EntityOption) c;
                        do
                        {
                            Console.WriteLine("Enter Id of the entity:\n");
                        } while (!int.TryParse(Console.ReadLine(), out myId));

                        switch (ep)
                        {
                            case EntityOption.Station:
                                try
                                {
                                    Console.WriteLine(bl.GetStation(myId));

                                }
                                catch (StationException e)
                                {
                                    Console.WriteLine(e);
                                }
                                break;
                            case EntityOption.Drone:
                                try
                                {
                                    Console.WriteLine(bl.GetDrone(myId));
                                }
                                catch (DroneException e)
                                {
                                    Console.WriteLine(e);
                                }
                                break;
                            case EntityOption.Customer:
                                try
                                {
                                    Console.WriteLine(bl.GetCustomer(myId));
                                }
                                catch (CustomerException e)
                                {
                                    Console.WriteLine(e);

                                }
                                break;
                            case EntityOption.Parcel:
                                try
                                {
                                    Console.WriteLine(bl.GetParcel(myId));
                                }
                                catch (ParcelException e)
                                {
                                    Console.WriteLine(e);
                                }
                                break;
                            case EntityOption.Exit:
                                break;
                            default:
                                break;
                        }

                        break;
                    case Option.ListView:
                        do
                        {
                            Console.WriteLine("Choose one of the entity:\n" + "1: List Stations\n" +
                                              "2: List Drones\n" + "3: List Customers\n" + "4: List Parcels\n" +
                                              "5: List Parcels No Drones\n" + "6: List Stations Charge\n" +
                                              "7:Exit\n");
                        } while (!int.TryParse(Console.ReadLine(), out c));

                        olv = (OptionListView) c;
                        switch (olv)
                        {
                            case OptionListView.ListStations:
                                Console.WriteLine(bl.GetStations());
                                break;
                            case OptionListView.ListDrones:
                                Console.WriteLine(bl.GetDrones());
                                break;
                            case OptionListView.ListCustomers:
                                Console.WriteLine(bl.GetCustomers());
                                break;
                            case OptionListView.ListParcels:
                                Console.WriteLine(bl.GetParcels());
                                break;
                            case OptionListView.ListParcelsNoDrones:
                                Console.WriteLine(bl.GetParcelsNoDrones());
                                break;
                            case OptionListView.ListStationsCharge:
                                Console.WriteLine(bl.GetStationsCharge());
                                break;
                            case OptionListView.Exit:
                                break;
                            default:
                                break;
                        }

                        break;
                    case Option.Exit:
                        break;
                    default:
                        break;
                }
            } 
            while (op != Option.Exit);
        }

        
        /// <summary>
        /// read from the user station to insert to list
        /// </summary>
        /// <returns></no returns, just read from user>
        public static void AddStation()
        {
            int num1;
            double num2;
            Location stationLocation = new Location();
            Station newStation = new Station();
            do
            {
                Console.WriteLine("Enter Id Station: ");
            } while (!int.TryParse(Console.ReadLine(), out num1));

            newStation.Id = num1;

            do
            {
                Console.WriteLine("Enter Name Station: ");

            } while (!int.TryParse(Console.ReadLine(), out num1));

            newStation.Name = num1;

            do
            {
                Console.WriteLine("Enter longitude Station: ");

            } while (!double.TryParse(Console.ReadLine(), out num2));

            stationLocation.Longitude = num2;

            do
            {
                Console.WriteLine("Enter Latitude Station: ");

            } while (!double.TryParse(Console.ReadLine(), out num2));

            stationLocation.Latitude = num2;
            newStation.Location = stationLocation;

            do
            {
                Console.WriteLine("Enter Available Charge Slots  Station: ");

            } while (!int.TryParse(Console.ReadLine(), out num1));

            newStation.ChargeSlots = num1;

            newStation.InCharges = new List<DroneCharge>();

            try
            {
                bl.AddStation(newStation);
                Console.WriteLine("Success! :)\n");
            }
            catch (StationException e)
            {
                Console.WriteLine(e);
            }
        }

        /// <summary>
        /// read fron the user drone to insert to list
        /// </summary>
        /// <returns></no returns, just read from user>
        public static void AddDrone()
        {
            int num;
            Drone newDrone = new Drone();

            do
            {
                Console.WriteLine("Enter Id Drone: ");
            } while (!int.TryParse(Console.ReadLine(), out num));

            newDrone.Id = num;

            Console.WriteLine("Enter Model Drone: ");
            newDrone.Model = Console.ReadLine();

            do
            {
                Console.WriteLine("Enter Weight Drone:\n" + "1: Light\n" + "2: Medium\n" + "3: Heavy\n");
                int.TryParse(Console.ReadLine(), out num);
            } while (num != 1 && num != 2 && num != 3);

            switch (num)
            {
                case 1:
                    newDrone.Weight = IBL.BO.WeightCategories.Light;
                    break;
                case 2:
                    newDrone.Weight = IBL.BO.WeightCategories.Medium;
                    break;
                case 3:
                    newDrone.Weight = IBL.BO.WeightCategories.Heavy;
                    break;
                default:
                    break;
            }

            do
            {
                Console.WriteLine("Enter id of station to put the drone in: ");
            } while (!int.TryParse(Console.ReadLine(), out num));

            try
            {
                bl.AddDrone(newDrone, num);
                Console.WriteLine("Success! :)\n");
            }
            catch (DroneException e)
            {
                Console.WriteLine(e);
            }
        }

        /// <summary>
        /// read fron the user customer to insert to list
        /// </summary>
        /// <returns></no returns, just read from user>
        public static void AddCustomer()
        {
            int num1;
            double num2;
            Location customerLocation = new Location();
            Customer newCustomer = new Customer();
            do
            {
                Console.WriteLine("Enter Id Customer: ");
            } while (!int.TryParse(Console.ReadLine(), out num1));

            newCustomer.Id = num1;

            Console.WriteLine("Enter Name Customer: ");
            newCustomer.Name = Console.ReadLine();

            Console.WriteLine("Enter Phone Customer: ");
            newCustomer.Phone = Console.ReadLine();

            do
            {
                Console.WriteLine("Enter Longitude Customer: ");
            } while (!double.TryParse(Console.ReadLine(), out num2));

            customerLocation.Longitude = num2;

            do
            {
                Console.WriteLine("Enter Latitude Customer: ");
            } while (!double.TryParse(Console.ReadLine(), out num2));

            customerLocation.Latitude = num2;
            newCustomer.Location = customerLocation;

            try
            {
                bl.AddCustomer(newCustomer);
                Console.WriteLine("Success! :)\n");
            }
            catch (CustomerException e)
            {
                Console.WriteLine(e);
            }
        }

        /// <summary>
        /// read from the user parcel to insert to list
        /// </summary>
        /// <returns></no returns, just read from user>
        public static void AddParcel()
        {
            int num;
            Parcel newParcel = new Parcel();
            do
            {
                Console.WriteLine("Enter Sender Id Parcel: ");
            } while (!int.TryParse(Console.ReadLine(), out num));

            newParcel.Sender.Id = num;

            do
            {
                Console.WriteLine("Enter Target Id Parcel: ");
            } while (!int.TryParse(Console.ReadLine(), out num));

            newParcel.Target.Id = num;

            do
            {
                Console.WriteLine("Enter Weight Parcel:\n" + "1: Light\n" + "2: Medium\n" + "3: Heavy\n");
                int.TryParse(Console.ReadLine(), out num);
            } while (num != 1 && num != 2 && num != 3);

            switch (num)
            {
                case 1:
                    newParcel.Weight = IBL.BO.WeightCategories.Light;
                    break;
                case 2:
                    newParcel.Weight = IBL.BO.WeightCategories.Medium;
                    break;
                case 3:
                    newParcel.Weight = IBL.BO.WeightCategories.Heavy;
                    break;
                default:
                    break;
            }

            do
            {

                Console.WriteLine("Enter Priority Parcel:\n" + "1: Normal\n" + "2: Fast\n" + "3: Emergency\n");
                int.TryParse(Console.ReadLine(), out num);
            } while (num != 1 && num != 2 && num != 3);

            switch (num)
            {
                case 1:
                    newParcel.Priority = IBL.BO.Priorities.Normal;
                    break;
                case 2:
                    newParcel.Priority = IBL.BO.Priorities.Fast;
                    break;
                case 3:
                    newParcel.Priority = IBL.BO.Priorities.Emergency;
                    break;
                default:
                    break;
            }

            try
            {
                bl.AddParcel(newParcel);
                Console.WriteLine("Success! :)\n");
            }
            catch (ParcelException e)
            {
                Console.WriteLine(e);
            }
        }

        public static void UpdateDroneModel()
        {
            int id;
            string model;
            do
            {
                Console.WriteLine("Enter Id Drone: ");
            } while (!int.TryParse(Console.ReadLine(), out id));

            Console.WriteLine("Enter Model Drone: ");
            model = Console.ReadLine();

            try
            {
                bl.UpdateDroneModel(id, model);
                Console.WriteLine("Success! :)\n");
            }
            catch (DroneException e)
            {
                Console.WriteLine(e);
            }
        }

        public static void UpdateDataStation()
        {
            int id, name, chargeSlots;
            do
            {
                Console.WriteLine("Enter Id Station: ");
            } while (!int.TryParse(Console.ReadLine(), out id));

            Console.WriteLine("Note! Enter at least one of the following data:\n");
            do
            {
                Console.WriteLine("If you want to update the name of the station, Enter name:\n if you don't want, Enter -1: \n");
            } while (!int.TryParse(Console.ReadLine(), out name));

            do
            {
                Console.WriteLine("If you want to update the number of charge Slots in the station, Enter the amount:\n if you don't want, Enter -1: \n");
            } while (!int.TryParse(Console.ReadLine(), out chargeSlots));

            try
            {
                bl.UpdateDataStation(id, name, chargeSlots);
                Console.WriteLine("Success! :)\n");
            }
            catch (StationException e)
            {
                Console.WriteLine(e);
            }
        }

        public static void UpdateDataCustomer()
        {
            int id;
            string name, phone;
            do
            {
                Console.WriteLine("Enter Id Customer: ");
            } while (!int.TryParse(Console.ReadLine(), out id));

            Console.WriteLine("Note! Enter at least one of the following data:\n");

            Console.WriteLine("If you want to update the name of the customer, Enter name:\n if you don't want, press Enter: \n");
            name = Console.ReadLine();

            Console.WriteLine("If you want to update the phone of the customer, Enter phone:\n if you don't want, press Enter: \n");
            phone = Console.ReadLine();

            try
            {
                bl.UpdateDataCustomer(id, name, phone);
                Console.WriteLine("Success! :)\n");
            }
            catch (CustomerException e)
            {
                Console.WriteLine(e);
            }
        }

        public static void SendDroneToCharge()
        {
            int id;
            do
            {
                Console.WriteLine("Enter Id Drone: ");
            } while (!int.TryParse(Console.ReadLine(), out id));

            try
            {
                bl.SendDroneToDroneCharge(id);
                Console.WriteLine("Success! :)\n");
            }
            catch (DroneException e)
            {
                Console.WriteLine(e);
            }
        }
        
        public static void ReleaseDroneFromCharge()
        {
            int id, chargeTime;
            do
            {
                Console.WriteLine("Enter Id Drone: ");
            } while (!int.TryParse(Console.ReadLine(), out id));

            do
            {
                Console.WriteLine("Enter the time the drone was in charge:\n ");
            } while (!int.TryParse(Console.ReadLine(), out chargeTime));

            try
            {
                bl.ReleaseDroneFromDroneCharge(id, chargeTime);
                Console.WriteLine("Success! :)\n");
            }
            catch (DroneException e)
            {
                Console.WriteLine(e);
            }
        }

        public static void ConnectParcelToDrone()
        {
            int droneId;
            do
            {
                Console.WriteLine("Enter Id Drone: ");
            } while (!int.TryParse(Console.ReadLine(), out droneId));

            try
            {
                bl.ConnectParcelToDrone(droneId);
                Console.WriteLine("Success! :)\n");
            }
            catch (DroneException e)
            {
                Console.WriteLine(e);
            }
        }

        public static void CollectionParcelByDrone()
        {
            int droneId;
            do
            {
                Console.WriteLine("Enter Id Drone: ");
            } while (!int.TryParse(Console.ReadLine(), out droneId));

            try
            {
                bl.CollectionParcelByDrone(droneId);
                Console.WriteLine("Success! :)\n");
            }
            catch (DroneException e)
            {
                Console.WriteLine(e);
            }
        }

        public static void SupplyParcelByDrone()
        {
            int droneId;
            do
            {
                Console.WriteLine("Enter Id Drone: ");
            } while (!int.TryParse(Console.ReadLine(), out droneId));

            try
            {
                bl.SupplyParcelByDrone(droneId);
                Console.WriteLine("Success! :)\n");
            }
            catch (DroneException e)
            {
                Console.WriteLine(e);
            }
        }
    }
}

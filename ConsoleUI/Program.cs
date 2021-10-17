using System;

namespace ConsoleUI
{
    class Program
    {
        public enum Option { Add, Update, View, ListView, Exit};
        public enum EntityOption { Station, Drone, Customer, Parcel, Exit};

        static void Main(string[] args)
        {
            Option op;
            Console.WriteLine("HELLO\n" + "Choose one of the following:\n" + "a: Add\n" + "u: Update\n" + "v: View\n" + "l: List View\n" + "e: Exit\n");
            int.TryParse(Console.ReadLine(), out op);
            do
	        {
                switch (op)
	            {
		            case Option.Add:
                        EntityOption ep;
                        Console.WriteLine("Choose one of the entity:\n" + "s: Station\n" + "d: Drone\n" + "c: Customer\n" + "p: Parcel\n" + "e: Exit\n");
                        int.TryParse(Console.ReadLine(), out ep);
                        switch (ep)
	                    {
		                    case EntityOption.Station:
                                DalObject.AddStation();
                                break;
                            case EntityOption.Drone:
                                DalObject.AddDrone();
                                break;
                            case EntityOption.Customer:
                                DalObject.AddCustomer();
                                break;
                            case EntityOption.Parcel:
                                DalObject.AddParcel();
                                break;
                            case EntityOption.Exit:
                                break;
                            default:
                                break;
	                    }
                        break;
                    case Option.Update:
                        break;
                    case Option.View:
                        break;
                    case Option.ListView:
                        break;
                    case Option.Exit:
                        break;
                    default:
                        break;
	            }
	        } while (op != Option.Exit);
            
            

            IDAL.DO.Station baseStation = new IDAL.DO.Station();
            Console.WriteLine(Station);
        }
    }
}

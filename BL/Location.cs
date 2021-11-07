using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    { 
        public class Location
        {
            public double Longitude { get; set; }
            public double Latitude { get; set; }
            public override string ToString()
            {
                return $"Longitude = {Longitude}, Latitude = {Latitude}";
            }

            /// <summary>
            /// read fron the user drone to insert to list
            /// </summary>
            /// <returns></no returns, just read from user>
            public static Drone InputDrone()
            {
                int num;
                Drone NewDrone = new Drone();

                do
                {
                    Console.WriteLine("Enter Id Drone: ");
                } while (!int.TryParse(Console.ReadLine(), out num));
                NewDrone.Id = num;

                Console.WriteLine("Enter Model Drone: ");
                NewDrone.Model = Console.ReadLine();

                do
                {
                    Console.WriteLine("Enter Weight Drone:\n" + "1: Light\n" + "2: Medium\n" + "3: Heavy\n");
                    int.TryParse(Console.ReadLine(), out num);
                } while (num != 1 && num != 2 && num != 3);
                switch (num)
                {
                    case 1:
                        NewDrone.Weight = WeightCategories.Light;
                        break;
                    case 2:
                        NewDrone.Weight = WeightCategories.Medium;
                        break;
                    case 3:
                        NewDrone.Weight = WeightCategories.Heavy;
                        break;
                    default:
                        break;
                }

                do
                {
                    Console.WriteLine("Enter num of station to put the drone in: ");
                } while (!int.TryParse(Console.ReadLine(), out num));

                //לראות מה עושים עם המספר הזה
                //להשלים מה שצריך בBL

                return NewDrone;
            }


        }
    }
}

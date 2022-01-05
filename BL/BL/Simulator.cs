using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using static BL.BL;
using BO;

namespace BL
{
    class Simulator
    {
        private Stopwatch stopwatch = new Stopwatch();
        private double chargeRate;
        volatile bool stopSimulator = false;

        Simulator(BlApi.IBL bl, int droneId, Action<Drone> action,
            Func<bool> stopSimulatorMod) //for the simulator mod
        {
            while (!stopSimulator)
            {
               
            }
        }
    }
}



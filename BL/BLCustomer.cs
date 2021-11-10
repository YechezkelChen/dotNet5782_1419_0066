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
        public void AddCustomer(Customer newCustomer)
        {
            try
            {
                CheckCustomer(newCustomer);
            }
            catch (CustomerException e)
            {
                throw new CustomerException("" + e);
            }
            IDAL.DO.Customer customer = new IDAL.DO.Customer();
            customer.Id = newCustomer.Id;
            customer.Name = newCustomer.Name;
            customer.Phone = newCustomer.Phone;
            customer.Longitude = newCustomer.Location.Longitude;
            customer.Latitude = newCustomer.Location.Latitude;
            dal.AddCustomer(customer);
        }

        public Customer GetCustomer(int id)
        {
            IDAL.DO.Customer idalCustomer = new IDAL.DO.Customer();
            try
            {
                idalCustomer = dal.GetCustomer(id);
            }
            catch (DalObject. e)
            {
                throw new StationException("" + e);
            }

            Station station = new Station();
            station.Id = idalStation.Id;
            station.Name = idalStation.Name;
            station.Location.Longitude = idalStation.Longitude;
            station.Location.Latitude = idalStation.Latitude;
            station.ChargeSlots = idalStation.ChargeSlots;
            foreach (var elementDroneCharge in dal.GetDronesCharge())
                station.InCharges.Add(elementDroneCharge);

            return station;
        }

        public void PrintCustomers()
        {
            foreach (IDAL.DO.Customer elementCustomer in dal.GetCustomers())
                Console.WriteLine(elementCustomer.ToString());
        }

        public void CheckCustomer(Customer customer)
        {
            if (customer.Id < 0)
                throw new CustomerException("ERROR: the ID is illegal! ");
        }
    }
}

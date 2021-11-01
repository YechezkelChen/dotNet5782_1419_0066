using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;
using IDAL;
using DalObject;

namespace DalObject
{
    public partial class DalObject : IDal
    {
        /// <summary>
        /// add a customer to the fustomer list
        /// </summary>
        /// <param name="newCustomer"></the new customer the user whants to add to the customer's list>
        public void AddCustomer(Customer newCustomer)
        {
            if (checkNotExistCustomer(newCustomer, DataSource.customers))
                DataSource.customers.Add(newCustomer);
            else
                throw new CustomerExeption("ERROR: the customer is exist!\n");
        }

        /// <summary>
        /// return the spesifice customer the user ask for
        /// </summary>
        /// <param name="customerId"></the Id of the customer the user ask for>
        /// <returns></returns>
        public Customer GetCustomer(int customerId)
        {
            Customer? newCustomer = null;
            foreach (Customer elementCustomer in DataSource.customers)
            {
                if (elementCustomer.id == customerId)
                    newCustomer = elementCustomer;
            }

            if (newCustomer == null)
                throw new CustomerExeption("ERROR: Id of customer not found\n");
            return (Customer)newCustomer;
        }

        /// <summary>
        /// return all the customer list
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Customer> GetCustomers()
        {
            List<Customer> newCustomers = new List<Customer>(DataSource.customers);
            return newCustomers;
        }

        /// <summary>
        /// the methode not need exeption becuse she use both sids(true and false)
        /// </summary>
        /// <param name="d"></the customer we check if she is exist>
        /// <param name="drones"></the list od customers>
        /// <returns></returns>
        public bool checkNotExistCustomer(Customer c, List<Customer> customers)
        {
            foreach (Customer elementCustomer in customers)
                if (elementCustomer.id == c.id)
                    return false;

            return true; //the customer not exist
        }
    }
}
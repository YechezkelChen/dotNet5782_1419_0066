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
        /// <param Name="newCustomer"></the new customer the user whants to add to the customer's list>
        public void AddCustomer(Customer newCustomer)
        {
            if (!IsExistCustomer(newCustomer, DataSource.Customers))
                DataSource.Customers.Add(newCustomer);
            else
                throw new CustomerException("ERROR: the customer is exist!\n");
        }

        /// <summary>
        /// return the spesifice customer the user ask for
        /// </summary>
        /// <param Name="customerId"></the Id of the customer the user ask for>
        /// <returns></returns>
        public Customer GetCustomer(int customerId)
        {
            Customer? newCustomer = null;
            foreach (Customer elementCustomer in DataSource.Customers)
            {
                if (elementCustomer.Id == customerId)
                    newCustomer = elementCustomer;
            }

            if (newCustomer == null)
                throw new CustomerException("ERROR: Id of customer not found\n");
            return (Customer)newCustomer;
        }

        /// <summary>
        /// return all the customer list
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Customer> GetCustomers()
        {
            return DataSource.Customers;
        }

        public void UpdateCustomer(Customer customer)
        {
            for (int i = 0; i < DataSource.Customers.Count(); i++)
                if (DataSource.Customers[i].Id == customer.Id)
                    DataSource.Customers[i] = customer;
        }

        /// <summary>
        /// the methode not need exeption becuse she use both sids(true and false)
        /// </summary>
        /// <param Name="d"></the customer we check if she is exist>
        /// <param Name="Drones"></the list od Customers>
        /// <returns></returns>
        public bool IsExistCustomer(Customer c, IEnumerable<Customer> customers)
        {
            foreach (Customer elementCustomer in customers)
                if (elementCustomer.Id == c.Id)
                    return false;

            return true; //the customer not exist
        }
    }
}
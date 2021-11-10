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
            if (CheckNotExistCustomer(newCustomer, DataSource.customers))
                DataSource.customers.Add(newCustomer);
            else
                throw new CustomerExeption("ERROR: the customer is exist!\n");
        }

        /// <summary>
        /// return the spesifice customer the user ask for
        /// </summary>
        /// <param Name="customerId"></the Id of the customer the user ask for>
        /// <returns></returns>
        public Customer GetCustomer(int customerId)
        {
            Customer? newCustomer = null;
            foreach (Customer elementCustomer in DataSource.customers)
            {
                if (elementCustomer.Id == customerId)
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
            return DataSource.customers;
        }

        /// <summary>
        /// the methode not need exeption becuse she use both sids(true and false)
        /// </summary>
        /// <param Name="d"></the customer we check if she is exist>
        /// <param Name="drones"></the list od customers>
        /// <returns></returns>
        public bool CheckNotExistCustomer(Customer c, IEnumerable<Customer> customers)
        {
            foreach (Customer elementCustomer in customers)
                if (elementCustomer.Id == c.Id)
                    return false;

            return true; //the customer not exist
        }

        public void UpdateDataCustomer(int id, string name, string phone)
        {
            for (int i = 0; i < DataSource.customers.Count(); i++)
            {
                if (DataSource.customers[i].Id == id)
                {
                    if (name != "")
                    {
                        Customer newCustomer = DataSource.customers[i];
                        newCustomer.Name = name;
                        DataSource.customers[i] = newCustomer;
                    }
                    if (phone != "")
                    {
                        Customer newCustomer = DataSource.customers[i];
                        newCustomer.Phone = phone;
                        DataSource.customers[i] = newCustomer;
                    }
                }
            }
        }
    }
}
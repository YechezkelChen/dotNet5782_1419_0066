using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;


namespace Dal
{
    partial class DalObject : DalApi.IDal
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
                throw new IdExistException("ERROR: the customer is exist.");
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
                throw new IdNotFoundException("ERROR: the coustomer not found.");
            return (Customer)newCustomer;
        }

        /// <summary>
        /// return all the customer list
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Customer> GetCustomers(Predicate<Customer> customerPredicate)
        {
            IEnumerable<Customer> customers = DataSource.Customers.FindAll(customerPredicate);
            return customers;
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
        private bool IsExistCustomer(Customer customer, IEnumerable<Customer> customers)
        {
            foreach (Customer elementCustomer in customers)
                if (elementCustomer.Id == customer.Id)
                    return true;

            return false; //the customer not exist
        }
    }
}
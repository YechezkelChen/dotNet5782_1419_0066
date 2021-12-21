using System;
using System.Collections.Generic;
using System.Linq;
using DO;


namespace Dal
{
    partial class DalObject : DalApi.IDal
    {
        /// <summary>
        /// add a customer to the customers list
        /// </summary>
        /// <param Name="newCustomer"></the new customer the user whants to add to the customer's list>
        public void AddCustomer(Customer newCustomer)
        {
            string check = IsExistCustomer(newCustomer.Id);
            if (check == "not exists")
                DataSource.Customers.Add(newCustomer);
            if(check == "exists")
                throw new IdExistException("ERROR: the customer is exist");
            if(check == "was exists")
                throw new IdExistException("ERROR: the customer was exist");
        }

        /// <summary>
        /// return the specific customer the user ask for
        /// </summary>
        /// <param Name="customerId"></the Id of the customer the user ask for>
        /// <returns></returns>
        public Customer GetCustomer(int customerId)
        {
            string check = IsExistCustomer(customerId);
            Customer customer = new Customer();
            if (check == "exists")
            {
                customer = DataSource.Customers.Find(elementCustomer => elementCustomer.Id == customerId);
            }
            if (check == "not exists")
                throw new IdNotFoundException("ERROR: the coustomer is not exit.");
            if (check == "was exists")
                throw new IdExistException("ERROR: the customer was exist");

            return customer;
        }

        /// <summary>
        /// return all the customer list
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Customer> GetCustomers(Predicate<Customer> customerPredicate)
        {
            IEnumerable<Customer> customers = DataSource.Customers.Where(customer => customerPredicate(customer)); 
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
        private string IsExistCustomer(int customerId)
        {
            foreach (Customer elementCustomer in DataSource.Customers)
            {

                if (elementCustomer.Id == customerId && elementCustomer.Deleted == false)
                    return "exists";
                if (elementCustomer.Id == customerId && elementCustomer.Deleted == true)
                    return "was exists";
            }
            return "not exists"; // the customer not exist
        }
    }
}
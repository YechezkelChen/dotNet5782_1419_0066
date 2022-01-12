using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using DalXml;
using DO;

namespace Dal
{
    partial class DalXml : DalApi.IDal
    {
        /// <summary>
        /// add a customer to the customers list
        /// </summary>
        /// <param Name="newCustomer"></the new customer the user whants to add to the customer's list>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddCustomer(Customer newCustomer)
        {
            XElement customers = XMLTools.LoadListFromXmlElement(customersPath);

            var addCustomer = (from c in customers.Elements()
                            where Convert.ToInt32(c.Element("Id").Value) == newCustomer.Id
                            select c).FirstOrDefault();

            if (!(addCustomer is null))
            {
                if (addCustomer.Element("Deleted").Value == "true")
                    throw new IdExistException("ERROR: the customer is deleted!\n");

                throw new IdExistException("ERROR: the customer is found!\n");
            }


            XElement id = new XElement("Id", newCustomer.Id);
            XElement name = new XElement("Name", newCustomer.Name);
            XElement phone = new XElement("Phone", newCustomer.Phone);
            XElement longitude = new XElement("Longitude", newCustomer.Longitude);
            XElement latitude = new XElement("Latitude", newCustomer.Latitude);
            XElement deleted = new XElement("Deleted", newCustomer.Deleted);

            customers.Add(new XElement("Customer", id, name, phone, longitude, latitude, deleted));

            XMLTools.SaveListToXmlElement(customers, customersPath);

        }

        /// <summary>
        /// Removes a customer from the list of customers.
        /// </summary>
        /// <param name="customerId"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void RemoveCustomer(int customerId)
        {
            XElement customers = XMLTools.LoadListFromXmlElement(customersPath);

            var deletedCustomer = (from c in customers.Elements()
                where Convert.ToInt32(c.Element("Id").Value) == customerId
                select c).FirstOrDefault();

            if (deletedCustomer is null)
                throw new IdNotFoundException("ERROR: the customer is not found!\n");
            if (deletedCustomer.Element("Deleted").Value == "true")
                throw new IdExistException("ERROR: the customer was exist");

            deletedCustomer.Element("Deleted").Value = "true";

            XMLTools.SaveListToXmlElement(customers, customersPath);
        }

        /// <summary>
        /// return the specific customer the user ask for
        /// </summary>
        /// <param Name="customerId"></the Id of the customer the user ask for>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Customer GetCustomer(int customerId)
        {
            Customer getCustomer = new Customer();
            XElement customers = XMLTools.LoadListFromXmlElement(customersPath);
            getCustomer = (from customer in customers.Elements()
                where Convert.ToInt32(customer.Element("Id").Value) == customerId
                select new Customer()
                {
                    Id = Convert.ToInt32(customer.Element("Id").Value),
                    Name = customer.Element("Name").Value,
                    Phone = customer.Element("Phone").Value,
                    Longitude = Convert.ToDouble(customer.Element("Longitude").Value),
                    Latitude = Convert.ToDouble(customer.Element("Latitude").Value),
                    Deleted = Convert.ToBoolean(customer.Element("Deleted").Value)
                }).FirstOrDefault();

            if (getCustomer.Id == 0)
                throw new IdNotFoundException("ERROR: the customer is not found.");

            if (getCustomer.Deleted == true)
                throw new IdExistException("ERROR: the customer was exist");

            return getCustomer;
        }

        /// <summary>
        /// return all the customer's list
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Customer> GetCustomers(Predicate<Customer> customerPredicate)
        {
            XElement customersXml = XMLTools.LoadListFromXmlElement(customersPath);
            IEnumerable<Customer> customers = (from customer in customersXml.Elements()
                select new Customer()
                {
                    Id = Convert.ToInt32(customer.Element("Id").Value),
                    Name = customer.Element("Name").Value,
                    Phone = customer.Element("Phone").Value,
                    Longitude = Convert.ToDouble(customer.Element("Longitude").Value),
                    Latitude = Convert.ToDouble(customer.Element("Latitude").Value),
                    Deleted = Convert.ToBoolean(customer.Element("Deleted").Value)
                });
            customers = customers.Where(customer => customerPredicate(customer));
            return customers;
        }

        /// <summary>
        /// update the specific customer the user ask for
        /// </summary>
        /// <param name="updateCustomer"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateCustomer(Customer updateCustomer)
        {
            XElement customers = XMLTools.LoadListFromXmlElement(customersPath);

            XElement customer = (from c in customers.Elements()
                              where Convert.ToInt32(c.Element("Id").Value) == updateCustomer.Id
                              select c).FirstOrDefault();

            customer.Element("Id").Value = updateCustomer.Id.ToString();
            customer.Element("Name").Value = updateCustomer.Name;
            customer.Element("Phone").Value = updateCustomer.Phone;
            customer.Element("Longitude").Value = updateCustomer.Longitude.ToString();
            customer.Element("Latitude").Value = updateCustomer.Latitude.ToString();
            customer.Element("Deleted").Value = updateCustomer.Deleted.ToString();
            XMLTools.SaveListToXmlElement(customers, customersPath);
        }
    }
}

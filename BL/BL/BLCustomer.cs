﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using DalObject;
using IBL.BO;
using IDAL;
using IDAL.DO;
using Customer = IBL.BO.Customer;
using Drone = IBL.BO.Drone;
using Parcel = IBL.BO.Parcel;
using Priorities = IBL.BO.Priorities;
using Station = IBL.BO.Station;
using WeightCategories = IBL.BO.WeightCategories;


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
            catch (DalObject.CustomerException e)
            {
                throw new CustomerException("" + e);
            }

            Customer customer = new Customer();
            customer.Id = idalCustomer.Id;
            customer.Name = idalCustomer.Name;
            customer.Phone = idalCustomer.Phone;
            customer.Location = new Location() {Longitude = idalCustomer.Longitude, Latitude = idalCustomer.Latitude};
            customer.FromTheCustomerList = new List<ParcelInCustomer>();
            customer.ToTheCustomerList = new List<ParcelInCustomer>();

            foreach (var elementParcel in dal.GetParcels())
                if (customer.Id == elementParcel.SenderId)
                {
                    ParcelInCustomer parcelInCustomer = new ParcelInCustomer();
                    parcelInCustomer.Id = elementParcel.Id;
                    parcelInCustomer.Weight = Enum.Parse<WeightCategories>(elementParcel.Weight.ToString());
                    parcelInCustomer.Priority = Enum.Parse<Priorities>(elementParcel.Priority.ToString());

                    if (elementParcel.Requested != DateTime.MinValue)
                        parcelInCustomer.Status = ParcelStatuses.Requested;
                    if (elementParcel.Scheduled != DateTime.MinValue)
                        parcelInCustomer.Status = ParcelStatuses.Scheduled;
                    if (elementParcel.PickedUp != DateTime.MinValue)
                        parcelInCustomer.Status = ParcelStatuses.PickedUp;
                    if (elementParcel.Delivered != DateTime.MinValue)
                        parcelInCustomer.Status = ParcelStatuses.Delivered;

                    parcelInCustomer.CustomerInDelivery = new CustomerInParcel()
                        {Id = customer.Id, NameCustomer = customer.Name};

                    customer.FromTheCustomerList.Add(parcelInCustomer);
                }

            foreach (var elementParcel in dal.GetParcels())
                if (customer.Id == elementParcel.TargetId)
                {
                    ParcelInCustomer parcelInCustomer = new ParcelInCustomer();
                    parcelInCustomer.Id = elementParcel.Id;
                    parcelInCustomer.Weight = Enum.Parse<WeightCategories>(elementParcel.Weight.ToString());
                    parcelInCustomer.Priority = Enum.Parse<Priorities>(elementParcel.Priority.ToString());
                    if (elementParcel.Delivered != DateTime.MinValue)
                        parcelInCustomer.Status = ParcelStatuses.Delivered;
                    if (elementParcel.PickedUp != DateTime.MinValue)
                        parcelInCustomer.Status = ParcelStatuses.PickedUp;
                    if (elementParcel.Scheduled != DateTime.MinValue)
                        parcelInCustomer.Status = ParcelStatuses.Scheduled;
                    if (elementParcel.Requested != DateTime.MinValue)
                        parcelInCustomer.Status = ParcelStatuses.Requested;

                    parcelInCustomer.CustomerInDelivery = new CustomerInParcel()
                        {Id = customer.Id, NameCustomer = customer.Name};

                    customer.ToTheCustomerList.Add(parcelInCustomer);
                }

            return customer;
        }

        public IEnumerable<CustomerToList> GetCustomers()
        {
            IEnumerable<IDAL.DO.Customer> idalcCustomers = dal.GetCustomers();
            List<CustomerToList> customerToLists = new List<CustomerToList>();
            

            foreach (var idalCustomer in idalcCustomers)
            {
                CustomerToList newCustomer = new CustomerToList();
                newCustomer.Id = idalCustomer.Id;
                newCustomer.Name = idalCustomer.Name;
                newCustomer.Phone = idalCustomer.Phone;

                // zero the variables
                newCustomer.SenderParcelDelivered = 0;
                newCustomer.SenderParcelPickedUp = 0;
                newCustomer.TargetParcelDelivered = 0;
                newCustomer.TargetParcelPickedUp = 0;

                foreach (var elementParcel in dal.GetParcels())
                {
                    if (elementParcel.SenderId == idalCustomer.Id && elementParcel.Delivered != DateTime.MinValue)
                        newCustomer.SenderParcelDelivered++;
                    else if (elementParcel.SenderId == idalCustomer.Id && elementParcel.PickedUp != DateTime.MinValue)
                        newCustomer.SenderParcelPickedUp++;
                    if (elementParcel.TargetId == idalCustomer.Id && elementParcel.Delivered != DateTime.MinValue)
                        newCustomer.TargetParcelDelivered++;
                    else if (elementParcel.TargetId == idalCustomer.Id && elementParcel.PickedUp != DateTime.MinValue)
                        newCustomer.TargetParcelPickedUp++;
                }

                customerToLists.Add(newCustomer);
            }

            return customerToLists;
        }

        public void UpdateDataCustomer(int id, string name, string phone)
        {
            IDAL.DO.Customer updateCustomer = new IDAL.DO.Customer();
            try
            {
                updateCustomer = dal.GetCustomer(id);
            }
            catch (DalObject.CustomerException e)
            {
                throw new DalObject.CustomerException("" + e);
            }

            if (name == "" && phone == "")
                throw new DalObject.CustomerException("ERROR: need one thing at least to change");

            if (name != "")
                updateCustomer.Name = name;

            if (phone != "")
                updateCustomer.Phone = phone;

            dal.UpdateCustomer(updateCustomer);
        }

        private void CheckCustomer(Customer customer)
        {
            if (customer.Id < 0)
                throw new CustomerException("ERROR: the ID is illegal! ");
            if (customer.Name == "")
                throw new DalObject.CustomerException("ERROR: Name must have value");
            if (customer.Phone == "")
                throw new DalObject.CustomerException("ERROR: Phone must have value");
        }

        private IEnumerable<IDAL.DO.Customer> ListCustomersWithDelivery(IEnumerable<IDAL.DO.Customer> customers,
            IEnumerable<IDAL.DO.Parcel> Parcels)
        {
            List<IDAL.DO.Customer> newCustomers = new List<IDAL.DO.Customer>();
            foreach (var elementCustomer in customers)
            {
                foreach (var elementParcel in Parcels)
                {
                    if (elementParcel.TargetId == elementCustomer.Id && elementParcel.Delivered != DateTime.MinValue)
                        newCustomers.Add(elementCustomer);
                }
            }

            return newCustomers;
        }
    }
}
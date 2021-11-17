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
            customer.Id = newCustomer.Id * 10 + lastDigitID(newCustomer.Id);
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
                throw new CustomerException("" + e);
            }

            if (name == "" && phone == "")
                throw new CustomerException("ERROR: need one thing at least to change");

            if (name != "")
                updateCustomer.Name = name;

            if (phone != "")
            {
                if(phone.Length != 10)
                    throw new CustomerException("ERROR: Phone must have 10 digits");
                updateCustomer.Phone = phone;
            }

            dal.UpdateCustomer(updateCustomer);
        }

        private void CheckCustomer(Customer customer)
        {
            if(customer.Id < 10000000 || customer.Id > 99999999)//Check that it's 8 digits.
                throw new CustomerException("ERROR: the ID is illegal! ");
            if (customer.Name.Length == 0)
                throw new CustomerException("ERROR: Name must have value");
            if (customer.Phone.Length != 10)
                throw new CustomerException("ERROR: Phone must have 10 digits");
        }

        private int lastDigitID(int lessID)
        {
            int digit1, digit2, sumResultDigits = 0, digitID;
            for (int i = 1; i <= lessID; i++)
            {
                digit1 = lessID % 10;
                digit1 *= 2;//Calculating the digits double their weight.
                sumResultDigits += sumDigits(digit1);//The sum of the result digits.
                lessID /= 10;
                digit2 = lessID % 10;
                digit2 *= 1;//Calculating the digits double their weight.
                sumResultDigits += sumDigits(digit2);//The sum of the result digits.
                lessID /= 10;
            }
            sumResultDigits %= 10;//The unity digit of the result.

            digitID = 10 - sumResultDigits;
            return digitID;//Returning the missing digit.v
        }

        private int sumDigits(int num)//Entering a number by the computer.
        {
            int sum_digits = 0;
            while (num > 0)
            {
                sum_digits += num % 10;
                num = num / 10;
            }
            return sum_digits;//Return of the sum of his digits.
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

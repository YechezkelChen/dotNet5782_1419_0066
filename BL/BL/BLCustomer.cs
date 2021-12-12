using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using BO;


namespace BL
{
    partial class BL : BlApi.IBL
    {
        /// <summary>
        /// add a customer
        /// </summary>
        /// <returns></no returns, add a customer>
        public void AddCustomer(Customer newCustomer)
        {
            try
            {
                CheckCustomer(newCustomer);// check the input of the user
            }
            catch (CustomerException e)
            {
                throw new CustomerException(e.Message, e);
            }

            DO.Customer customer = new DO.Customer();
            customer.Id = newCustomer.Id * 10 + lastDigitID(newCustomer.Id);
            customer.Name = newCustomer.Name;
            customer.Phone = newCustomer.Phone; 
            customer.Longitude = newCustomer.Location.Longitude;
            customer.Latitude = newCustomer.Location.Latitude;
            dal.AddCustomer(customer);
        }

        /// <summary>
        /// get a customer
        /// </summary>
        /// <returns></return the customer>
        public Customer GetCustomer(int id)
        {
            DO.Customer idalCustomer = new DO.Customer();
            try
            {
                idalCustomer = dal.GetCustomer(id);// if the customer not in data 
            }
            catch (Dal.CustomerException e)
            {
                throw new CustomerException(e.Message, e);
            }

            Customer customer = new Customer();
            customer.Id = idalCustomer.Id;
            customer.Name = idalCustomer.Name;
            customer.Phone = idalCustomer.Phone;
            customer.Location = new Location() {Longitude = idalCustomer.Longitude, Latitude = idalCustomer.Latitude};
            customer.FromTheCustomerList = new List<ParcelInCustomer>();
            customer.ToTheCustomerList = new List<ParcelInCustomer>();

            foreach (var elementParcel in dal.GetParcels(parcel => true))// accordion to the conditions in the exercise
                if (customer.Id == elementParcel.SenderId)
                {
                    ParcelInCustomer parcelInCustomer = new ParcelInCustomer();
                    parcelInCustomer.Id = elementParcel.Id;
                    parcelInCustomer.Weight = (WeightCategories)elementParcel.Weight;
                    parcelInCustomer.Priority = (Priorities)elementParcel.Priority;

                    if (elementParcel.Requested != null)
                        parcelInCustomer.Status = ParcelStatuses.Requested;
                    if (elementParcel.Scheduled != null)
                        parcelInCustomer.Status = ParcelStatuses.Scheduled;
                    if (elementParcel.PickedUp != null)
                        parcelInCustomer.Status = ParcelStatuses.PickedUp;
                    if (elementParcel.Delivered != null)
                        parcelInCustomer.Status = ParcelStatuses.Delivered;

                    parcelInCustomer.CustomerInDelivery = new CustomerInParcel()
                        {Id = customer.Id, NameCustomer = customer.Name};

                    customer.FromTheCustomerList.ToList().Add(parcelInCustomer);
                }

            foreach (var elementParcel in dal.GetParcels(parcel => true))
                if (customer.Id == elementParcel.TargetId)
                {
                    ParcelInCustomer parcelInCustomer = new ParcelInCustomer();
                    parcelInCustomer.Id = elementParcel.Id;
                    parcelInCustomer.Weight = (WeightCategories)elementParcel.Weight;
                    parcelInCustomer.Priority = (Priorities)elementParcel.Priority;
                    if (elementParcel.Delivered != null)
                        parcelInCustomer.Status = ParcelStatuses.Delivered;
                    if (elementParcel.PickedUp != null)
                        parcelInCustomer.Status = ParcelStatuses.PickedUp;
                    if (elementParcel.Scheduled != null)
                        parcelInCustomer.Status = ParcelStatuses.Scheduled;
                    if (elementParcel.Requested != null)
                        parcelInCustomer.Status = ParcelStatuses.Requested;

                    parcelInCustomer.CustomerInDelivery = new CustomerInParcel()
                        {Id = customer.Id, NameCustomer = customer.Name};

                    customer.ToTheCustomerList.ToList().Add(parcelInCustomer);
                }

            return customer;
        }

        /// <summary>
        /// get a customers
        /// </summary>
        /// <returns></return all customers>
        public IEnumerable<CustomerToList> GetCustomers()
        {
            List<CustomerToList> customerToLists = new List<CustomerToList>();
            
            foreach (var idalCustomer in dal.GetCustomers(customer => true))
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

                foreach (var elementParcel in dal.GetParcels(parcel => true))
                {
                    if (elementParcel.SenderId == idalCustomer.Id && elementParcel.Delivered != null)
                        newCustomer.SenderParcelDelivered++;
                    else if (elementParcel.SenderId == idalCustomer.Id && elementParcel.PickedUp != null)
                        newCustomer.SenderParcelPickedUp++;
                    if (elementParcel.TargetId == idalCustomer.Id && elementParcel.Delivered != null)
                        newCustomer.TargetParcelDelivered++;
                    else if (elementParcel.TargetId == idalCustomer.Id && elementParcel.PickedUp != null)
                        newCustomer.TargetParcelPickedUp++;
                }

                customerToLists.Add(newCustomer);
            }

            return customerToLists;
        }

        /// <summary>
        /// Update the data of the customer
        /// </summary>
        /// <returns></no returns, update the data of the customer>
        public void UpdateDataCustomer(int id, string name, string phone)
        {
            DO.Customer updateCustomer = new DO.Customer();
            try
            {
                updateCustomer = dal.GetCustomer(id);
            }
            catch (Dal.CustomerException e)// if the customer not exist
            {
                throw new CustomerException(e.Message, e);
            }

            if (name == "" && phone == "")// if the user not pur anything
                throw new CustomerException("ERROR: need one thing at least to change");

            // update the data according to the user asks
            if (name != "")
                updateCustomer.Name = name;

            if (phone != "")
            {
                if(phone.Length != 10)
                    throw new CustomerException("ERROR: Phone must have 10 digits");
                updateCustomer.Phone = phone;
            }

            dal.UpdateCustomer(updateCustomer);// update the data center
        }

        /// <summary>
        /// Check the input of the user
        /// </summary>
        /// <returns></no returns, just check the input of the user>
        private void CheckCustomer(Customer customer)
        {
            if(customer.Id < 10000000 || customer.Id > 99999999)//Check that it's 8 digits.
                throw new CustomerException("ERROR: the ID is illegal! ");
            if (customer.Name.Length == 0)
                throw new CustomerException("ERROR: Name must have value");
            int phone;
            if (customer.Phone.Length != 10 || customer.Phone.Substring(0, 2) != "05" ||
                !int.TryParse(customer.Phone.Substring(0, customer.Phone.Length), out phone)) // check format phone
                throw new CustomerException("ERROR: Phone must have 10 digits and to begin with the numbers 05");
            if (customer.Location.Longitude < -1 || customer.Location.Longitude > 1)
                throw new CustomerException("ERROR: Longitude must to be between -1 to 1");
            if (customer.Location.Latitude < -1 || customer.Location.Latitude > 1)
                throw new CustomerException("ERROR: Latitude must to be between -1 to 1");
        }

        /// <summary>
        /// get last digit of the id
        /// </summary>
        /// <returns></return the last digit of the id>
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

        /// <summary>
        ///Entering a number by the computer.
        /// <returns></return the sum of digit >
        private int sumDigits(int num)
        {
            int sum_digits = 0;
            while (num > 0)
            {
                sum_digits += num % 10;
                num = num / 10;
            }
            return sum_digits;//Return of the sum of his digits.
        }
    }
}

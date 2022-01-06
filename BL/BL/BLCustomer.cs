using System.Collections.Generic;
using System.Linq;
using BO;
using System.Runtime.CompilerServices;

namespace BL
{
    partial class BL : BlApi.IBL
    {
        /// <summary>
        /// add a customer
        /// </summary>
        /// <returns></no returns, add a customer>
       
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddCustomer(Customer newCustomer)
        {
            try
            {
                CheckCustomer(newCustomer); // check the input of the user
            }
            catch (IdException e)
            {
                throw new IdException(e.Message, e);
            }
            catch (NameException e)
            {
                throw new NameException(e.Message, e);
            }
            catch (PhoneException e)
            {
                throw new PhoneException(e.Message, e);
            }
            catch (LocationException e)
            {
                throw new LocationException(e.Message, e);
            }

            lock (dal)
            {
                DO.Customer customer = new DO.Customer();
                customer.Id = newCustomer.Id * 10 + LastDigitId(newCustomer.Id); // Add check digit to Id
                customer.Name = newCustomer.Name;
                customer.Phone = newCustomer.Phone;
                customer.Longitude = newCustomer.Location.Longitude;
                customer.Latitude = newCustomer.Location.Latitude;
                customer.Deleted = false;
                try
                {
                    dal.AddCustomer(customer);
                }
                catch (DO.IdExistException ex)
                {
                    throw new IdException(ex.Message, ex);
                }
            }
        }

        /// <summary>
        /// Removes a customer from the list of customers.
        /// </summary>
        /// <param name="customerId"></param>
       
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void RemoveCustomer(int customerId)
        {
            lock (dal)
            {
                try
                {
                    dal.RemoveCustomer(customerId); // Remove the customer
                }
                catch (DO.IdExistException e)
                {
                    throw new IdException(e.Message, e);
                }
                catch (DO.IdNotFoundException e)
                {
                    throw new IdException(e.Message, e);
                }
            }
        }

        /// <summary>
        /// get a customer
        /// </summary>
        /// <returns></return the customer>
      
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Customer GetCustomer(int customerId)
        {
            Customer customer = new Customer();

            lock (dal)
            {
                DO.Customer dalCustomer = new DO.Customer();
                try
                {
                    dalCustomer = dal.GetCustomer(customerId);
                }
                catch (DO.IdNotFoundException e)
                {
                    throw new IdException(e.Message, e);
                }

                customer.Id = dalCustomer.Id;
                customer.Name = dalCustomer.Name;
                customer.Phone = dalCustomer.Phone;
                customer.Location = new Location() {Longitude = dalCustomer.Longitude, Latitude = dalCustomer.Latitude};

                customer.FromTheCustomerList = from parcelToList in GetParcels()
                    let parcel = GetParcel(parcelToList.Id)
                    where customer.Id == parcel.Sender.Id
                    select new ParcelInCustomer
                    {
                        Id = parcelToList.Id,
                        Weight = parcelToList.Weight,
                        Priority = parcelToList.Priority,
                        Status = parcelToList.Status,
                        CustomerInDelivery = new CustomerInParcel() {Id = parcel.Target.Id, Name = parcel.Target.Name}
                    };

                customer.ToTheCustomerList = from parcelToList in GetParcels()
                    let parcel = GetParcel(parcelToList.Id)
                    where customer.Id == parcel.Target.Id
                    select new ParcelInCustomer
                    {
                        Id = parcelToList.Id,
                        Weight = parcelToList.Weight,
                        Priority = parcelToList.Priority,
                        Status = parcelToList.Status,
                        CustomerInDelivery = new CustomerInParcel() {Id = parcel.Sender.Id, Name = parcel.Sender.Name}
                    };
            }

            return customer;
        }

        /// <summary>
        /// get a customers
        /// </summary>
        /// <returns></return all customers>
       
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<CustomerToList> GetCustomers()
        {
            lock (dal)
            {
                return from dalCustomer in dal.GetCustomers(customer => customer.Deleted == false)
                    let customer = GetCustomer(dalCustomer.Id)
                    select new CustomerToList
                    {
                        Id = dalCustomer.Id,
                        Name = dalCustomer.Name,
                        Phone = dalCustomer.Phone,
                        SenderParcelDelivered = customer.FromTheCustomerList.Count(parcel => parcel.Status == ParcelStatuses.Delivered),
                        SenderParcelPickedUp = customer.FromTheCustomerList.Count(parcel => parcel.Status == ParcelStatuses.PickedUp),
                        TargetParcelDelivered = customer.ToTheCustomerList.Count(parcel => parcel.Status == ParcelStatuses.Delivered),
                        TargetParcelPickedUp = customer.ToTheCustomerList.Count(parcel => parcel.Status == ParcelStatuses.PickedUp)
                    };
            }
        }

        /// <summary>
        /// Update the data of the customer
        /// </summary>
        /// <returns></no returns, update the data of the customer>
      
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateDataCustomer(int id, string name, string phone)
        {
            lock (dal)
            {
                DO.Customer updateCustomer = new DO.Customer();
                try
                {
                    updateCustomer = dal.GetCustomer(id);
                }
                catch (DO.IdNotFoundException e) // if the customer not exist
                {
                    throw new IdException(e.Message, e);
                }

                if (name == "" && phone == "") // if the user not put anything
                    throw new NameException("ERROR: need one thing at least to change");

                if (name != "")
                    updateCustomer.Name = name;

                if (phone != "")
                {
                    if (phone.Length != 10)
                        throw new PhoneException("ERROR: Phone must have 10 digits");
                    updateCustomer.Phone = phone;
                }

                dal.UpdateCustomer(updateCustomer); // update the data center
            }
        }

        /// <summary>
        /// get last digit of the id
        /// </summary>
        /// <returns></return the last digit of the id>

        [MethodImpl(MethodImplOptions.Synchronized)]
        public int LastDigitId(int lessId)
        {
            int digit1, digit2, sumResultDigits = 0, digitID;
            for (int i = 1; i <= lessId; i++)
            {
                digit1 = lessId % 10;
                digit1 *= 2;//Calculating the digits double their weight.
                sumResultDigits += SumDigits(digit1);//The sum of the result digits.
                lessId /= 10;
                digit2 = lessId % 10;
                digit2 *= 1;//Calculating the digits double their weight.
                sumResultDigits += SumDigits(digit2);//The sum of the result digits.
                lessId /= 10;
            }
            sumResultDigits %= 10;//The unity digit of the result.

            digitID = 10 - sumResultDigits;
            return digitID;//Returning the missing digit.v
        }

        /// <summary>
        /// Sum digits of number
        /// <returns></return the sum of digit >
        private int SumDigits(int num)
        {
            int sum_digits = 0;
            while (num > 0)
            {
                sum_digits += num % 10;
                num = num / 10;
            }
            return sum_digits;//Return of the sum of his digits.
        }

        /// <summary>
        /// Check the input of the user
        /// </summary>
        /// <returns></no returns, just check the input of the user>
        private void CheckCustomer(Customer customer)
        {
            if (customer.Id < 10000000 || customer.Id > 99999999) // Check that it's 8 digits.
                throw new IdException("ERROR: the ID is illegal! ");
            if (customer.Name.Length == 0)
                throw new NameException("ERROR: name must have value");
            int phone;
            if (customer.Phone.Length != 10 || customer.Phone.Substring(0, 2) != "05" ||
                !int.TryParse(customer.Phone.Substring(2, customer.Phone.Length - 2), out phone)) // check format phone
                throw new PhoneException("ERROR: phone must have 10 digits and to begin with the numbers 05");
            if (customer.Location.Longitude < -1 || customer.Location.Longitude > 1)
                throw new LocationException("ERROR: longitude must to be between -1 to 1");
            if (customer.Location.Latitude < -1 || customer.Location.Latitude > 1)
                throw new LocationException("ERROR: latitude must to be between -1 to 1");
        }
    }
}

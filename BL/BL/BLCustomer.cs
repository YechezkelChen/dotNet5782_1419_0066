using System.Collections.Generic;
using System.Linq;
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

            DO.Customer customer = new DO.Customer();
            customer.Id = newCustomer.Id * 10 + lastDigitID(newCustomer.Id); // Add check digit to Id
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

        /// <summary>
        /// get a customer
        /// </summary>
        /// <returns></return the customer>
        public Customer GetCustomer(int customerId)
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

            Customer customer = new Customer();
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
                                               CustomerInDelivery = new CustomerInParcel() { Id = parcel.Target.Id, Name = parcel.Target.Name }
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
                                             CustomerInDelivery = new CustomerInParcel() { Id = parcel.Sender.Id, Name = parcel.Sender.Name }
                                         };
            return customer;
        }

        /// <summary>
        /// get a customers
        /// </summary>
        /// <returns></return all customers>
        public IEnumerable<CustomerToList> GetCustomers()
        {
            return from dalCustomer in dal.GetCustomers(customer => customer.Deleted == false)
                   let customer = GetCustomer(dalCustomer.Id)
                   select new CustomerToList
                   {
                       Id = dalCustomer.Id,
                       Name = dalCustomer.Name,
                       Phone = dalCustomer.Phone,
                       SenderParcelDelivered = customer.FromTheCustomerList.Where(parcel => parcel.Status == ParcelStatuses.Delivered).Count(),
                       SenderParcelPickedUp = customer.FromTheCustomerList.Where(parcel => parcel.Status == ParcelStatuses.PickedUp).Count(),
                       TargetParcelDelivered = customer.ToTheCustomerList.Where(parcel => parcel.Status == ParcelStatuses.Delivered).Count(),
                       TargetParcelPickedUp = customer.ToTheCustomerList.Where(parcel => parcel.Status == ParcelStatuses.PickedUp).Count()
                   };
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
            catch (DO.IdNotFoundException e) // if the customer not exist
            {
                throw new IdException(e.Message,e);
            }

            if (name == "" && phone == "") // if the user not put anything
                throw new NameException("ERROR: need one thing at least to change");

            if (name != "")
                updateCustomer.Name = name;

            if (phone != "")
            {
                if(phone.Length != 10)
                    throw new PhoneException("ERROR: Phone must have 10 digits");
                updateCustomer.Phone = phone;
            }

            dal.UpdateCustomer(updateCustomer); // update the data center
        }

        /// <summary>
        /// Check the input of the user
        /// </summary>
        /// <returns></no returns, just check the input of the user>
        private void CheckCustomer(Customer customer)
        {
            if(customer.Id < 10000000 || customer.Id > 99999999) // Check that it's 8 digits.
                throw new IdException("ERROR: the ID is illegal! ");
            if (customer.Name.Length == 0)
                throw new NameException("ERROR: name must have value");
            int phone;
            if (customer.Phone.Length != 10 || customer.Phone.Substring(0, 2) != "05" ||
                !int.TryParse(customer.Phone.Substring(2, customer.Phone.Length), out phone)) // check format phone
                throw new PhoneException("ERROR: phone must have 10 digits and to begin with the numbers 05");
            if (customer.Location.Longitude < -1 || customer.Location.Longitude > 1)
                throw new LocationException("ERROR: longitude must to be between -1 to 1");
            if (customer.Location.Latitude < -1 || customer.Location.Latitude > 1)
                throw new LocationException("ERROR: latitude must to be between -1 to 1");
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

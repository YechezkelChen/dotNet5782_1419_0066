using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace PL
{
    public class NotConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool) value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool) value;
        }
    };

    public class MultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values.Cast<bool>().Any(x => x) ? Visibility.Collapsed : Visibility.Visible;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class FromColorTextToIsEnable : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values.Cast<SolidColorBrush>().Any(x =>  x == Brushes.Red) ? false : true;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class IdTextToColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int id;
            if (value.ToString() == "" || !value.ToString().All(char.IsDigit))
                id = 0;
            else
                id = int.Parse(value.ToString());

            if (id < 100000 || id > 999999) // Check that it's 6 digits.
                return Brushes.Red;
            else
                return Brushes.SlateGray;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class IdCustomerTextToColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int id;
            if (value.ToString() == "" || !value.ToString().All(char.IsDigit))
                id = 0;
            else
                id = int.Parse(value.ToString());

            if (id < 10000000 || id > 99999999) // Check that it's 6 digits.
                return Brushes.Red;
            else
                return Brushes.SlateGray;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ModelOrNameTextToColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.ToString() == "")
                return Brushes.Red;
            else
                return Brushes.SlateGray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ComboBoxToColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return Brushes.Red;
            else
                return Brushes.SlateGray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    public class LocationTextToColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        { 
            double location;
            if (value.ToString() == "" || !value.ToString().All(char.IsDigit))
                location = 0;
            else
                location = System.Convert.ToDouble(value.ToString());

            if (location < -1 || location > 1)
                return Brushes.Red;
            else
                return Brushes.SlateGray;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ChargeSlotsTextToColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int chargeSlots;
            if (value.ToString() == "" || !value.ToString().All(char.IsDigit))
                chargeSlots = 0;
            else
                chargeSlots = int.Parse(value.ToString());

            if (chargeSlots < 0) // Check that it's 6 digits.
                return Brushes.Red;
            else
                return Brushes.SlateGray;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class EmptyListToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) 
                return false;
            else
                return true;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    //int phone;
    ////if (customer.Phone.Length != 10 || customer.Phone.Substring(0, 2) != "05" ||
    ////    !int.TryParse(customer.Phone.Substring(2, customer.Phone.Length), out phone)) // check format phone

    //if (value.ToString() == "")
    //return Brushes.Red;
    //else
    //return Brushes.SlateGray;
}

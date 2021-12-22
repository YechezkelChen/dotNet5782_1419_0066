using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace PL
{
    /// <summary>
    /// Interaction logic for ListWindow.xaml
    /// </summary>
    public partial class ListWindow : Window
    {
        public ListWindow()
        {
            InitializeComponent();
        }
        private void ListTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowData.Content = "";
            if (ListDrones.IsSelected)
                ShowList.Content = new DroneListPage(this);
            if (ListStations.IsSelected)
                ShowList.Content = new StationListPage(this);
            if (ListCustomers.IsSelected)
                ShowList.Content = new CustomerListPage(this);
            if (ListParcels.IsSelected)
                ShowList.Content = new ParcelListPage(this);
            if (CloseWindow.IsSelected)
            {
                CloseWindow.Visibility = Visibility.Hidden;
                this.Close();
            }
        }
        private void CloseWithSpecialButton(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (CloseWindow.Visibility != Visibility.Hidden)
                e.Cancel = true;
        }

        public void CopyPropertiesTo<T, S>(S from, T to)
        {
            foreach (PropertyInfo propTo in to.GetType().GetProperties())
            {
                PropertyInfo propFrom = typeof(S).GetProperty(propTo.Name);
                if (propFrom == null)
                    continue;
                var value = propFrom.GetValue(from, null);
                if (value is ValueType || value is string)
                    propTo.SetValue(to, value);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;
using PO;

namespace PL
{
    /// <summary>
    /// Interaction logic for ParcelListPage.xaml
    /// </summary>
    public partial class ParcelListPage : Page
    {
        private BlApi.IBL bl = BlApi.BlFactory.GetBl();
        private ObservableCollection<ParcelToList> parcels;

        public ParcelListPage(ObservableCollection<ParcelToList> parcels)
        {
            InitializeComponent();
            this.parcels = parcels;
            ParcelsListView.DataContext = this.parcels;
            ParcelsData();

            StatusSelector.ItemsSource = Enum.GetValues(typeof(ParcelStatuses));
        }

        private void ParcelsData()
        {
            parcels.Clear();

            IEnumerable<BO.ParcelToList> parcelsData = new List<BO.ParcelToList>();
            IEnumerable<BO.ParcelToList> parcelsFiltering = new List<BO.ParcelToList>();

            parcelsData = bl.GetParcels();

            // Filtering of status.
            if (StatusSelector.SelectedItem == null)
                parcelsFiltering = bl.GetParcels();
            else
                parcelsFiltering = bl.GetParcelsByStatus((BO.ParcelStatuses)StatusSelector.SelectedItem);

            parcelsData = parcelsFiltering.ToList().FindAll(parcel => parcelsData.ToList().Find(p => p.Id == parcel.Id) != null);

            foreach (var parcel in parcelsData)
            {
                ParcelToList newParcel = new ParcelToList();
                bl.CopyPropertiesTo(parcel, newParcel);
                parcels.Add(newParcel);
            }
        }

        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ParcelsData();
        }

        private void DateSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ParcelsData();
        }

        private void GroupByCustomersSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            parcels.Clear();
            if (GroupByCustomersSelector.SelectedItem != null)
            {
                var groups = bl.GetParcelsByGroupCustomers(GroupByCustomersSelector.SelectedItem.ToString());
                foreach (var group in groups)
                    foreach (var parcel in group)
                    {
                        ParcelToList newParcel = new ParcelToList();
                        bl.CopyPropertiesTo(parcel, newParcel);
                        parcels.Add(newParcel);
                    }
            }
        }

        private void RefreshStatusButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            StatusSelector.SelectedItem = null;
            ParcelsData();
        }

        private void RefreshDateButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            DateSelector.SelectedItem = null;
            ParcelsData();
        }

        private void RefreshGroupButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            GroupByCustomersSelector.SelectedItem = null;
            ParcelsData();
        }
    }
}

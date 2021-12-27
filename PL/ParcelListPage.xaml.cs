using System;
using System.Collections.ObjectModel;
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
        }

        private void ParcelsData()
        {
            parcels.Clear();
            foreach (var parcel in bl.GetParcels())
            {
                ParcelToList newParcel = new ParcelToList();
                CopyPropertiesTo(parcel, newParcel);
                parcels.Add(newParcel);
            }
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

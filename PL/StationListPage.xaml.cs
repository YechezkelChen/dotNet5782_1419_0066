﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for StationListPage.xaml
    /// </summary>
    public partial class StationListPage : Page
    {
        ListWindow listWindow;
        private BlApi.IBL bl;
        public StationListPage(ListWindow Window)
        {
            InitializeComponent();
        }

        private void StationsListView_MouseEnter(object sender, MouseEventArgs e)
        {
            StationToList stationToList = (StationToList)StationsListView.SelectedItem;
            Station station = bl.GetStation(stationToList.Id);
            listWindow.ShowData.Content = new StationPage(listWindow, station);
            ShowDronesAfterFiltering();
        }
    }
}

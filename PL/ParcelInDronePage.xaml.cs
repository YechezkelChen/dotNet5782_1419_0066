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
    /// Interaction logic for ParcelInDrone.xaml
    /// </summary>
    public partial class ParcelInDronePage : Page
    {
        public ParcelInDronePage(Drone drone)
        {
            InitializeComponent();
            Drone shoeDrone = drone;
            BlockingControls();
            ShowParcelInDrone(shoeDrone);
        }

        void BlockingControls()
        {
            IdParcelTextBox.IsEnabled = false;
            StatusParcelTextBox.IsEnabled = false;
            PriorityParcelTextBox.IsEnabled = false;
            WeightParcelTextBox.IsEnabled = false;
            SenderInParcelTextBox.IsEnabled = false;
            PickUpLocationParcelTextBox.IsEnabled = false;
            ReceiverInParcelTextBox.IsEnabled = false;
            TargetLocationParcelTextBox.IsEnabled = false;
            DistanceOfTransferTextBox.IsEnabled = false;
        }

        void ShowParcelInDrone(Drone drone)
        {
            if (drone.ParcelByTransfer is null)
            {
                IdParcelTextBox.Text = "";
                StatusParcelTextBox.Text = "";
                PriorityParcelTextBox.Text = "";
                WeightParcelTextBox.Text = "";
                SenderInParcelTextBox.Text = "";
                PickUpLocationParcelTextBox.Text = "";
                ReceiverInParcelTextBox.Text = "";
                TargetLocationParcelTextBox.Text = "";
                DistanceOfTransferTextBox.Text = "";
            }
            else
            {
                IdParcelTextBox.Text = drone.ParcelByTransfer.Id.ToString();
                StatusParcelTextBox.Text = drone.ParcelByTransfer.OnTheWay.ToString();
                PriorityParcelTextBox.Text = drone.ParcelByTransfer.Priority.ToString();
                WeightParcelTextBox.Text = drone.ParcelByTransfer.Weight.ToString();
                SenderInParcelTextBox.Text = drone.ParcelByTransfer.Sender.ToString();
                PickUpLocationParcelTextBox.Text = drone.ParcelByTransfer.PickUpLocation.ToString();
                ReceiverInParcelTextBox.Text = drone.ParcelByTransfer.Target.ToString();
                TargetLocationParcelTextBox.Text = drone.ParcelByTransfer.TargetLocation.ToString();
                DistanceOfTransferTextBox.Text = drone.ParcelByTransfer.DistanceOfTransfer.ToString();
            }
        }
    }
}

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

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IBL.IBL bl;
        public MainWindow()
        {
            InitializeComponent();
            bl = new IBL.BL();
        }

        private void ShowDroneButton_Click(object sender, RoutedEventArgs e)
        {
            new DroneListWindow(bl).ShowDialog();
            this.Close(); //לשאול את יאיר למה זה לא עובד אם שמים אותו לפני הקריאה לבנאי??? ככה זה סוגר יפה
        }
    }
}
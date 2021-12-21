﻿using System;
using System.ComponentModel;

namespace PO
{
    public class Parcel : INotifyPropertyChanged
    {
        public int Id
        {
            get => Id;
            set { Id = value; OnPropertyChanged("Id"); }
        }

        public CustomerInParcel Sender
        {
            get => Sender;
            set { Sender = value; OnPropertyChanged("Sender"); }
        }
        public CustomerInParcel Target
        {
            get => Target;
            set { Target = value; OnPropertyChanged("Target"); }
        }

        public WeightCategories Weight
        {
            get => Weight;
            set { Weight = value; OnPropertyChanged("Weight"); }
        }

        public Priorities Priority
        {
            get => Priority;
            set { Priority = value; OnPropertyChanged("Priority"); }
        }

        public DroneInParcel DroneInParcel
        {
            get => DroneInParcel;
            set { DroneInParcel = value; OnPropertyChanged("DroneInParcel"); }
        }

        public DateTime? Requested
        {
            get => Requested;
            set { Requested = value; OnPropertyChanged("Requested"); }
        }
        public DateTime? Scheduled
        {
            get => Scheduled;
            set { Scheduled = value; OnPropertyChanged("Scheduled"); }
        }
        public DateTime? PickedUp
        {
            get => PickedUp;
            set { PickedUp = value; OnPropertyChanged("PickedUp"); }
        }
        public DateTime? Delivered
        {
            get => Delivered;
            set { Delivered = value; OnPropertyChanged("Delivered"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
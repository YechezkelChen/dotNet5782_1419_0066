using System;
using System.ComponentModel;

namespace IBL
{
    namespace BO
    {
        public enum WeightCategories
        {
            [Description("Light Weight")]
            Light,
            [Description("Medium Weight")]
            Medium,
            [Description("Heavy Weight")]
            Heavy
        }

        public enum DroneStatuses
        {
            [Description("Available Statuse")]
            Available,
            [Description("Maintenance Statuse")]
            Maintenance,
            [Description("Delivery Statuse")]
            Delivery
        }

        public enum Priorities
        {
            [Description("Normal Priority")]
            Normal,
            [Description("Fast Priority")]
            Fast,
            [Description("Emergency Priority")]
            Emergency
        }

        public enum ParcelStatuses
        {
            [Description("Requested Statuse")]
            Requested,
            [Description("Scheduled Statuse")]
            Scheduled,
            [Description("PickedUp Statuse")]
            PickedUp,
            [Description("Delivered Statuse")]
            Delivered
        }
    }
}
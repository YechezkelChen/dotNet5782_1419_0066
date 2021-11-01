using System;
using System.ComponentModel;
namespace IBL
{
    namespace BO
    {
        public enum WeightCategories
        {
            [Description("Light weight")]
            Light,
            [Description("Medium weight")]
            Medium,
            [Description("Heavy weight")]
            Heavy
        }


        public enum DroneStatuses
        {
            [Description("Available status")]
            Available,
            [Description("Maintenance status")]
            Maintenance,
            [Description("Delivery status")]
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
    }
}
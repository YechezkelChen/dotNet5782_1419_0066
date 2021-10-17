using System;

namespace IDAL
{
    namespace DO
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
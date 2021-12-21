using System.ComponentModel;


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
        [Description("Available Status")]
        Available,
        [Description("Maintenance Status")]
        Maintenance,
        [Description("Delivery Status")]
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
        [Description("Requested Status")]
        Requested,
        [Description("Scheduled Status")]
        Scheduled,
        [Description("PickedUp Status")]
        PickedUp,
        [Description("Delivered Status")]
        Delivered
    }
}
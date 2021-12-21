using System.ComponentModel;

namespace DO
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
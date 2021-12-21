namespace BO
{
    public class ParcelToList
    {
        public int Id { get; set; }
        public string SenderName { get; set; }
        public string TargetName { get; set; }
        public WeightCategories Weight { get; set; }
        public Priorities Priority { get; set; }
        public ParcelStatuses Status { get; set; }
        public override string ToString()
        {
            return $"Id #{Id}: Sender name = {SenderName},Target name = {TargetName}," +
                   $"Weight = {Weight},  Priority = {Priority}, Status = {Status}";
        }
    }
}
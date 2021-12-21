namespace BO
{
    public class ParcelInCustomer
    {
        public int Id { get; set; }
        public WeightCategories Weight { get; set; }
        public Priorities Priority { get; set; }
        public ParcelStatuses Status { get; set; }
        public CustomerInParcel CustomerInDelivery { get; set; }
        public override string ToString()
        {
            return $"Id #{Id}: Weight = {Weight}, Priority = {Priority}, " +
                   $" Status = {Status}, Customer in delivery = {CustomerInDelivery}";
        }
    }
}

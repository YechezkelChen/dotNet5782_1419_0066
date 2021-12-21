namespace BO
{
    public class StationToList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AvalibleChargeSlots { get; set; }
        public int NotAvailableChargeSlots { get; set; }
        public override string ToString()
        {
            return $"Id #{Id}: Name = {Name}, Available charge slots = {AvalibleChargeSlots}, " +
                    $"Not available charge slots = {NotAvailableChargeSlots}";
        }
    }
}
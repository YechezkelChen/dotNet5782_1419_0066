namespace BO
{
    public class DroneInCharge
    {
        public int Id { get; set; }
        public double Battery { get; set; }

        public override string ToString()
        {
            return
                $"Id #{Id}:" +
                $"Battery = {Battery}\n";
        }
    }
}

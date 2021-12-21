namespace BlApi
{
    public static class BlFactory
    {
        public static IBL GetBl()
        {
            IBL bl = BL.BL.Instance;
            return bl;
        }
    }
}

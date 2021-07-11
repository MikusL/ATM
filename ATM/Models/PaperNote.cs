namespace ATM.Models
{
    public struct PaperNote
    {
        public decimal Weight;
        public decimal Width;
        public decimal Length;
        public string SerialNumber;
        public int Amount;

        public PaperNote(int amount)
        {
            Amount = amount;

            SerialNumber = "";
            Weight = 0;
            Width = 0;
            Length = 0;
        }
    }
}

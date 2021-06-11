namespace Data
{
    public interface IPerson
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string CardNumber { get; set; }
        decimal Balance { get; set; }
    }
}

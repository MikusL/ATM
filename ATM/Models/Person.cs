using ATM.Interfaces;

namespace ATM.Models
{
    public class Person : IPerson
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string CardNumber { get; set; }
        public decimal Balance { get; set; }
    }
}

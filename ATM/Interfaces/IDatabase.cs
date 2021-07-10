using ATM.Models;
using System.Collections.Generic;

namespace ATM.Interfaces
{
    public interface IDatabase
    {
        public IList<Client> ClientList { get; set; }
        public IList<Operator> OperatorList { get; set; }
        public IList<string> CardNumberList { get; set; }
        public IList<IATMachine> AtmList { get; set; }
        public IList<Fee> FeeList { get; set; }
    }
}

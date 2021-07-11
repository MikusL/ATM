using ATM.Interfaces;
using ATM.Models;
using System.Collections.Generic;

namespace ATM.ATM
{
    public class Database : IDatabase
    {
        public IList<Client> ClientList { get; set; }
        public IList<Operator> OperatorList { get; set; }
        public IList<string> CardNumberList { get; set; }
        public IList<IATMachine> AtmList { get; set; }
        public IList<Fee> FeeList { get; set; }

        public Database()
        {
            ClientList = new List<Client>();
            OperatorList = new List<Operator>();
            CardNumberList = new List<string>();
            AtmList = new List<IATMachine>();
            FeeList = new List<Fee>();
        }
    }
}

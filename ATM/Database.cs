using System.Collections.Generic;
using Data;

namespace ATM
{
    public class Database : IDatabase
    {
        public static IList<Client> ClientList { get; set; }
        public static IList<Operator> OperatorList { get; set; }
        public static IList<string> CardNumberList { get; set; }
        public static IList<IAtMachine> AtmList { get; set; }
        public static IList<Fee> FeeList { get; set; }
    }
}

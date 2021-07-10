using System;

namespace ATM.Exceptions
{
    [Serializable]
    public class ClientsBalanceIsNotZero : Exception
    {
        public ClientsBalanceIsNotZero()
        { }

        public ClientsBalanceIsNotZero(string message)
            : base(message)
        { }

        public ClientsBalanceIsNotZero(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}

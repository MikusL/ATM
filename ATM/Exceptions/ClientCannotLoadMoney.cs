using System;

namespace ATM.Exceptions
{
    [Serializable]
    public class ClientCannotLoadMoney : Exception
    {
        public ClientCannotLoadMoney()
        { }

        public ClientCannotLoadMoney(string message)
            : base(message)
        { }

        public ClientCannotLoadMoney(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}

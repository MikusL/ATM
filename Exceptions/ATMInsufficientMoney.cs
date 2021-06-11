using System;

namespace Exceptions
{
    [Serializable]
    public class ATMInsufficientMoney : Exception
    {
        public ATMInsufficientMoney()
        { }

        public ATMInsufficientMoney(string message)
            : base(message)
        { }

        public ATMInsufficientMoney(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}

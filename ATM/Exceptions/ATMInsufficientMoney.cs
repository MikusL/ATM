using System;

namespace ATM.Exceptions
{
    [Serializable]
    public class AtmInsufficientMoney : Exception
    {
        public AtmInsufficientMoney()
        { }

        public AtmInsufficientMoney(string message)
            : base(message)
        { }

        public AtmInsufficientMoney(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}

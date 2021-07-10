using System;

namespace ATM.Exceptions
{
    [Serializable]
    public class InsufficientBalance : Exception
    {
        public InsufficientBalance()
        { }

        public InsufficientBalance(string message)
            : base(message)
        { }

        public InsufficientBalance(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}

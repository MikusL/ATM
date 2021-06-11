using System;

namespace Exceptions
{
    [Serializable]
    public class ATMDoesNotExist : Exception
    {
        public ATMDoesNotExist()
        { }

        public ATMDoesNotExist(string message)
            : base(message)
        { }

        public ATMDoesNotExist(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}

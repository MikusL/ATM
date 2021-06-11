using System;

namespace Exceptions
{
    [Serializable]
    public class ATMAlreadyExists : Exception
    {
        public ATMAlreadyExists()
        { }

        public ATMAlreadyExists(string message)
            : base(message)
        { }

        public ATMAlreadyExists(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}

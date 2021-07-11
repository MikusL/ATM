using System;

namespace ATM.Exceptions
{
    [Serializable]
    public class InvalidCardInserted : Exception
    {
        public InvalidCardInserted()
        { }

        public InvalidCardInserted(string message)
            : base(message)
        { }

        public InvalidCardInserted(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}

using System;

namespace ATM.Exceptions
{
    [Serializable]
    public class AtmAlreadyExists : Exception
    {
        public AtmAlreadyExists()
        { }

        public AtmAlreadyExists(string message)
            : base(message)
        { }

        public AtmAlreadyExists(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}

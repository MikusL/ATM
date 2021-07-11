using System;

namespace ATM.Exceptions
{
    [Serializable]
    public class OperatorAlreadyExists : Exception
    {
        public OperatorAlreadyExists()
        { }

        public OperatorAlreadyExists(string message)
            : base(message)
        { }

        public OperatorAlreadyExists(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}

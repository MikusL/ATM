using System;

namespace ATM.Exceptions
{
    [Serializable]
    public class OperatorDoesNotExist : Exception
    {
        public OperatorDoesNotExist()
        { }

        public OperatorDoesNotExist(string message)
            : base(message)
        { }

        public OperatorDoesNotExist(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}

using System;

namespace ATM.Exceptions
{
    [Serializable]
    public class CurrentClientIsNull : Exception
    {
        public CurrentClientIsNull()
        { }

        public CurrentClientIsNull(string message)
            : base(message)
        { }

        public CurrentClientIsNull(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}

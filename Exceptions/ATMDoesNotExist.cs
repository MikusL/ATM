using System;

namespace Exceptions
{
    [Serializable]
    public class AtmDoesNotExist : Exception
    {
        public AtmDoesNotExist()
        { }

        public AtmDoesNotExist(string message)
            : base(message)
        { }

        public AtmDoesNotExist(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}

using System;

namespace Exceptions
{
    [Serializable]
    public class ClientDoesNotExist : Exception
    {
        public ClientDoesNotExist()
        { }

        public ClientDoesNotExist(string message)
            : base(message)
        { }

        public ClientDoesNotExist(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}

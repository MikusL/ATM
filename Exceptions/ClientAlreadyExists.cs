using System;

namespace Exceptions
{
    [Serializable]
    public class ClientAlreadyExists : Exception
    {
        public ClientAlreadyExists()
        { }

        public ClientAlreadyExists(string message)
            : base(message)
        { }

        public ClientAlreadyExists(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}

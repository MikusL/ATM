using System;

namespace Exceptions
{
    [Serializable]
    public class InvalidAction : Exception
    {
        public InvalidAction()
        { }

        public InvalidAction(string message)
            : base(message)
        { }

        public InvalidAction(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}

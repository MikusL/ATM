using System;

namespace ATM.Exceptions
{
    [Serializable]
    public class InvalidOperatorAction : Exception
    {
        public InvalidOperatorAction()
        { }

        public InvalidOperatorAction(string message)
            : base(message)
        { }

        public InvalidOperatorAction(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}

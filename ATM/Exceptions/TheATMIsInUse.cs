using System;

namespace ATM.Exceptions
{
    [Serializable]
    public class TheATMIsInUse : Exception
    {
        public TheATMIsInUse()
        { }

        public TheATMIsInUse(string message)
            : base(message)
        { }

        public TheATMIsInUse(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}

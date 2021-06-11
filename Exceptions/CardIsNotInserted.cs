using System;

namespace Exceptions
{
    [Serializable]
    public class CardIsNotInserted : Exception
    {
        public CardIsNotInserted()
        { }

        public CardIsNotInserted(string message)
            : base(message)
        { }

        public CardIsNotInserted(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}

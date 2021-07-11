using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exceptions
{
    [Serializable]
    public class InsufficientBalance : Exception
    {
        public InsufficientBalance()
        { }

        public InsufficientBalance(string message)
            : base(message)
        { }

        public InsufficientBalance(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}

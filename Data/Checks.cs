using Exceptions;

namespace Data
{
    public class Checks
    {
        public static void CardInsertedAndCurrentUserChecks(IPerson currentUser, bool cardInserted)
        {
            if (!cardInserted) throw new CardIsNotInserted("There is no card inserted into the ATM");
            if (currentUser == null) throw new CurrentClientIsNull("Current client is null");
        }
    }
}

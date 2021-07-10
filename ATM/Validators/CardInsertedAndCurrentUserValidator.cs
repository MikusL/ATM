using ATM.Exceptions;
using ATM.Interfaces;

namespace ATM.Validators
{
    public static class CardInsertedAndCurrentUserValidator
    {
        public static void Validate(IPerson currentUser, bool cardInserted)
        {
            if (!cardInserted) throw new CardIsNotInserted("There is no card inserted into the ATM");
            if (currentUser == null) throw new CurrentClientIsNull("Current client is null");
        }
    }
}

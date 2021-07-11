using ATM.Exceptions;
using ATM.Interfaces;

namespace ATM.Validators
{
    public static class CurrentUserBalanceValidator
    {
        public static void Validate(IPerson currentUser, int amount)
        {
            if (currentUser.Balance < amount)
                throw new InsufficientBalance("The clients balance is too low");
        }
    }
}

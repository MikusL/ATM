using ATM.Exceptions;
using ATM.Models;

namespace ATM.Validators
{
    public static class AtmTotalMoneyValidator
    {
        public static void Validate(Money totalMoney, int amount)
        {
            if (totalMoney.Amount < amount)
                throw new AtmInsufficientMoney("The ATM does not have enough money to withdraw the given amount");
        }
    }
}

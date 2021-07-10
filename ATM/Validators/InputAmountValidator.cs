using ATM.Exceptions;

namespace ATM.Validators
{
    public static class InputAmountValidator
    {
        public static void Validate(int amount)
        {
            if (amount % 5 != 0 || amount <= 0)
                throw new InvalidInput("Invalid amount input");
        }
    }
}

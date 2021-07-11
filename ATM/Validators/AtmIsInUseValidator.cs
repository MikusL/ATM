using ATM.Exceptions;

namespace ATM.Validators
{
    public static class AtmIsInUseValidator
    {
        public static void Validator(bool cardInserted)
        {
            if (cardInserted) throw new TheATMIsInUse("There is a card already inserted");
        }
    }
}

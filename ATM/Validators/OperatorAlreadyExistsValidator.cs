using ATM.Exceptions;
using ATM.Interfaces;
using System.Linq;

namespace ATM.Validators
{
    public static class OperatorAlreadyExistsValidator
    {
        public static void Validate(string name, string surname, IDatabase database)
        {
            if (database.OperatorList.FirstOrDefault(x => x.Name == name && x.Surname == surname) != null)
                throw new OperatorAlreadyExists("That operator already exists in our database");
        }
    }
}

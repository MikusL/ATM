using ATM.Exceptions;
using ATM.Interfaces;
using System.Linq;

namespace ATM.Validators
{
    public static class AtmAlreadyExistsValidator
    {
        public static void Validate(string serialNumber, IDatabase database)
        {
            if (database.AtmList.FirstOrDefault(x => x.SerialNumber == serialNumber) != null)
                throw new AtmAlreadyExists("That ATM already exists in our database");
        }
    }
}

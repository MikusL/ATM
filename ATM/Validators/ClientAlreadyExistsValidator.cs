using ATM.Exceptions;
using ATM.Interfaces;
using System.Linq;

namespace ATM.Validators
{
    public static class ClientAlreadyExistsValidator
    {
        public static void Validate(string name, string surname, IDatabase database)
        {
            if (database.ClientList.FirstOrDefault(x => x.Name == name && x.Surname == surname) != null)
                throw new ClientAlreadyExists("That client already exists in our database");
        }
    }
}

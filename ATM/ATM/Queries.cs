using ATM.Exceptions;
using ATM.Interfaces;
using ATM.Models;
using System.Linq;

namespace ATM.ATM
{
    public static class Queries
    {
        public static ATMachine GetAtmBySerialNumber(string serialNumber, IDatabase database)
        {
            var atm = database.AtmList.FirstOrDefault(x => x.SerialNumber == serialNumber);

            if (atm == null) throw new AtmDoesNotExist("There is no ATMachine with that serial number");

            return (ATMachine)atm;
        }

        public static Operator GetOperatorByCardNumber(string cardNumber, IDatabase database)
        {
            var operatorObject = database.OperatorList.FirstOrDefault(x => x.CardNumber == cardNumber);

            if (operatorObject == null) throw new OperatorDoesNotExist("There is no operator with that card number");

            return operatorObject;
        }

        public static Operator GetOperatorByEmployeeId(int employeeId, IDatabase database)
        {
            var operatorObject = database.OperatorList.FirstOrDefault(x => x.EmployeeId == employeeId);

            if (operatorObject == null) throw new OperatorDoesNotExist("There is no operator with that employee id");

            return operatorObject;
        }

        public static Operator GetOperatorByFullName(string name, string surname, IDatabase database)
        {
            var operatorObject = database.OperatorList.FirstOrDefault(x => x.Name == name && x.Surname == surname);

            if (operatorObject == null) throw new OperatorDoesNotExist("There is no operator with that name and surname");

            return operatorObject;
        }

        public static Client GetClientById(int id, IDatabase database)
        {
            var client = database.ClientList.FirstOrDefault(x => x.Id == id);

            if (client == null) throw new ClientDoesNotExist("There is no client with that id");

            return client;
        }

        public static Client GetClientByCardNumber(string cardNumber, IDatabase database)
        {
            var client = database.ClientList.FirstOrDefault(x => x.CardNumber == cardNumber);

            if (client == null) throw new ClientDoesNotExist("There is no client with that card number");

            return client;
        }

        public static Client GetClientByFullName(string name, string surname, IDatabase database)
        {
            var client = database.ClientList.FirstOrDefault(x => x.Name == name && x.Surname == surname);

            if (client == null) throw new ClientDoesNotExist("There is no client with that name and surname");

            return client;
        }
    }
}

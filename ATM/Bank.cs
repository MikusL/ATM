using System;
using System.Linq;
using Data;
using Exceptions;

namespace ATM
{
    public class Bank
    {
        public static string ClientPrefix;
        public static string OperatorPrefix;
        public string Name;
        private Money _totalMoney;
        private int _clientId;
        private int _employeeId;
        

        public Bank(string clientPrefix,string operatorPrefix, Money totalMoney, string bankName)
        {
            ClientPrefix = clientPrefix;
            OperatorPrefix = operatorPrefix;
            _totalMoney = totalMoney;
            Name = bankName;
        }

        public void AddAtMachine(string manufacturer, string serialNumber)
        {
            if (Database.AtmList.FirstOrDefault(x => x.SerialNumber == serialNumber) != null)
            {
                throw new AtmAlreadyExists("That ATM already exists in our database");
            }

            Database.AtmList.Add(new AtMachine(manufacturer, serialNumber));
        }

        public void RemoveAtMachine(string serialNumber)
        {
            var atm = Database.AtmList.FirstOrDefault(x => x.SerialNumber == serialNumber);

            if (atm == null) throw new AtmDoesNotExist("There is no ATM with that serial number");

            Database.AtmList.Remove(atm);
        }

        public void AddOperator(string name, string surname)
        {
            if (Database.OperatorList.FirstOrDefault(x => x.Name == name && x.Surname == surname) != null) 
                throw new OperatorAlreadyExists("That operator already exists in our database");

            Database.OperatorList.Add(new Operator(name, surname, ++_employeeId, OperatorPrefix, Database.CardNumberList));
        }

        public void RemoveOperator(int employeeId)
        {
            var operatorObject = Database.OperatorList.FirstOrDefault(x => x.EmployeeId == employeeId);

            if (operatorObject == null) throw new OperatorDoesNotExist("There is no operator with that employee ID");

            Database.OperatorList.Remove(operatorObject);
        }

        public void AddClient(string name, string surname, DateTime doB)
        {
            if (Database.ClientList.FirstOrDefault(x => x.Name == name && x.Surname == surname) != null)
            {
                throw new ClientAlreadyExists("That client already exists in our database");
            }

            Database.ClientList.Add(new Client(doB, name, surname, ++_clientId, ClientPrefix, Database.CardNumberList));
        }

        public void DeleteClient(int clientId)
        {
            if (GetClientById(clientId).Balance != 0) throw new InvalidAction("The clients balance is not zero");

            Database.ClientList.Remove(GetClientById(clientId));
        }

        public Money ClientWithdrawsMoney(int amount, int clientId)
        {
            var client = GetClientById(clientId);

            if (client.Balance < amount) throw new InsufficientBalance("The client does not have enough money");

            client.Balance -= amount;
            _totalMoney.Amount -= amount;
            Money money = new Money(amount);

            foreach (var kvp in money.Notes.Reverse())
            {
                if (amount == 0) break;

                while (amount >= kvp.Key.Amount)
                {
                    if (_totalMoney.Notes[kvp.Key] > 0)
                    {
                        amount -= kvp.Key.Amount;
                        money.Notes[kvp.Key] += 1;
                        _totalMoney.Notes[kvp.Key] -= 1;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return money;
        }

        public void ClientDepositsMoney(Money amount, int clientId)
        {
            var client = GetClientById(clientId);
            client.Balance += amount.Amount;
            _totalMoney.Amount += amount.Amount;

            foreach (var kvp in amount.Notes)
            {
                _totalMoney.Notes[kvp.Key] += amount.Notes[kvp.Key];
            }
        }

        public decimal ClientBalance(int clientId)
        {
            var client = GetClientById(clientId);
            return client.Balance;
        }

        public static AtMachine GetAtmBySerialNumber(string serialNumber)
        {
            var atm = Database.AtmList.FirstOrDefault(x => x.SerialNumber == serialNumber);

            if (atm == null) throw new AtmDoesNotExist("There is no ATMachine with that serial number");

            return (AtMachine) atm;
        }

        public static Operator GetOperatorByCardNumber(string cardNumber)
        {
            var operatorObject = Database.OperatorList.FirstOrDefault(x => x.CardNumber == cardNumber);

            if (operatorObject == null) throw new OperatorDoesNotExist("There is no operator with that card number");
            
            return operatorObject;
        }

        public static Operator GetOperatorByEmployeeId(int employeeId)
        {
            var operatorObject = Database.OperatorList.FirstOrDefault(x => x.EmployeeId == employeeId);

            if (operatorObject == null) throw new OperatorDoesNotExist("There is no operator with that employee id");

            return operatorObject;
        }

        public static Operator GetOperatorByFullName(string name, string surname)
        {
            var operatorObject = Database.OperatorList.FirstOrDefault(x => x.Name == name && x.Surname == surname);

            if (operatorObject == null) throw new OperatorDoesNotExist("There is no operator with that name and surname");

            return operatorObject;
        }

        public static Client GetClientById(int id)
        {
            var client = Database.ClientList.FirstOrDefault(x => x.Id == id);

            if (client == null) throw new ClientDoesNotExist("There is no client with that id");

            return client;
        }

        public static Client GetClientByCardNumber(string cardNumber)
        {
            var client = Database.ClientList.FirstOrDefault(x => x.CardNumber == cardNumber);

            if (client == null) throw new ClientDoesNotExist("There is no client with that card number");
            
            return client;
        }

        public static Client GetClientByFullName(string name, string surname)
        {
            var client = Database.ClientList.FirstOrDefault(x => x.Name == name && x.Surname == surname);

            if (client == null) throw new ClientDoesNotExist("There is no client with that name and surname");

            return client;
        }
    }
}

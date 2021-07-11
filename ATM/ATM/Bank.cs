using ATM.Exceptions;
using ATM.Interfaces;
using ATM.Models;
using ATM.Validators;
using System;
using System.Linq;

namespace ATM.ATM
{
    public class Bank : IBank
    {
        public string ClientPrefix { get; set; }
        public string OperatorPrefix { get; set; }
        public string Name { get; set; }
        private Money _totalMoney;
        private int _clientId;
        private int _employeeId;
        private IDatabase _database;


        public Bank(string clientPrefix, string operatorPrefix, Money totalMoney, string bankName, IDatabase database)
        {
            ClientPrefix = clientPrefix;
            OperatorPrefix = operatorPrefix;
            _totalMoney = totalMoney;
            _database = database;
            Name = bankName;
        }

        public void AddATMachine(string manufacturer, string serialNumber)
        {
            AtmAlreadyExistsValidator.Validate(serialNumber, _database);
            _database.AtmList.Add(new ATMachine(manufacturer, serialNumber, ClientPrefix, OperatorPrefix, _database));
        }

        public void RemoveATMachine(string serialNumber)
        {
            var atm = Queries.GetAtmBySerialNumber(serialNumber, _database);
            _database.AtmList.Remove(atm);
        }

        public void AddOperator(string name, string surname)
        {
            OperatorAlreadyExistsValidator.Validate(name, surname, _database);
            _database.OperatorList.Add(new Operator(name, surname, ++_employeeId, OperatorPrefix, _database.CardNumberList));
        }

        public void RemoveOperator(int employeeId)
        {
            var operatorObject = Queries.GetOperatorByEmployeeId(employeeId, _database);
            _database.OperatorList.Remove(operatorObject);
        }

        public void AddClient(string name, string surname, DateTime doB)
        {
            ClientAlreadyExistsValidator.Validate(name, surname, _database);
            _database.ClientList.Add(new Client(doB, name, surname, ++_clientId, ClientPrefix, _database.CardNumberList));
        }

        public void RemoveClient(int clientId)
        {
            if (Queries.GetClientById(clientId, _database).Balance != 0)
                throw new ClientsBalanceIsNotZero("The clients balance is not zero");

            _database.ClientList.Remove(Queries.GetClientById(clientId, _database));
        }

        public Money ClientWithdrawsMoney(int amount, int clientId)
        {
            var client = Queries.GetClientById(clientId, _database);

            if (client.Balance < amount)
                throw new InsufficientBalance("The client does not have enough money");

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
            if (amount.Amount <= 0) throw new InvalidInput("Client cannot deposit 0 or less");

            var client = Queries.GetClientById(clientId, _database);
            client.Balance += amount.Amount;
            _totalMoney.Amount += amount.Amount;

            foreach (var kvp in amount.Notes)
            {
                _totalMoney.Notes[kvp.Key] += amount.Notes[kvp.Key];
            }
        }

        public decimal ClientBalance(int clientId)
        {
            var client = Queries.GetClientById(clientId, _database);
            return client.Balance;
        }
    }
}

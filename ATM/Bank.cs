using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using Exceptions;

namespace ATM
{
    public class Bank
    {
        public static IList<Client> ClientList { get; set; }
        public static IList<Operator> OperatorList { get; set; }
        public static IList<string> CardNumberList { get; set; }
        public static IList<ATMachine> ATMList { get; set; }
        public static IList<Fee> FeeList { get; set; }
        public static string ClientPrefix;
        public static string OperatorPrefix;
        public string Name;
        private Money _totalMoney;
        private int _clientID;
        private int _employeeID;
        

        public Bank(string clientPrefix,string operatorPrefix, Money totalMoney, string bankName)
        {
            ClientPrefix = clientPrefix;
            OperatorPrefix = operatorPrefix;
            _totalMoney = totalMoney;
            Name = bankName;
            FeeList = new List<Fee>();
            ClientList = new List<Client>();
            ATMList = new List<ATMachine>();
            OperatorList = new List<Operator>();
            CardNumberList = new List<string>();
        }

        public void AddATMachine(string manufacturer, string serialNumber)
        {
            if (ATMList.FirstOrDefault(x => x.SerialNumber == serialNumber) != null)
            {
                throw new ATMAlreadyExists("That ATM already exists in our database");
            }

            ATMList.Add(new ATMachine(manufacturer, serialNumber));
        }

        public void RemoveATMachine(string serialNumber)
        {
            var ATM = ATMList.FirstOrDefault(x => x.SerialNumber == serialNumber);

            if (ATM == null) throw new ATMDoesNotExist("There is no ATM with that serial number");

            ATMList.Remove(ATM);
        }

        public void AddOperator(string name, string surname)
        {
            if (OperatorList.FirstOrDefault(x => x.Name == name && x.Surname == surname) != null)
            {
                throw new OperatorAlreadyExists("That operator already exists in our database");
            }

            OperatorList.Add(new Operator(name, surname, ++_employeeID, OperatorPrefix));
        }

        public void RemoveOperator(int employeeID)
        {
            var operatorObject = OperatorList.FirstOrDefault(x => x.EmployeeID == employeeID);

            if (operatorObject == null) throw new OperatorDoesNotExist("There is no operator with that employee ID");

            OperatorList.Remove(operatorObject);
        }

        public void AddClient(string name, string surname, DateTime DoB)
        {
            if (ClientList.FirstOrDefault(x => x.Name == name && x.Surname == surname) != null)
            {
                throw new ClientAlreadyExists("That client already exists in our database");
            }

            ClientList.Add(new Client(DoB, name, surname, ++_clientID, ClientPrefix));
        }

        public void DeleteClient(int clientID)
        {
            if (GetClientByID(clientID).Balance != 0) throw new InvalidAction("The clients balance is not zero");
           
            ClientList.Remove(GetClientByID(clientID));
        }

        public Money ClientWithdrawsMoney(int amount, int clientID)
        {
            var client = GetClientByID(clientID);

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

        public void ClientDepositsMoney(Money amount, int clientID)
        {
            var client = GetClientByID(clientID);
            client.Balance += amount.Amount;
            _totalMoney.Amount += amount.Amount;

            foreach (var kvp in amount.Notes)
            {
                _totalMoney.Notes[kvp.Key] += amount.Notes[kvp.Key];
            }
        }

        public decimal ClientBalance(int clientID)
        {
            var client = GetClientByID(clientID);
            return client.Balance;
        }

        public static ATMachine GetATMBySerialNumber(string serialNumber)
        {
            var ATM = ATMList.FirstOrDefault(x => x.SerialNumber == serialNumber);

            if (ATM == null) throw new ATMDoesNotExist("There is no ATMachine with that serial number");

            return ATM;
        }

        public static Operator GetOperatorByCardNumber(string cardNumber)
        {
            var operatorObject = OperatorList.FirstOrDefault(x => x.CardNumber == cardNumber);

            if (operatorObject == null) throw new OperatorDoesNotExist("There is no operator with that card number");
            
            return operatorObject;
        }

        public static Operator GetOperatorByEmployeeID(int employeeID)
        {
            var operatorObject = OperatorList.FirstOrDefault(x => x.EmployeeID == employeeID);

            if (operatorObject == null) throw new OperatorDoesNotExist("There is no operator with that employee id");

            return operatorObject;
        }

        public static Operator GetOperatorByFullName(string name, string surname)
        {
            var operatorObject = OperatorList.FirstOrDefault(x => x.Name == name && x.Surname == surname);

            if (operatorObject == null) throw new OperatorDoesNotExist("There is no operator with that name and surname");

            return operatorObject;
        }

        public static Client GetClientByID(int id)
        {
            var client = ClientList.FirstOrDefault(x => x.ID == id);

            if (client == null) throw new ClientDoesNotExist("There is no client with that id");

            return client;
        }

        public static Client GetClientByCardNumber(string cardNumber)
        {
            var client = ClientList.FirstOrDefault(x => x.CardNumber == cardNumber);

            if (client == null) throw new ClientDoesNotExist("There is no client with that card number");
            
            return client;
        }

        public static Client GetClientByFullName(string name, string surname)
        {
            var client = ClientList.FirstOrDefault(x => x.Name == name && x.Surname == surname);

            if (client == null) throw new ClientDoesNotExist("There is no client with that name and surname");

            return client;
        }
    }
}

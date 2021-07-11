using ATM.Models;
using System;

namespace ATM.Interfaces
{
    public interface IBank
    {
        public string Name { get; set; }
        public string ClientPrefix { get; set; }
        public string OperatorPrefix { get; set; }

        public void AddATMachine(string manufacturer, string serialNumber);

        public void RemoveATMachine(string serialNumber);

        public void AddOperator(string name, string surname);

        public void RemoveOperator(int employeeId);

        public void AddClient(string name, string surname, DateTime doB);

        public void RemoveClient(int clientId);

        public Money ClientWithdrawsMoney(int amount, int clientId);

        public void ClientDepositsMoney(Money amount, int clientId);

        public decimal ClientBalance(int clientId);
    }
}

using ATM.ATM;
using ATM.Exceptions;
using ATM.Interfaces;
using ATM.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;

namespace ATM.Tests
{
    [TestClass]
    public class BankTests
    {
        private IBank _Bank;
        private IDatabase _database;

        [TestInitialize]
        public void Initialize()
        {
            Money money = new Money(85000);
            money = CycleThroughMoneyNotesAndSetValue(money, 1000);
            _database = new Database();
            _Bank = new Bank("4444", "3333", money, "Bank", _database);
        }

        [TestMethod]
        public void AddATMachine_ValidATM_SuccessfullyAddsATM()
        {
            // Act
            _Bank.AddATMachine("Manufacturer", "Serial-Number");
        }

        [TestMethod]
        public void AddATMachine_AddingTheSameATM_ThrowsExceptionAtmAlreadyExists()
        {
            // Act
            _Bank.AddATMachine("Manufacturer", "Serial-Number");
            // Assert
            Assert.ThrowsException<AtmAlreadyExists>(() => _Bank.AddATMachine("Manufacturer", "Serial-Number"));
        }

        [TestMethod]
        public void RemoveATM_ValidATM_RemovesSuccessfully()
        {
            // Act
            _Bank.AddATMachine("Manufacturer", "Serial-Number");
            _Bank.RemoveATMachine("Serial-Number");
        }

        [TestMethod]
        public void RemoveATM_InvalidATM_ThrowsExceptionAtmDoesNotExist()
        {
            // Assert
            Assert.ThrowsException<AtmDoesNotExist>(() => _Bank.RemoveATMachine("SerialNumber"));
        }

        [TestMethod]
        public void AddOperator_ValidOperator_SuccessfullyAddsOperator()
        {
            // Act
            _Bank.AddOperator("Name", "Surname");
        }

        [TestMethod]
        public void AddOperator_SameOperator_ThrowsExceptionOperatorAlreadyExists()
        {
            // Act
            _Bank.AddOperator("Name", "Surname");
            // Assert
            Assert.ThrowsException<OperatorAlreadyExists>(() => _Bank.AddOperator("Name", "Surname"));
        }

        [TestMethod]
        public void RemoveOperator_ValidOperator_SuccessfullyRemovesOperator()
        {
            // Act
            _Bank.AddOperator("Name", "Surname");
            _Bank.RemoveOperator(Queries.GetOperatorByFullName("Name", "Surname", _database).EmployeeId);
        }

        [TestMethod]
        public void RemoveOperator_InvalidOperatorName_ThrowsExceptionOperatorDoesNotExist()
        {
            // Assert
            Assert.ThrowsException<OperatorDoesNotExist>(() =>
                _Bank.RemoveOperator(Queries.GetOperatorByFullName("Name", "Surname", _database).EmployeeId));
        }

        [TestMethod]
        public void RemoveOperator_InvalidOperator_ThrowsExceptionOperatorDoesNotExist()
        {
            // Assert
            Assert.ThrowsException<OperatorDoesNotExist>(() => _Bank.RemoveOperator(5));
        }

        [TestMethod]
        public void AddClient_ValidClient_SuccessfullyAddsClient()
        {
            // Act
            _Bank.AddClient("Name", "Surname", DateTime.Now);
        }

        [TestMethod]
        public void AddClient_InvalidClient_ThrowsExceptionClientAlreadyExists()
        {
            // Act 
            _Bank.AddClient("Name", "Surname", DateTime.Now);
            // Assert
            Assert.ThrowsException<ClientAlreadyExists>(() => _Bank.AddClient("Name", "Surname", DateTime.Now));
        }

        [TestMethod]
        public void ClientBalance_Returns()
        {
            // Act
            _Bank.AddClient("Name", "Surname", DateTime.Now);
            // Assert
            Assert.AreEqual(0, _Bank.ClientBalance(Queries.GetClientByFullName("Name", "Surname", _database).Id));
        }

        [TestMethod]
        public void ClientWithdrawsMoney_ClientWithdraws85_SuccessfullyWithdraws()
        {
            // Arrange
            var target = new Money(85);
            target = CycleThroughMoneyNotesAndSetValue(target, 1);
            // Act
            _Bank.AddClient("Name", "Surname", DateTime.Now);
            Queries.GetClientByFullName("Name", "Surname", _database).Balance = 500;
            // Assert
            Assert.AreEqual(JsonConvert.SerializeObject(target),
                JsonConvert.SerializeObject(_Bank.ClientWithdrawsMoney(85, Queries.GetClientByFullName("Name", "Surname", _database).Id)));
        }

        [TestMethod]
        public void ClientWithdrawsMoney_ClientWithdraws600_SuccessfullyWithdraws()
        {
            // Arrange
            Money target = new Money(50060);
            target.Notes[new PaperNote(50)] = 1000;
            target.Notes[new PaperNote(20)] = 3;
            // Act
            _Bank.AddClient("Name", "Surname", DateTime.Now);
            Queries.GetClientByFullName("Name", "Surname", _database).Balance = 500000;
            // Assert
            Assert.AreEqual(JsonConvert.SerializeObject(target),
                JsonConvert.SerializeObject(_Bank.ClientWithdrawsMoney(50060, Queries.GetClientByFullName("Name", "Surname", _database).Id)));
        }

        [TestMethod]
        public void ClientWithdrawsMoney_BalanceTooLittle_ThrowsExceptionInsufficientBalance()
        {
            // Act
            _Bank.AddClient("Name", "Surname", DateTime.Now);
            // Assert
            Assert.ThrowsException<InsufficientBalance>(() =>
                _Bank.ClientWithdrawsMoney(10000, Queries.GetClientByFullName("Name", "Surname", _database).Id));
        }

        [TestMethod]
        public void ClientDepositsMoney_ClientDeposits85_SuccessfullyDeposits()
        {
            // Arrange
            var money = new Money(85);
            money = CycleThroughMoneyNotesAndSetValue(money, 1);
            // Act
            _Bank.AddClient("Name", "Surname", DateTime.Now);
            _Bank.ClientDepositsMoney(money, Queries.GetClientByFullName("Name", "Surname", _database).Id);
        }

        [TestMethod]
        public void ClientDepositsMoney_NegativeAmount_ThrowsExceptionInvalidInput()
        {
            // Act
            _Bank.AddClient("Name", "Surname", DateTime.Now);
            // Assert
            Assert.ThrowsException<InvalidInput>(() => _Bank.ClientDepositsMoney(new Money(-10), Queries.GetClientByFullName("Name", "Surname", _database).Id));
        }

        [TestMethod]
        public void RemoveClient_InvalidFullName_ThrowsExceptionClientDoesNotExist()
        {
            // Assert
            Assert.ThrowsException<ClientDoesNotExist>(() =>
                _Bank.RemoveClient(Queries.GetClientByFullName("Wrong", "Name", _database).Id));
        }

        [TestMethod]
        public void RemoveClient_BalanceIsNotZero_ThrowsException()
        {
            // Act
            _Bank.AddClient("Name", "Surname", DateTime.Now);
            Queries.GetClientByFullName("Name", "Surname", _database).Balance = 500;
            // Assert
            Assert.ThrowsException<ClientsBalanceIsNotZero>(() =>
                _Bank.RemoveClient(Queries.GetClientByFullName("Name", "Surname", _database).Id));
        }

        [TestMethod]
        public void RemoveClient_ValidClient_SuccessfullyRemovesClient()
        {
            // Act
            _Bank.AddClient("Name", "Surname", DateTime.Now);
            // Arrange
            var client = Queries.GetClientByFullName("Name", "Surname", _database);
            client.Balance = 0;
            // Act
            _Bank.RemoveClient(Queries.GetClientByFullName("Name", "Surname", _database).Id);
        }

        [TestMethod]
        public void RemoveClient_InvalidID_ThrowsExceptionClientDoesNotExist()
        {
            // Assert
            Assert.ThrowsException<ClientDoesNotExist>(() => _Bank.RemoveClient(15));
        }

        [TestMethod]
        public void BankNameCheck()
        {
            // Assert
            Assert.AreEqual("Bank", _Bank.Name);
        }

        private static Money CycleThroughMoneyNotesAndSetValue(Money money, int value)
        {
            foreach (var note in money.Notes)
            {
                money.Notes[note.Key] = value;
            }

            return money;
        }
    }
}

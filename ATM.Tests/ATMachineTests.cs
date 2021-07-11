using ATM.ATM;
using ATM.Exceptions;
using ATM.Interfaces;
using ATM.Models;
using ATM.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ATM.Tests
{
    [TestClass]
    public class ATMachineTests
    {
        private IATMachine _ATMachine;
        private IDatabase _database;
        private string operatorCardNumber = "333311112222";
        private string clientCardNumber = "4444111122223333";

        [TestInitialize]
        public void Initialize()
        {
            _database = new Database();
            _ATMachine = new ATMachine("Manufacturer", "Serial-Number", "3333", "4444", _database);

            var client = new Client(DateTime.Now, "Name", "Surname", 1, "4444", _database.CardNumberList);
            client.CardNumber = clientCardNumber;
            client.Balance = 5000;
            _database.ClientList.Add(client);

            var operatorObj = new Operator("OperatorName", "OperatorSurname", 1, "3333", _database.CardNumberList);
            operatorObj.CardNumber = operatorCardNumber;
            _database.OperatorList.Add(operatorObj);
        }

        [TestMethod]
        public void InsertCard_InsertClientAndOperatorCards_ThrowsExceptionTheATMIsInUse()
        {
            // Act
            _ATMachine.InsertCard(clientCardNumber);
            // Assert
            Assert.ThrowsException<TheATMIsInUse>(() => _ATMachine.InsertCard(operatorCardNumber));
        }

        [TestMethod]
        public void InsertCard_InsertOperatorAndClientCards_ThrowsExceptionTheATMIsInUse()
        {
            // Act
            _ATMachine.InsertCard(operatorCardNumber);
            // Assert
            Assert.ThrowsException<TheATMIsInUse>(() => _ATMachine.InsertCard(clientCardNumber));
        }

        [TestMethod]
        public void InsertCard_InvalidCardInserted_ThrowsExceptionInvalidCardInserted()
        {
            // Assert
            Assert.ThrowsException<InvalidCardInserted>(() => _ATMachine.InsertCard("123456789101"));
        }

        [TestMethod]
        public void InsertCard_InvalidOperatorCardInserted_ThrowsExceptionOperatorDoesNotExist()
        {
            // Assert
            Assert.ThrowsException<OperatorDoesNotExist>(() => _ATMachine.InsertCard("333312341234"));
        }

        [TestMethod]
        public void InsertCard_InvalidClientCardInserted_ThrowsExceptionClientDoesNotExist()
        {
            // Assert
            Assert.ThrowsException<ClientDoesNotExist>(() => _ATMachine.InsertCard("4444123412341234"));
        }

        [TestMethod]
        public void GetBalance_InsertOperatorCard_ThrowsExceptionInvalidOperatorAction()
        {
            // Act
            _ATMachine.InsertCard(operatorCardNumber);
            // Assert
            Assert.ThrowsException<InvalidOperatorAction>(() => _ATMachine.GetCardBalance());
        }

        [TestMethod]
        public void GetBalance_InsertClientCard_Returns0()
        {
            // Act
            _ATMachine.InsertCard(clientCardNumber);
            // Assert
            Assert.AreEqual(5000, _ATMachine.GetCardBalance());
        }

        [TestMethod]
        public void WithdrawMoney_ClientWithdraws85_ReturnsCorrectMoneyStruct()
        {
            // Arrange
            var target = new Money(85);
            target = CycleThroughMoneyNotesAndSetValue(target, 1);
            LoadATMMoney(8500, 100);
            // Act
            _ATMachine.InsertCard(clientCardNumber);
            // Assert
            Assert.AreEqual(JsonConvert.SerializeObject(target), JsonConvert.SerializeObject(_ATMachine.WithdrawMoney(85)));
        }

        [TestMethod]
        public void WithdrawMoney_CardIsNotInserted_ThrowsExceptionCardIsNotInserted()
        {
            // Assert
            Assert.ThrowsException<CardIsNotInserted>(() => _ATMachine.WithdrawMoney(85));
        }

        [TestMethod]
        public void WithdrawMoney_CurrentClientIsNullCardIsInserted_ThrowsExceptionCurrentClientIsNull()
        {
            // Assert
            Assert.ThrowsException<CurrentClientIsNull>(() => CardInsertedAndCurrentUserValidator.Validate(null, true));
        }

        [TestMethod]
        public void WithdrawMoney_OperatorWithdraws_ThrowsExceptionInvalidOperatorException()
        {
            // Act
            _ATMachine.InsertCard(operatorCardNumber);
            // Assert
            Assert.ThrowsException<InvalidOperatorAction>(() => _ATMachine.WithdrawMoney(10));
        }

        [TestMethod]
        public void WithdrawMoney_ClientEntersInvalidNumber_ThrowsExceptionInvalidInput()
        {
            // Act
            _ATMachine.InsertCard(clientCardNumber);
            // Assert
            Assert.ThrowsException<InvalidInput>(() => _ATMachine.WithdrawMoney(13));
        }

        [TestMethod]
        public void WithdrawMoney_ClientHasInsufficientBalance_ThrowsExceptionInsufficientBalance()
        {
            // Act
            _ATMachine.InsertCard(clientCardNumber);
            // Assert
            Assert.ThrowsException<InsufficientBalance>(() => _ATMachine.WithdrawMoney(6000));
        }

        [TestMethod]
        public void WithdrawMoney_ATMHasInsuffcientMoney_ThrowsExceptionInsufficientMoney()
        {
            // Act
            LoadATMMoney(850, 10);
            _ATMachine.InsertCard(clientCardNumber);
            // Assert
            Assert.ThrowsException<AtmInsufficientMoney>(() => _ATMachine.WithdrawMoney(1000));
        }

        [TestMethod]
        public void WithdrawMoney_Loads850Withdraws600_ReturnsCorrectMoney()
        {
            // Act
            Money target = new Money(600);
            target.Notes[new PaperNote(50)] = 10;
            target.Notes[new PaperNote(20)] = 5;
            LoadATMMoney(850, 10);
            _ATMachine.InsertCard(clientCardNumber);
            // Assert
            Assert.AreEqual(JsonConvert.SerializeObject(target), JsonConvert.SerializeObject(_ATMachine.WithdrawMoney(600)));
        }

        [TestMethod]
        public void LoadMoney_ClientCard_ThrowsExceptionClientCannotLoadMoney()
        {
            // Act
            _ATMachine.InsertCard(clientCardNumber);
            // Assert
            Assert.ThrowsException<ClientCannotLoadMoney>(() => _ATMachine.LoadMoney(new Money(100)));
        }

        [TestMethod]
        public void RetrieveChargedFees_OperatorCard_ThrowsExceptionInvalidOperatorAction()
        {
            // Act
            _ATMachine.InsertCard(operatorCardNumber);
            // Assert
            Assert.ThrowsException<InvalidOperatorAction>(() => _ATMachine.RetrieveChargedFees());
        }

        [TestMethod]
        public void RetrieveChargedFees_ClientCard_ReturnsCorrectList()
        {
            // Arrange
            List<Fee> target = new List<Fee> { new Fee(0.85m, clientCardNumber) };
            // Act
            LoadATMMoney(850, 10);
            _ATMachine.InsertCard(clientCardNumber);
            _ATMachine.WithdrawMoney(85);
            // Arrange
            Assert.AreEqual(JsonConvert.SerializeObject(target), JsonConvert.SerializeObject(_ATMachine.RetrieveChargedFees()));
        }

        [TestMethod]
        public void ManufacturerPropertyTest()
        {
            // Assert
            Assert.AreEqual("Manufacturer", _ATMachine.Manufacturer);
        }

        [TestMethod]
        public void SerialNumberPropertyTest()
        {
            // Assert
            Assert.AreEqual("Serial-Number", _ATMachine.SerialNumber);
        }

        private void LoadATMMoney(int totalAmount, int eachNoteAmount)
        {
            _ATMachine.InsertCard(operatorCardNumber);
            Money money = new Money(totalAmount);
            money = CycleThroughMoneyNotesAndSetValue(money, eachNoteAmount);
            _ATMachine.LoadMoney(money);
            _ATMachine.ReturnCard();
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

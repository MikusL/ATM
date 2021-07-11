using ATM.Exceptions;
using ATM.Interfaces;
using ATM.Models;
using ATM.Validators;
using System.Collections.Generic;
using System.Linq;

namespace ATM.ATM
{
    public class ATMachine : IATMachine
    {
        private Money _totalMoney;
        private bool _cardInserted;
        private IPerson _currentUser;
        private readonly string _operatorPrefix;
        private readonly string _clientPrefix;
        private readonly IDatabase _database;
        public string Manufacturer { get; }
        public string SerialNumber { get; }

        public ATMachine(string manufacturer, string serialNumber, string operatorPrefix, string clientPrefix, IDatabase database)
        {
            _database = database;
            Manufacturer = manufacturer;
            SerialNumber = serialNumber;
            _totalMoney = new Money(0);
            _operatorPrefix = operatorPrefix;
            _clientPrefix = clientPrefix;
        }

        public void InsertCard(string cardNumber)
        {
            AtmIsInUseValidator.Validator(_cardInserted);

            var firstNumberSequence = cardNumber[..4];

            if (firstNumberSequence == _operatorPrefix && cardNumber.Length == 12)
            {
                _currentUser = Queries.GetOperatorByCardNumber(cardNumber, _database);
            }
            else if (firstNumberSequence == _clientPrefix && cardNumber.Length == 16)
            {
                _currentUser = Queries.GetClientByCardNumber(cardNumber, _database);
            }
            else
            {
                throw new InvalidCardInserted("This card is not from our bank");
            }

            _cardInserted = true;
        }

        public decimal GetCardBalance()
        {
            CardInsertedAndCurrentUserValidator.Validate(_currentUser, _cardInserted);
            if (_currentUser.GetType() == typeof(Operator))
                throw new InvalidOperatorAction("An operator does not have a balance");

            return _currentUser.Balance;
        }

        public Money WithdrawMoney(int amount)
        {
            CardInsertedAndCurrentUserValidator.Validate(_currentUser, _cardInserted);
            if (_currentUser.GetType() == typeof(Operator))
                throw new InvalidOperatorAction("An operator cannot withdraw money");
            InputAmountValidator.Validate(amount);
            CurrentUserBalanceValidator.Validate(_currentUser,amount);
            AtmTotalMoneyValidator.Validate(_totalMoney, amount);

            _currentUser.Balance -= amount;
            _totalMoney.Amount -= amount;
            Money money = new Money(amount);
            var fee = (decimal)amount / 100;

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

            _currentUser.Balance -= fee;
            _database.FeeList.Add(new Fee(fee, _currentUser.CardNumber));

            return money;
        }

        public void ReturnCard()
        {
            _currentUser = null;
            _cardInserted = false;
        }

        public void LoadMoney(Money money)
        {
            CardInsertedAndCurrentUserValidator.Validate(_currentUser, _cardInserted);
            if (_currentUser.GetType() == typeof(Client))
                throw new ClientCannotLoadMoney("The client cannot load money into the ATM");

            _totalMoney.Amount += money.Amount;

            foreach (var paperNote in money.Notes)
            {
                _totalMoney.Notes[paperNote.Key] += money.Notes[paperNote.Key];
            }
        }

        public IEnumerable<Fee> RetrieveChargedFees()
        {
            CardInsertedAndCurrentUserValidator.Validate(_currentUser, _cardInserted);
            if (_currentUser.GetType() == typeof(Operator))
                throw new InvalidOperatorAction("An operator does not have charged fees");

            return _database.FeeList.Where(fee => fee.CardNumber == _currentUser.CardNumber).ToList();
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using Data;
using Exceptions;

namespace ATM
{
    public class AtMachine : IAtMachine
    {
        private Money _totalMoney;
        private bool _cardInserted;
        private IPerson _currentUser;
        public string Manufacturer { get; }
        public string SerialNumber { get; }

        public AtMachine(string manufacturer, string serialNumber)
        {
            Manufacturer = manufacturer;
            SerialNumber = serialNumber;
            _totalMoney = new Money(0);
        }

        public void InsertCard(string cardNumber)
        {
            string firstNumberSequence = cardNumber.Substring(0, 4);

            if (firstNumberSequence == Bank.OperatorPrefix)
            {
                _currentUser = Bank.GetOperatorByCardNumber(cardNumber);
            }
            else if (firstNumberSequence == Bank.ClientPrefix)
            {
                _currentUser = Bank.GetClientByCardNumber(cardNumber);
            }
            else
            {
                throw new InvalidCardInserted("This card is not from our bank");
            }

            _cardInserted = true;
        }

        public decimal GetCardBalance()
        {
            if (_currentUser.GetType() == typeof(Operator)) throw new InvalidOperatorAction("An operator does not have a balance");
            Checks.CardInsertedAndCurrentUserChecks(_currentUser,_cardInserted);

            return _currentUser.Balance;
        }

        public Money WithdrawMoney(int amount)
        {
            if (_currentUser.GetType() == typeof(Operator)) throw new InvalidOperatorAction("An operator cannot withdraw money");
            Checks.CardInsertedAndCurrentUserChecks(_currentUser, _cardInserted);
            if (amount % 5 != 0 || amount <= 0) throw new InvalidInput("Invalid amount input");
            if (_currentUser.Balance < amount) throw new InsufficientBalance("The clients balance is too low");
            if (_totalMoney.Amount < amount) throw new AtmInsufficientMoney("The ATM does not have enough money to withdraw the given amount");

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
            Database.FeeList.Add(new Fee(fee,_currentUser.CardNumber));

            return money;
        }

        public void ReturnCard()
        {
            _currentUser = null;
            _cardInserted = false;
        }

        public void LoadMoney(Money money)
        {
            if (_currentUser.GetType() == typeof(Client)) throw new ClientCannotLoadMoney("The client cannot load money into the ATM");
            Checks.CardInsertedAndCurrentUserChecks(_currentUser, _cardInserted);

            _totalMoney.Amount += money.Amount;

            foreach (var paperNote in money.Notes)
            {
                _totalMoney.Notes[paperNote.Key] += money.Notes[paperNote.Key];
            }
        }

        public IEnumerable<Fee> RetrieveChargedFees()
        {
            if (_currentUser.GetType() == typeof(Operator)) throw new InvalidOperatorAction("An operator does not have charged fees");
            Checks.CardInsertedAndCurrentUserChecks(_currentUser, _cardInserted);

            var temp = new List<Fee>();

            foreach (var fee in Database.FeeList)
            {
                if (fee.CardNumber == _currentUser.CardNumber)
                {
                    temp.Add(fee);
                }
            }

            return temp;
        }
    }
}

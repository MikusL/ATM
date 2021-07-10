using System;

namespace ATM.Models
{
    public struct Fee
    {
        public string CardNumber { get; set; }
        public decimal WithdrawalFeeAmount { get; set; }
        public string WithdrawalDate { get; set; }

        public Fee(decimal amount, string cardNumber)
        {
            WithdrawalDate = DateTime.Now.ToString("HH:mm");
            WithdrawalFeeAmount = amount;
            CardNumber = cardNumber;
        }
    }
}

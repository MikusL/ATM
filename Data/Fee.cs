using System;

namespace Data
{
    public struct Fee
    {
        public string CardNumber { get; set; }
        public decimal WithdrawalFeeAmount { get; set; }
        public DateTime WithdrawalDate { get; set; }

        public Fee(decimal amount, string cardNumber)
        {
            WithdrawalDate = DateTime.Now;
            WithdrawalFeeAmount = amount;
            CardNumber = cardNumber;
        }
    }
}

using System;
using Data;

namespace ATM
{
    public class Client : IPerson
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string CardNumber { get; set; }
        public decimal Balance { get; set; }
        public DateTime DateOfBirth;
        public int ID;

        public Client(DateTime DoB, string name, string surname, int id, string clientPrefix)
        {
            Name = name;
            Surname = surname;
            DateOfBirth = DoB;
            Balance = 0;
            CardNumber = GenerateCardNumber(clientPrefix);
            ID = id;
        }

        private string GenerateCardNumber(string prefix)
        {
            string result = prefix;

            while (result == prefix)
            {
                for (var i = 0; i < 3; i++)
                {
                    Random rnd = new Random();
                    int temp = rnd.Next(1, 10000);
                    string cardNumberSection = temp.ToString();

                    while (cardNumberSection.Length < 4)
                    {
                        cardNumberSection = "0" + cardNumberSection;
                    }

                    result += cardNumberSection + " ";
                }

                result = result.Trim();

                if (Bank.CardNumberList.Contains(result))
                {
                    result = prefix;
                }
                else
                {
                    Bank.CardNumberList.Add(result);
                }
            }

            return result;
        }
    }
}

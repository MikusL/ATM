using System;
using System.Collections.Generic;

namespace Data
{
    public class Client : IPerson
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string CardNumber { get; set; }
        public decimal Balance { get; set; }
        public DateTime DateOfBirth;
        public int Id;
        private IList<string> CardNumberList { get; set; }

        public Client(DateTime doB, string name, string surname, int id, string clientPrefix, IList<string> cardNumberList)
        {
            Name = name;
            Surname = surname;
            DateOfBirth = doB;
            Balance = 0;
            CardNumber = GenerateCardNumber(clientPrefix);
            Id = id;
            CardNumberList = cardNumberList;
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

                if (CardNumberList.Contains(result))
                {
                    result = prefix;
                }
                else
                {
                    CardNumberList.Add(result);
                }
            }

            return result;
        }
    }
}

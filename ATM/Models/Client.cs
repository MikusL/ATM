using System;
using System.Collections.Generic;

namespace ATM.Models
{
    public class Client : Person
    {
        public DateTime DateOfBirth;
        public int Id;
        private IList<string> CardNumberList { get; set; }

        public Client(DateTime doB, string name, string surname, int id, string clientPrefix, IList<string> cardNumberList)
        {
            Name = name;
            Surname = surname;
            DateOfBirth = doB;
            Balance = 0;
            Id = id;
            CardNumberList = cardNumberList;
            CardNumber = GenerateCardNumber(clientPrefix);
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

                    result += " " + cardNumberSection;
                }

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

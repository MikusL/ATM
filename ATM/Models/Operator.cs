using System;
using System.Collections.Generic;

namespace ATM.Models
{
    public class Operator : Person
    {
        public int EmployeeId;
        private IList<string> CardNumberList { get; set; }


        public Operator(string name, string surname, int employeeId, string operatorPrefix, IList<string> cardNumberList)
        {
            Name = name;
            Surname = surname;
            EmployeeId = employeeId;
            CardNumberList = cardNumberList;
            CardNumber = GenerateCardNumber(operatorPrefix);
        }

        public string GenerateCardNumber(string prefix)
        {
            string result = prefix;

            while (result == prefix)
            {
                for (var i = 0; i < 2; i++)
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

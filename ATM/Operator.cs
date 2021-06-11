using System;
using Data;

namespace ATM
{
    public class Operator : IPerson
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string CardNumber { get; set; }
        public decimal Balance { get; set; }
        public int EmployeeID;
        

        public Operator(string name, string surname, int employeeID, string operatorPrefix)
        {
            Name = name;
            Surname = surname;
            EmployeeID = employeeID;
            CardNumber = GenerateCardNumber(operatorPrefix);
        }

        public string GenerateCardNumber(string prefix)
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

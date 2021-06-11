using System.Collections.Generic;

namespace Data
{
    public struct Money
    {
        public int Amount { get; set; }
        public Dictionary<PaperNote, int> Notes { get; set; }

        public Money(int amount)
        {
            Amount = amount;
            Notes = new Dictionary<PaperNote, int>();

            Notes.Add(new PaperNote(5),0);
            Notes.Add(new PaperNote(10),0);
            Notes.Add(new PaperNote(20),0);
            Notes.Add(new PaperNote(50),0);
        }
    }
}

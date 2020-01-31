using System;
using System.Collections.Generic;
using System.Linq;

namespace Guestbook.Core
{
    internal class DisplayEntriesView : View
    {
        public IEnumerable<Entry> Entries { get; set; }
        public string DisplayMessage { get; set; }
        public void Display()
        {
            Clear();
            Console.WriteLine(DisplayMessage + "\n");

            foreach (var entry in Entries)
            {
                Console.Write(entry.DateOfEntry.ToString("dddd, dd MMMM yyyy HH:mm:ss"));
                Console.WriteLine($",\n   {entry.Author.Alias} wrote:\n   {entry.EntryText}\n");
            }
            WaitForKeyPress();
            NextView();
        }
    }
}
using System;

namespace Guestbook.Core
{
    internal class PostEntryView : View
    {
        public Func<string, Result> EntryValidation { get; set; }
        public Action<string> PostEntryCallback { get; set; }

        public void Display()
        {
            Console.WriteLine("Please write your entry to the guestbook");
            var entryText = ValidateInput(EntryValidation);
            PostEntryCallback(entryText);
            Clear();
            WaitForKeyPress();
            NextView();
        }

    }
}
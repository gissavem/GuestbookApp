using System;

namespace Guestbook.Core
{
    internal class PostEntryView : View
    {
        public Action<string> PostEntryCallback { get; set; }

        public void Display()
        {
            Clear();
            Console.WriteLine("Please write your entry to the guestbook");
            var entryText = ValidateInput(ValidateEntryText);
            PostEntryCallback(entryText);
            Clear();
            WaitForKeyPress();
            NextView();
        }

    }
}
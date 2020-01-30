using Guestbook.Core.Features.Entries;
using System;

namespace Guestbook.Core
{
    internal class PostEntryView : View
    {
        public Func<string, Result> EntryValidation { get; set; }
        public Action<string> PostEntryCallback { get; set; }

        public void Display()
        {
            var entryText = ValidateInput(EntryValidation);
            PostEntryCallback(entryText);
            Clear();
            NextView();
        }

    }
}
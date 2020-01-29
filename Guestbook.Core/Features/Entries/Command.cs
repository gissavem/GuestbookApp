using Guestbook.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guestbook.Core.Features.Entries
{
    public class Command
    {
        public Command(string entryText, Author author)
        {
            if (string.IsNullOrWhiteSpace(entryText))
            {
                throw new ArgumentException("message", nameof(entryText));
            }

            EntryText = entryText;
            Author = author ?? throw new ArgumentNullException(nameof(author));
        }
        public string EntryText { get; set; }
        public Author Author { get; set; }
    }
}

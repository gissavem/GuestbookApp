using Guestbook.Core.Entities;
using Guestbook.Core.Persistance;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guestbook.Core.Features.Entries
{
    public class CommandHandler
    {
        private readonly EntryContext _context;

        public CommandHandler(EntryContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public void Handle (Command command)
        {
            
            var currentAuthor = _context.Authors.Find(command.Author.Id);
            var authorEntry = new Entry()
            {
                Id = Guid.NewGuid().ToString(),
                DateOfEntry = DateTime.Now,
                EntryText = command.EntryText
            };
            if (currentAuthor.Entries == null)
            {
                currentAuthor.Entries = new List<Entry> { authorEntry };
            }
            else
            {
                currentAuthor.Entries.Add(authorEntry);
            }
            _context.SaveChanges();
            System.Console.Clear();
            System.Console.WriteLine("Thank you for your entry!");
            
        }
  
    }
}

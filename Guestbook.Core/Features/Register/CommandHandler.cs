using System;
using Guestbook.Core.Entities;
using Guestbook.Core.Persistance;

namespace Guestbook.Core.Features.Register
{
    public class CommandHandler
    {
        private readonly EntryContext _context;

        public CommandHandler(EntryContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }  
        public void Handle(Command command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(command.Password);
            
            var author = new Author(command.Username, passwordHash, command.Alias);
            
            _context.Authors.Add(author);
            _context.SaveChanges();
       
        }
    }
}
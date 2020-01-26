using System;
using System.Linq;
using Guestbook.Core.Persistance;

namespace Guestbook.Core.Features.Login
{
    public class CommandHandler
    {
        private readonly EntryContext _context;

        public CommandHandler(EntryContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public CommandResult Handle(Command command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            
            var query = _context.Authors.Where(a => a.UserName == command.Username);

            var author = query.SingleOrDefault();
            
            if (author == null)
            {
                return new CommandResult()
                {
                    Success = false
                };
            }
           
            if (BCrypt.Net.BCrypt.Verify(command.Password, author.PasswordHash))
            {
                return new CommandResult()
                {
                    Success = true,
                    Author = author
                };
            }
            return new CommandResult()
            {
                Success = false
            };
        }
    }
}
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
        public LoginResult ValidateLogin(Input input)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            
            var query = _context.Authors.Where(a => a.Username == input.Username);

            var author = query.SingleOrDefault();
            
            if (author == null)
            {
                return new LoginResult()
                {
                    Success = false
                };
            }
           
            if (BCrypt.Net.BCrypt.Verify(input.Password, author.PasswordHash))
            {
                return new LoginResult()
                {
                    Success = true,
                    Author = author
                };
            }
            return new LoginResult()
            {
                Success = false
            };
        }
    }
}
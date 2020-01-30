using System;
using System.Collections.Generic;
using System.Linq;
using Guestbook.Core.Persistance;

namespace Guestbook.Core.Features.Register
{
    public class InputValidator
    {
        private const int MaxEntryTextLength = 140;
        private readonly EntryContext _context;

        public InputValidator(EntryContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Result ValidateUsername(string username)
        {
            var result = new Result();
            

            if (string.IsNullOrWhiteSpace(username))
            {
                result.ValidationMessages.Add("You cannot enter an empty string as a username.");
                result.Success = false;
                return result;
            }

            if (!ValidateUsernameLength(username))
            {
                result.ValidationMessages.Add("Your username cannot exceed 12 characters.");
                result.Success = false;
            }

            if (!UsernameIsUnique(username))
            {
                result.ValidationMessages.Add("That username is already taken, please pick another!");
                result.Success = false;
                return result;
            }

            return result;
        }

        public Result ValidatePassword(string password)
        {
            var result = new Result();

            if (string.IsNullOrWhiteSpace(password))
            {
                result.ValidationMessages.Add("You cannot enter an empty string as a password.");
                result.Success = false;
                return result;
            }

            if (!ValidatePasswordLength(password))
            {
                result.ValidationMessages.Add("Your password cannot exceed 12 characters.");
                result.Success = false;
            }

            return result;
        }

        public Result ValidateAlias(string alias)
        {
            var result = new Result();

            if (string.IsNullOrWhiteSpace(alias))
            {
                result.ValidationMessages.Add("You cannot enter an empty string as a alias.");
                result.Success = false;
                return result;
            }

            if (!ValidateAliasLength(alias))
            {
                result.ValidationMessages.Add("Your username cannot exceed 20 characters.");
                result.Success = false;
            }

            return result;
        }
        public Result ValidateEntryText(string entryText)
        {
            var result = new Result();

            if (string.IsNullOrWhiteSpace(entryText))
            {
                result.ValidationMessages.Add("You cannot enter an empty string as an entry.");
                result.Success = false;
                return result;
            }

            if (!ValidateEntryTextLength(entryText))
            {
                result.ValidationMessages.Add("Your entry cannot exceed 140 characters.");
                result.Success = false;
            }

            return result;
        }
        public Result ValidateLogin(Login.Input input)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            var result = new Result();

            var query = _context.Authors.Where(a => a.Username == input.Username);

            var author = query.SingleOrDefault();

            if (author == null)
            {
                result.Success = false;
                result.ValidationMessages.Add("Invalid username or password.");
                return result;
            }

            if (BCrypt.Net.BCrypt.Verify(input.Password, author.PasswordHash))
            {
                result.Success = true;
                return result;
            }
            result.Success = false;
            result.ValidationMessages.Add("Invalid username or password.");
            return result;
        }
        private bool ValidateAliasLength(string alias)
        {
            const int maxAliasLength = 20;

            return alias.Length <= maxAliasLength;
        }

        private static bool ValidateUsernameLength(string username)
        {
            const int maxUsernameLength = 12;

            return username.Length <= maxUsernameLength;
        }

        private bool UsernameIsUnique(string userName)
        {
            var query = _context.Authors.Where(user => user.Username == userName);

            return !query.ToList().Any();
        }


        private bool ValidatePasswordLength(string password)
        {
            const int maxPasswordLength = 12;

            return password.Length <= maxPasswordLength;
        }
        

        private bool ValidateEntryTextLength(string entryText)
        {
            return entryText.Length <= MaxEntryTextLength;
        }
    }


    public class Result
    {
        public bool Success { get; set; } = true;
        public List<string> ValidationMessages { get; set; } = new List<string>();
    }
}
using System;

namespace Guestbook.Core.Features.Register
{
    public class Command
    {
        public Command(string username, string password, string @alias)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(username));
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(password));
            if (string.IsNullOrWhiteSpace(alias))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(alias));
            Username = username;
            Password = password;
            Alias = alias;
        }
        
        public string Username { get; }
        public string Password { get; }
        public string Alias { get; }

    }
}
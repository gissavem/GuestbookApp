using System;

namespace Guestbook.Core.Features.Login
{
    public class Command
    {
        
        public Command(string password, string username)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(password));
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(username));
            Password = password;
            Username = username;
        }
        public string Username { get; }
        public string Password { get; }
    }
}
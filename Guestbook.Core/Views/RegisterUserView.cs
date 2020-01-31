using System;

namespace Guestbook.Core
{
    internal class RegisterUserView : View
    {
        public Func<string> GetInput { get; set; }
        public Action<RegistrationInput> RegisterUserCallback { get; set; }
        internal void Display()
        {
            var input = new RegistrationInput();
            Console.WriteLine("Please enter a username");
            input.Username = ValidateInput(ValidateUsername);
            CheckIfUnique = null;
            Console.WriteLine("Please enter a password");
            input.Password = ValidateInput(ValidatePassword, true);
            Console.WriteLine("Please enter an alias");
            input.Alias = ValidateInput(ValidateAlias);

            RegisterUserCallback(input);
            WaitForKeyPress();
            NextView();
        }
    }
}
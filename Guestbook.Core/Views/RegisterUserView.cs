using System;

namespace Guestbook.Core
{
    internal class RegisterUserView : View
    {
        public Func<string> GetInput { get; set; }
        public Func<string, Result> UsernameValidation { get; set; }
        public Func<string, Result> PasswordValidation { get; set; }
        public Func<string, Result> AliasValidation { get; set; }
        public Action<RegistrationInput> RegisterUserCallback { get; set; }
        internal void Display()
        {
            var input = new RegistrationInput();
            Console.WriteLine("Please enter a username");
            input.Username = ValidateInput(UsernameValidation);
            CheckIfUnique = null;
            Console.WriteLine("Please enter a password");
            input.Password = ValidateInput(PasswordValidation, true);
            Console.WriteLine("Please enter an alias");
            input.Alias = ValidateInput(AliasValidation);

            RegisterUserCallback(input);
            WaitForKeyPress();
            NextView();
        }
    }
}
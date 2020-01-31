using Guestbook.Core.Features.Register;
using System;

namespace Guestbook.Core
{
    internal class RegisterUserView : View
    {
        public Func<string> GetInput { get; set; }
        public Func<string, Result> UsernameValidation { get; set; }
        public Func<string, Result> PasswordValidation { get; set; }
        public Func<string, Result> AliasValidation { get; set; }
        public Action<Input> RegisterUserCallback { get; set; }
        internal void Display()
        {
            var input = new Input();
            Console.WriteLine("Please enter a username");
            input.Username = ValidateInput(UsernameValidation);
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
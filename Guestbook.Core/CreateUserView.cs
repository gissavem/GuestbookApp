using Guestbook.Core.Features.Register;
using System;

namespace Guestbook.Core
{
    internal class CreateUserView : View
    {
        public Func<string> GetInput { get; set; }
        public Func<string, Result> UsernameValidation { get; set; }
        public Func<string, Result> PasswordValidation { get; set; }
        public Func<string, Result> AliasValidation { get; set; }
        public Action<Input> CreateUserCallback { get; set; }
        internal void DisplayCreateUserView()
        {
            var input = new Input();

            input.Username = ValidateInput(UsernameValidation);
            input.Password = ValidateInput(PasswordValidation, true);
            input.Alias = ValidateInput(AliasValidation);

            CreateUserCallback(input);
            NextView();
        }
    }
}
using Guestbook.Core.Features.Register;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guestbook.Core
{
    class LoginView : View
    {
        public Action LoginSuccessCallback { get; set; }
        public Action LoginFailCallback { get; internal set; }
        public Func<Features.Login.Input, Result> LoginValidationMethod { get; set; }

        public void Display()
        {
            var input = new Features.Login.Input()
            {
                Username = GetUsername(),
                Password = GetPassword()
            };

            var result = LoginValidationMethod(input);

            foreach (var message in result.ValidationMessages)
            {
                Console.WriteLine(message);
            }
            WaitForKeyPress();
            if (result.Success)
            {
                LoginSuccessCallback();
            }
            else
            {
                LoginFailCallback();
            }
        }
        private string GetUsername()
        {
            Console.Write($"please enter your username: ");
            return Console.ReadLine();
        }
        private string GetPassword()
        {
            Console.Write($"please enter your password: ");
            string password = null;
            while (true)
            {
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter)
                    break;
                password += key.KeyChar;
                Console.Write("*");
            }
            Clear();
            return password;
        }
    }
}

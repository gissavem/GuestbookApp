using System;
using Guestbook.Core.Features.Register;

namespace Guestbook.Core.Features.Register
{
    public class InputHandler
    {
        private readonly InputValidator _inputValidator;

        public InputHandler(InputValidator inputValidator)
        {
            _inputValidator = inputValidator ?? throw new ArgumentNullException(nameof(inputValidator));
        }

        public Input Handle()
        {
            var input = new Input()
            {
                Username = TryUsername(),
                Password = TryPassword(),
                Alias = TryAlias()
            };
            return input;
        }

        private string TryAlias()
        {
            string alias;
            bool tryAgain;
            do
            {
                System.Console.Write($"please enter your alias: ");
                alias = System.Console.ReadLine();

                var validationResult = _inputValidator.ValidateAlias(alias);

                tryAgain = !validationResult.Success;

                if (!validationResult.Success)
                {
                    foreach (var message in validationResult.ValidationMessages)
                    {
                        System.Console.WriteLine(message);
                    }
                }
            } while (tryAgain);

            return alias;
        }

        private string TryUsername()
        {
            string username;
            bool tryAgain;
            do
            {
                System.Console.Write($"please enter your username: ");
                username = System.Console.ReadLine();

                var validationResult = validateAction(username);//_inputValidator.ValidateUsername(username);

                tryAgain = !validationResult.Success;

                if (!validationResult.Success)
                {
                    foreach (var message in validationResult.ValidationMessages)
                    {
                        System.Console.WriteLine(message);
                    }
                }
            } while (tryAgain);

            return username;
        }
        
        private string TryPassword()
        {
            string password = null;
            bool tryAgain;
            do
            {
                System.Console.Write($"please enter your password: ");
                while (true)
                {
                    var key = System.Console.ReadKey(true);
                    if (key.Key == ConsoleKey.Enter)
                        break;
                    if (key.Key == ConsoleKey.Backspace)
                    {
                        continue;
                    }

                    password += key.KeyChar;
                    System.Console.Write("*");
                }

                var validationResult = _inputValidator.ValidatePassword(password);

                tryAgain = !validationResult.Success;

                if (!validationResult.Success)
                {
                    foreach (var message in validationResult.ValidationMessages)
                    {
                        System.Console.WriteLine(message);
                    }
                }
            } while (tryAgain);

            System.Console.Clear();
            return password;
        }
    }
}
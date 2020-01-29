using System;
using System.Collections.Generic;
using System.Text;
using Guestbook.Core.Features.Entries;

namespace Guestbook.Console.Features.Entries
{
    class InputHandler
    {
        private readonly InputValidator _inputValidator;

        public InputHandler(InputValidator inputValidator)
        {
            _inputValidator = inputValidator ?? throw new ArgumentNullException(nameof(inputValidator));
        }
        public Input Handle()
        {
            var input = new Input
            {
                Text = TryEntryInput()
            };
            return input;
        }
        private string TryEntryInput()
        {
            string entryText;
            bool tryAgain;
            do
            {
                System.Console.Write($"please write your entry: ");
                entryText = System.Console.ReadLine();

                var validationResult = _inputValidator.ValidateEntryText(entryText);

                tryAgain = !validationResult.Success;

                if (!validationResult.Success)
                {
                    foreach (var message in validationResult.ValidationMessages)
                    {
                        System.Console.WriteLine(message);
                    }
                }
            } while (tryAgain);

            return entryText;
        }
    }
}

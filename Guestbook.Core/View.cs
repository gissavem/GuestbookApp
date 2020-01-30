using Guestbook.Core.Entities;
using Guestbook.Core.Features.Register;
using System;
using System.Linq;

namespace Guestbook.Core
{
    public class View
    {
        public Action NextView { get; set; }

        public string ValidateInput(Func<string, Result> validation, bool hideInput = false)
        {
            string input;
            bool tryAgain;
            do
            {
                if (hideInput)
                {
                    input = null;
                    while (true)
                    {
                        var key = Console.ReadKey(true);
                        if (key.Key == ConsoleKey.Enter)
                            break;
                        input += key.KeyChar;
                        Console.Write("*");
                    }
                    Console.Clear();
                }
                else
                {
                    input = Console.ReadLine();
                }

                var validationResult = validation(input);

                tryAgain = !validationResult.Success;

                if (!validationResult.Success)
                {
                    foreach (var message in validationResult.ValidationMessages)
                    {
                        Console.WriteLine(message);
                    }
                }
            } while (tryAgain);

            return input;
        }
        protected void Clear()
        {
            Console.Clear();
        }
    }
}
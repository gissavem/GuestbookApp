using System;

namespace Guestbook.Core
{
    public class View
    {
        public Action NextView { get; set; }
        /// <summary>
        /// ReTries the input until the validation method provided returns a successful result. The hideInput paramater hides the input in the console if set to true.
        /// </summary>
        /// <param name="validation"></param>
        /// <param name="hideInput"></param>
        /// <returns></returns>
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
        protected void WaitForKeyPress()
        {
            Console.WriteLine("\nPress any key to continue..");
            Console.ReadKey(true);
        }
        protected void Clear()
        {
            Console.Clear();
        }
    }
}
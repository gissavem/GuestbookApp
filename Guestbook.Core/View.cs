using System;

namespace Guestbook.Core
{
    public class View
    {
        private const int MaxEntryTextLength = 140;
        public Func<string, Result> IsUnique { get; set; }
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

                var uniqueResult = new Result();
                if (IsUnique != null)
                {
                    uniqueResult = IsUnique(input);
                    tryAgain = !uniqueResult.Success;
                }

                if (!validationResult.Success || !uniqueResult.Success)
                {
                    foreach (var message in validationResult.ValidationMessages)
                    {
                        Console.WriteLine(message);
                    }
                    foreach (var message in uniqueResult.ValidationMessages)
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
        public Result ValidateUsername(string username)
        {
            var result = new Result();


            if (string.IsNullOrWhiteSpace(username))
            {
                result.ValidationMessages.Add("You cannot enter an empty string as a username.");
                result.Success = false;
                return result;
            }

            if (!ValidateUsernameLength(username))
            {
                result.ValidationMessages.Add("Your username cannot exceed 12 characters.");
                result.Success = false;
            }

            return result;
        }

        public Result ValidatePassword(string password)
        {
            var result = new Result();

            if (string.IsNullOrWhiteSpace(password))
            {
                result.ValidationMessages.Add("You cannot enter an empty string as a password.");
                result.Success = false;
                return result;
            }

            if (!ValidatePasswordLength(password))
            {
                result.ValidationMessages.Add("Your password cannot exceed 12 characters.");
                result.Success = false;
            }

            return result;
        }

        public Result ValidateAlias(string alias)
        {
            var result = new Result();

            if (string.IsNullOrWhiteSpace(alias))
            {
                result.ValidationMessages.Add("You cannot enter an empty string as a alias.");
                result.Success = false;
                return result;
            }

            if (!ValidateAliasLength(alias))
            {
                result.ValidationMessages.Add("Your username cannot exceed 20 characters.");
                result.Success = false;
            }

            return result;
        }
        public Result ValidateEntryText(string entryText)
        {
            var result = new Result();

            if (string.IsNullOrWhiteSpace(entryText))
            {
                result.ValidationMessages.Add("You cannot enter an empty string as an entry.");
                result.Success = false;
                return result;
            }

            if (!ValidateEntryTextLength(entryText))
            {
                result.ValidationMessages.Add("Your entry cannot exceed 140 characters.");
                result.Success = false;
            }

            return result;
        }
        private bool ValidateAliasLength(string alias)
        {
            const int maxAliasLength = 20;

            return alias.Length <= maxAliasLength;
        }

        private static bool ValidateUsernameLength(string username)
        {
            const int maxUsernameLength = 12;

            return username.Length <= maxUsernameLength;
        }
        private bool ValidatePasswordLength(string password)
        {
            const int maxPasswordLength = 12;

            return password.Length <= maxPasswordLength;
        }

        private bool ValidateEntryTextLength(string entryText)
        {
            return entryText.Length <= MaxEntryTextLength;
        }
    }
}
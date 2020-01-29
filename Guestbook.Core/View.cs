using Guestbook.Core.Entities;
using Guestbook.Core.Features.Login;
using Guestbook.Core.Features.Register;
using System;
using System.Linq;

namespace Guestbook.Core
{
    internal class View
    {
        public View()
        {
        }
        public Action<string> SendToController { get; set; }
        public Func<string, Result> ValidationMethod { get; set; }
        public Func<string> GetInput { get; set; }
        public void PrintWelcome()
        {
            Console.WriteLine("Welcome to this guestbook!");
        }
        public void PrintLoginMenuInstructions()
        {
            Console.WriteLine("If you are an existing user press 1, to register a new user press 2, to quit press 3.");
        }
        public void PrintMainMenuInstructions()
        {
            Console.WriteLine(" Press 1 to leave an entry in the guestbook.\n\n Press 2 to view past entries\n\n Press 3 to log off");
        }
        public ConsoleKey GetConsoleKey()
        {
            return Console.ReadKey().Key;
        }
        public void Clear()
        {
            Console.Clear();
        }
        public string TryGetUserInput()
        {
            if (ValidationMethod == null || GetInput == null)
            {
                throw new Exception("Delegate properties of view must be set before this method is called.");
            }

            string input;
            bool tryAgain;
            do
            {
                input = GetInput();
                 
                var validationResult = ValidationMethod(input);

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
        public Features.Login.Input GetLoginInput()
        {
            return new Features.Login.Input()
            {
                Username = GetUsername(),
                Password = GetPassword()
            };
        }
        public string GetPassword()
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
            Console.Clear();
            return password;
        }

        public void PrintAllEntries(IOrderedQueryable<Entry> entries)
        {
            foreach (var entry in entries)
            {
                Console.Write(entry.DateOfEntry.ToString("dddd, dd MMMM yyyy HH:mm:ss"));
                Console.WriteLine($", {entry.Author.Alias} wrote:\n   {entry.EntryText}\n");
            }
        }

        public void ConfirmEntry()
        {
            Console.WriteLine("Thank you for your entry!");
        }

        public string GetEntry()
        {
            Console.Write($"please write your entry: ");
            return Console.ReadLine();
        }

        public string GetUsername()
        {
            Console.Write($"please enter your username: ");
            return Console.ReadLine();
        }
        public string GetAlias()
        {
            Console.Write($"please enter your alias: ");
            return Console.ReadLine(); 
        }
        public void PrintInvalidLogin()
        {
            Console.WriteLine("Invalid username or password.");
        }
    }
}
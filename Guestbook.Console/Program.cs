using System;
using System.Collections.Generic;
using System.Linq;
using Guestbook.Core.Entities;
using Guestbook.Core.Features.Register;
using Guestbook.Core.Persistance;
using CommandHandler = Guestbook.Core.Features.Login.CommandHandler;


namespace Guestbook.Console
{
    class Program
    {
        static void Main()
        {
            var programRunning = true;

            using (var context = new EntryContext())
            {

                Author currentAuthor = null;
                context.Database.EnsureCreated();

                System.Console.WriteLine("Welcome to this guestbook!");
                
                while (currentAuthor == null)
                {
                    
                    System.Console.WriteLine("If you are an existing user press 1, otherwise press 2");

                    
                    switch (System.Console.ReadKey().Key)
                    {
                        case ConsoleKey.D1:
                            System.Console.Clear();
                            var input = new Features.Login.InputHandler().Handle();
                            var command = new Guestbook.Core.Features.Login.Command(input.Password, input.Username);
                            var result = new CommandHandler(context).Handle(command);
                            if (result.Success)
                            {
                                currentAuthor = result.Author;
                            }
                            else
                            {
                                System.Console.WriteLine("Invalid username or password.");
                            }
                            break;
                        case ConsoleKey.D2:
                            System.Console.Clear();
                            var inputValidator = new InputValidator(context);
                            var input2 = new Features.Register.InputHandler(inputValidator).Handle();
                            var command2 = new Guestbook.Core.Features.Register.Command(input2.Username, input2.Password, input2.Alias);
                            new Guestbook.Core.Features.Register.CommandHandler(context).Handle(command2);
                            break;
                        default:
                            System.Console.Clear();
                            break;
                    }
                }
                context.SaveChanges();
                
                while (programRunning)
                {
                    PrintInstructions();

                    var userInput = System.Console.ReadKey();
                    if (userInput.Key == ConsoleKey.D1)
                    {
                        AddEntryToDatabase(context, currentAuthor);
                    }
                    else if (userInput.Key == ConsoleKey.D2)
                    {
                        PrintAllEntries(context);
                    }
                    else if (userInput.Key == ConsoleKey.D3)
                    {
                        programRunning = false;
                    }
                    else
                    {
                    }
                }
            }
        }

        private static void PrintInstructions()
        {
            System.Console.WriteLine(" Press 1 to leave an entry in the guestbook.\n\n Press 2 to view past entries\n\n Press 3 to quit");
        }
        private static void AddEntryToDatabase(EntryContext context, Author currentUser)
        {
            System.Console.Clear();
            var user = context.Authors.Find(currentUser.Id);
            var userEntry = new Entry()
            {
                Id = Guid.NewGuid().ToString(),
                DateOfEntry = DateTime.Now,
                EntryText = TryEntryInput()
            };
            if (user.Entries == null)
            {
                user.Entries = new List<Entry> { userEntry };
            }
            else
            {
                user.Entries.Add(userEntry);
            }
            context.SaveChanges();
            System.Console.Clear();
            System.Console.WriteLine("Thank you for your entry!");
        }

        private static string TryEntryInput()
        {
            string input = null;
            while (input == null)
            {
                System.Console.WriteLine("Please write your entry to the guestbook (MAX 140 characters)");
                try
                {
                    input = System.Console.ReadLine()?.Trim();
                    if (input != null && input.Length > 140)
                    {
                        throw new Exception("Your entry was to long, please try a shorter one.");
                    }
                    if (string.IsNullOrEmpty(input))
                    {
                        throw new Exception("You need to write something");
                    }
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine(ex.Message);
                    input = null;
                }
            }
            return input;
        }
        private static void PrintAllEntries(EntryContext context)
        {
            System.Console.Clear();
            var query = from entry in context.Entries
                        orderby entry.DateOfEntry ascending
                        select entry;

            foreach (var entry in query)
            {
                System.Console.Write(entry.DateOfEntry.ToString("dddd, dd MMMM yyyy HH:mm:ss"));
                System.Console.WriteLine($", {entry.Author.Alias} wrote:\n   {entry.EntryText}\n");
            }
        }
    }
}

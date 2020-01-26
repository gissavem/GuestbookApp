using BCrypt.Net;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AzureLab
{
    class Program
    {
        static void Main()
        {
            bool programRunning = true;

            using (EntryContext context = new EntryContext())
            {
                var menuManager = new MenuManager();
                menuManager.ShowMainMenu();

                Author currentUser = null;
                context.Database.EnsureCreated();

                while (currentUser == null)
                {
                    Console.WriteLine("Welcome to this guestbook!");
                    Console.WriteLine("If you are an existing user press 1, otherwise press 2");

                    switch (Console.ReadKey().Key)
                    {
                        case ConsoleKey.D1:
                            Console.Clear();
                            currentUser = TryLogin(context);
                            break;
                        case ConsoleKey.D2:
                            Console.Clear();
                            currentUser = CreateAndGetNewUser(context);
                            break;
                        default:
                            Console.Clear();
                            break;
                    }
                }
                context.SaveChanges();
                
                while (programRunning)
                {
                    PrintInstructions();

                    var userInput = Console.ReadKey();
                    switch (userInput.Key)
                    {
                        case ConsoleKey.D1:
                            AddEntryToDatabase(context, currentUser);
                            break;
                        case ConsoleKey.D2:
                            PrintAllEntries(context);
                            break;
                        case ConsoleKey.D3:
                            programRunning = false;
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private static Author TryLogin(EntryContext context)
        {
            string userName = TryUsername();
            string password = TryPassword();

            var query = from user in context.Authors
                        where user.UserName == userName
                        select user;

            if (query.ToList().Any() == false)
            {
                Console.WriteLine("\nThe username or password was incorrect!");
                return null;
            }
            if (query.First().UserName == userName && BCrypt.Net.BCrypt.Verify(password, query.First().Password))
            {
                return query.First();
            }

            Console.WriteLine("\nThe username or password was incorrect!");
            return null;
        }

        private static string TryPassword()
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

        private static string TryUsername()
        {
            Console.Write($"please enter your username: ");
            return Console.ReadLine();
        }

        private static Author CreateAndGetNewUser(EntryContext context)
        {
            string userName = TryUserInput("username");

            while (CheckIfTaken(context, userName))
            {
                Console.WriteLine("That username is already taken, please pick another!");
                userName = TryUserInput("username");
                Console.Clear();
            }

            string password = TryUserInput("password");
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

            string alias = TryUserInput("alias");

            Author author = new Author()
            {
                Id = Guid.NewGuid().ToString(),
                Alias = alias,
                UserName = userName,
                Password = passwordHash
            };
            try
            {
                context.Authors.Add(author);
                context.SaveChanges();
                return author;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        private static bool CheckIfTaken(EntryContext context, string userName)
        {
            var query = from user in context.Authors
                        where user.UserName == userName
                        select user;
            if (query.ToList().Any())
            {
                return true;
            }
            return false;
        }

        private static void PrintInstructions()
        {
            Console.WriteLine(" Press 1 to leave an entry in the guestbook.\n\n Press 2 to view past entries\n\n Press 3 to quit");
        }
        private static void AddEntryToDatabase(EntryContext context, Author currentUser)
        {
            Console.Clear();
            var user = context.Authors.Find(currentUser.Id);
            Entry userEntry = new Entry()
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
            Console.Clear();
            Console.WriteLine("Thank you for your entry!");
        }

        private static string TryEntryInput()
        {
            string input = null;
            while (input == null)
            {
                Console.WriteLine("Please write your entry to the guestbook (MAX 140 characters)");
                try
                {
                    input = Console.ReadLine().Trim();
                    if (input.Length > 140)
                    {
                        throw new Exception("Your entry was to long, please try a shorter one.");
                    }
                    if (input == "" || input == null)
                    {
                        throw new Exception("You need to write something");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    input = null;
                }
            }
            return input;
        }
        private static void PrintAllEntries(EntryContext context)
        {
            Console.Clear();
            var query = from entry in context.Entries
                        orderby entry.DateOfEntry ascending
                        select entry;

            foreach (var entry in query)
            {
                Console.Write(entry.DateOfEntry.ToString("dddd, dd MMMM yyyy HH:mm:ss"));
                Console.WriteLine($", {entry.Author.Alias} wrote:\n   {entry.EntryText}\n");
            }
        }

        private static string TryUserInput(string target)
        {

            string stringToReturn = null;
            while (stringToReturn == null)
            {
                try
                {
                    string input = null;

                    if (target == "password")
                    {
                        input = TryPassword();
                    }
                    else
                    {
                        Console.WriteLine($"Please enter {target}:");
                        input = Console.ReadLine().Trim();
                    }
                    if (input == null || input == "")
                    {
                        Console.WriteLine();
                        throw new Exception($"\nYou cannot enter an empty string as a {target}.. Please try again");
                    }
                    if (input.Length > 12)
                    {
                        Console.WriteLine();
                        throw new Exception($"Your {target} cannot exceed 12 characters..");
                    }
                    stringToReturn = input;
                    Console.Clear();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    continue;
                }
            }
            return stringToReturn;
        }

    }
}

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
                Authors currentUser = null;

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
                            //MAKE SURE THAT YOU CANNOT CREATE A USER IF THEIR USERNAME ALREADY EXISTS
                            break;
                        default:
                            Console.Clear();
                            break;
                    }
                }

  
                //Confirm if it exists, otherwise ask send them to EXISTING USER?
                //ASK FOR PASSWORD
                //Confirm if correct, otherwise ask again

                //Set current user to the logged in user

                //currentUserName = TryUserInput();

                
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

        private static Authors TryLogin(EntryContext context)
        {
            string userName = TryLoginInput("Username");
            string password = TryLoginInput("Password");

            var query = from user in context.Authors
                        where user.UserName == userName && user.Password == password
                        select user;

            if (query.Any() == false)
                return null;

            if (query.First().UserName == userName && query.First().Password == password)
            {
                return query.First();
            }

            return null;
            
        }

        private static string TryLoginInput(string target)
        {
            Console.WriteLine($"please enter your {target}");
            return Console.ReadLine();
        }

        private static Authors CreateAndGetNewUser(EntryContext context)
        {
            string userName = TryUserInput("username");
            string password = TryUserInput("password");
            string alias = TryUserInput("alias");

            Authors author = new Authors()
            {
                Id = Guid.NewGuid().ToString(),
                Alias = alias,
                UserName = userName,
                Password = password
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

        private static void PrintInstructions()
        {
            Console.WriteLine(" Press 1 to leave an entry in the guestbook.\n\n Press 2 to view past entries\n\n Press 3 to quit");
        }
        private static void AddEntryToDatabase(EntryContext context, Authors currentUser)
        {
            Console.Clear();
            var user = context.Authors.Find(currentUser.Id);
            Entries userEntry = new Entries()
            {
                Id = Guid.NewGuid().ToString(),
                DateOfEntry = DateTime.Now,
                EntryText = TryEntryInput()
            };
            if (user.AuthorEntries == null) user.AuthorEntries = new List<Entries> { userEntry };

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
                    Console.WriteLine($"Please enter {target}:");
                    string input = Console.ReadLine().Trim();
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

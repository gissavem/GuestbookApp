//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Guestbook.Core.Entities;
//using Guestbook.Core.Features.Register;
//using Guestbook.Core.Persistance;


//namespace Guestbook.Console
//{
//    class Program

//    // Om man gör MVC:
//    //Då är Program.cs bootstrapping. och hade i princip sett ut så här:
//    //Main() 
//    //{
//    //    
//    //    var view = new view();
//    //    var context = new Context();
//    //    var controller = new Controller(view, context);
//    //    controller.Run();
//    //}
//    {
//        static void Main() //Main metoden av programmet
//        {
//            var programRunning = true; // business check för att ha igång programmet

//            using (var context = new EntryContext()) // Model initializering, bör göras i bootstrappingen
//            {

//                Author currentAuthor = null; // controller prylar
//                context.Database.EnsureCreated(); // databas skapande, bör göras i modell init

//                System.Console.WriteLine("Welcome to this guestbook!");
                
//                while (currentAuthor == null) // en business logic loop  controller prylar
//                {
                    
//                    System.Console.WriteLine("If you are an existing user press 1, otherwise press 2"); //bör ligga som en metod i vyn

                    
//                    switch (System.Console.ReadKey().Key) //bör ligga som en metod i vyn
//                    {
//                        case ConsoleKey.D1: //bör ligga som en metod i vyn
//                            System.Console.Clear(); //bör ligga som en metod i vyn
//                            var input = new Features.Login.InputHandler().Handle(); //input detaljer
//                            var command = new Core.Features.Login.Command(input.Password, input.Username); //bör vara i controllern
//                            // varför modellerar vi inputen? Vi kan väl skapa ett business modell objekt direkt?
//                            var result = new Core.Features.Login.CommandHandler(context).ValidateLogin(command); //error handling
//                            if (result.Success)
//                            {
//                                currentAuthor = result.Author;
//                            }
//                            else
//                            {
//                                System.Console.WriteLine("Invalid username or password.");
//                            }
//                            break;
//                        case ConsoleKey.D2:
//                            System.Console.Clear();
//                            var inputValidator = new InputValidator(context);
//                            var input2 = new Features.Register.InputHandler(inputValidator).Handle();
//                            var command2 = new Core.Features.Register.Command(input2.Username, input2.Password, input2.Alias);
//                            new Core.Features.Register.CommandHandler(context).Handle(command2);
//                            break;
//                        default:
//                            System.Console.Clear();
//                            break;
//                    }
//                }
//                context.SaveChanges();
                
//                while (programRunning) // en business logic loop 
//                {
//                    PrintInstructions(); //ouputta till vy anrop

//                    var userInput = System.Console.ReadKey(); //input ifrån vyn
//                    if (userInput.Key == ConsoleKey.D1)
//                    {
//                        var inputValidator = new Core.Features.Entries.InputValidator(); // varför bryter vi ut input valideringen?
//                        var input = new Features.Entries.InputHandler(inputValidator).Handle(); // varför bryter vi ut input valideringen?
//                        var command = new Core.Features.Entries.Command(input.Text, currentAuthor); // ingen fail, onödig kod?
//                        new Core.Features.Entries.CommandHandler(context).Handle(command); 
//                    }
//                    else if (userInput.Key == ConsoleKey.D2)
//                    {
//                        PrintAllEntries(context); //ouputta till vy anrop
//                    }
//                    else if (userInput.Key == ConsoleKey.D3)
//                    {
//                        programRunning = false; //business logic anrop
//                    }
//                    else
//                    {
//                    }
//                }
//            }
//        }

//        private static void PrintInstructions()
//        {
//            System.Console.WriteLine(" Press 1 to leave an entry in the guestbook.\n\n Press 2 to view past entries\n\n Press 3 to quit");
//        }

//        private static void PrintAllEntries(EntryContext context)
//        {
//            System.Console.Clear();
//            var query = from entry in context.Entries
//                        orderby entry.DateOfEntry ascending
//                        select entry;

//            foreach (var entry in query)
//            {
//                System.Console.Write(entry.DateOfEntry.ToString("dddd, dd MMMM yyyy HH:mm:ss"));
//                System.Console.WriteLine($", {entry.Author.Alias} wrote:\n   {entry.EntryText}\n");
//            }
//        }
//    }
//}

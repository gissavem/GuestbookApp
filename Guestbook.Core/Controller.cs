using Guestbook.Core.Entities;
using Guestbook.Core.Persistance;
using System;

namespace Guestbook.Core
{
    internal class Controller
    {
        private readonly EntryContext context;
        private readonly View view;

        public Controller(EntryContext context, View view)
        {
            this.context = context ?? throw new System.ArgumentNullException(nameof(context));
            this.view = view ?? throw new System.ArgumentNullException(nameof(view));
        }
        private Author CurrentAuthor { get; set; }

        public void Run()
        {
            view.PrintWelcome();

            while (CurrentAuthor == null)
            {
                view.PrintLoginMenu();
                switch (view.GetConsoleKey())
                {
                    case ConsoleKey.D1:
                        view.Clear();
                        var input = new Features.Login.InputHandler().Handle(); //input detaljer
                        var command = new Core.Features.Login.Command(input.Password, input.Username); //bör vara i controllern
                                                                                                       // varför modellerar vi inputen? Vi kan väl skapa ett business modell objekt direkt?
                        var result = new Core.Features.Login.CommandHandler(context).Handle(command); //error handling
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
                        break;
                    case ConsoleKey.D3:
                        return;
                    default:
                        break;
                }
            }
        }
    }
}
using Guestbook.Core.Entities;
using Guestbook.Core.Persistance;
using System;

namespace Guestbook.Core
{
    internal class Controller
    {
        private readonly EntryContext context;
        private readonly View view;
        private readonly Features.Register.InputValidator inputValidator;

        public Controller(EntryContext context, View view)
        {
            this.context = context ?? throw new System.ArgumentNullException(nameof(context));
            this.view = view ?? throw new System.ArgumentNullException(nameof(view));
            inputValidator = new Features.Register.InputValidator(context);
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
                        TryLoginRequest();
                        break;
                    case ConsoleKey.D2:
                        view.Clear();
                        TryCreateUser();
                        break;
                    case ConsoleKey.D3:
                        return;
                    default:
                        break;
                }
            }
        }

        private void TryCreateUser()
        {
            var input = new Features.Register.Input();

            view.GetInput = view.GetUsername;
            view.ValidationMethod = inputValidator.ValidateUsername;
            input.Username = view.TryGetUserInput();

            view.GetInput = view.GetPassword;
            view.ValidationMethod = inputValidator.ValidatePassword;
            input.Password = view.TryGetUserInput();

            view.GetInput = view.GetAlias;
            view.ValidationMethod = inputValidator.ValidateAlias;
            input.Alias = view.TryGetUserInput();

            var command = new Features.Register.Command(input.Username, input.Password, input.Alias);
            new Core.Features.Register.CommandHandler(context).Handle(command);
        }

        private void TryLoginRequest()
        {
            var input = view.GetLoginInput();
            var command = new Features.Login.Command(input.Password, input.Username); 
            var result = new Features.Login.CommandHandler(context).Handle(command); 
            if (result.Success)
            {
                CurrentAuthor = result.Author;
            }
            else
            { 
                view.PrintInvalidLogin(); 
            }
        }
        
    }
}
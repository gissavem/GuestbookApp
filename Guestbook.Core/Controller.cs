using Guestbook.Core.Entities;
using Guestbook.Core.Features.Register;
using Guestbook.Core.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Guestbook.Core
{
    internal class Controller
    {
        private readonly EntryContext context;
        private readonly InputValidator inputValidator;
        private LoginView loginView;
        private MultiChoiceMenuView menuView;
        private InfoMessageView messageView;
        private CreateUserView createUserView;

        public Controller(EntryContext context)
        {
            this.context = context ?? throw new System.ArgumentNullException(nameof(context));
            inputValidator = new InputValidator(context);

        }
        private Author CurrentAuthor { get; set; }

        public void Run()
        {
            GoToMainMenu();
        }

        private void GoToMainMenu()
        {
            InitializeMainMenu();
            messageView.Display();
        }

        private void GoToLoginMenu()
        {
            InitializeLoginMenu();
            menuView.DisplayMultiChoiceMenu();
        }
        private void GoToLoginView()
        {
            InitializeLoginView();
            loginView.DisplayLoginView();
        }
        private void GoToUserInterface()
        {
            InitializeUserInterfaceMenu();
            menuView.DisplayMultiChoiceMenu();
        }
        private void GoToCreateUserView()
        {
            InitializeCreateUserView();
            createUserView.DisplayCreateUserView();
        }

        private void InitializeCreateUserView()
        {
            createUserView = new CreateUserView();
            createUserView.UsernameValidation = inputValidator.ValidateUsername;
            createUserView.PasswordValidation = inputValidator.ValidatePassword;
            createUserView.AliasValidation = inputValidator.ValidateAlias;
            createUserView.CreateUserCallback = CreateUser;
            createUserView.NextView = GoToLoginMenu;

        }

        private void InitializeMainMenu()
        {
            messageView = new InfoMessageView()
            {
                Message = "Welcome to this guestbook!",
                NextView = GoToLoginMenu
            };
        }


        private void InitializeLoginMenu()
        {
            menuView = new MultiChoiceMenuView();
            menuView.MenuItems = new List<NavigationMenuItem>()
            {
                new NavigationMenuItem()
                {
                    GoesTo = GoToLoginView,
                    DisplayString = "login with an existing user"
                },
                new NavigationMenuItem() 
                {
                    GoesTo = GoToCreateUserView,
                    DisplayString = "create a new user"
                },
                new NavigationMenuItem()
                {
                    GoesTo = QuitProgram,
                    DisplayString = "quit program"
                }
            };
        }

        private void InitializeUserInterfaceMenu()
        {
            menuView = new MultiChoiceMenuView();
            menuView.MenuItems = new List<NavigationMenuItem>()
            {
                new NavigationMenuItem()
                {
                    //GoesTo = TryCreateEntry,
                    DisplayString = "write a new entry"
                },
                new NavigationMenuItem()
                {
                    //Goes to view.PrintAllEntries(GetAllEntries());

                    DisplayString = "view all entries"
                },
                new NavigationMenuItem()
                {
                    //GoesTo = QuitProgram,
                    DisplayString = "log out user"
                }
            };
        }
        private void InitializeLoginView()
        {
            loginView = new LoginView();
            loginView.LoginValidationMethod = LoginUser;
            loginView.LoginSuccessCallback = GoToUserInterface;
            loginView.LoginFailCallback = GoToLoginMenu;
        }
        private void CreateUser(Features.Register.Input input)
        {
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(input.Password);

            var author = new Author(input.Username, passwordHash, input.Alias);

            context.Authors.Add(author);
            context.SaveChanges();
        }
        private Result LoginUser(Features.Login.Input input)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            var result = new Result();

            var query = context.Authors.Where(a => a.Username == input.Username);

            var author = query.SingleOrDefault();

            if (author == null)
            {
                result.Success = false;
                result.ValidationMessages.Add("Invalid username or password.");
                return result;
            }

            if (BCrypt.Net.BCrypt.Verify(input.Password, author.PasswordHash))
            {
                result.Success = true;
                CurrentAuthor = author;
                result.ValidationMessages.Add($"Welcome {author.Alias}!");
                return result;
            }
            result.Success = false;
            result.ValidationMessages.Add("Invalid username or password.");
            return result;
        }
        private void QuitProgram()
        {
            throw new NotImplementedException();
        }

        private IOrderedQueryable<Entry> GetAllEntries()
        {
            return from entry in context.Entries
                        orderby entry.DateOfEntry ascending
                        select entry;
        }

        //private void TryCreateEntry()
        //{
        //    view.GetInput = view.GetEntry;
        //    view.ValidationMethod = inputValidator.ValidateEntryText;
        //    var entryText = view.TryGetUserInput();
        //    var command = new Features.Entries.Command(entryText, CurrentAuthor); 
        //    new Features.Entries.CommandHandler(context).Handle(command);
        //    view.Clear();
        //    view.ConfirmEntry();
        //}
        //public void PrintAllEntries(IOrderedQueryable<Entry> entries)
        //{
        //    foreach (var entry in entries)
        //    {
        //        Console.Write(entry.DateOfEntry.ToString("dddd, dd MMMM yyyy HH:mm:ss"));
        //        Console.WriteLine($", {entry.Author.Alias} wrote:\n   {entry.EntryText}\n");
        //    }
        //}




    }
}
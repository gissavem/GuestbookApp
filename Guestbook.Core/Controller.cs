using System;
using System.Collections.Generic;
using System.Linq;

namespace Guestbook.Core
{
    internal class Controller
    {
        private readonly EntryContext context;
        private LoginView loginView;
        private MultiChoiceMenuView menuView;
        private InfoMessageView messageView;
        private RegisterUserView registerUserView;
        private PostEntryView postEntryView;
        private DisplayEntriesView displayEntriesView;

        public Controller(EntryContext context)
        {
            this.context = context ?? throw new System.ArgumentNullException(nameof(context));
        }
        public Author CurrentAuthor { get; set; }
        public void Run() => GoToMainMenu();
        private void GoToMainMenu()
        {
            InitializeMainMenu();
            messageView.Display();
        }
        private void InitializeMainMenu()
        {
            messageView = (messageView is null) ? new InfoMessageView() : messageView;
            messageView.Message = "Welcome to this guestbook!";
            messageView.NextView = GoToLoginMenu;
        }
        private void GoToLoginMenu()
        {
            InitializeLoginMenu();
            menuView.Display();
        }
        private void InitializeLoginMenu()
        {
            menuView = (menuView is null) ? new MultiChoiceMenuView() : menuView;
            menuView.MenuItems = new List<NavigationMenuItem>()
            {
                new NavigationMenuItem()
                {
                    GoesTo = GoToLoginView,
                    DisplayString = "login with an existing user"
                },
                new NavigationMenuItem()
                {
                    GoesTo = GoToRegisterUserView,
                    DisplayString = "create a new user"
                },
                new NavigationMenuItem()
                {
                    GoesTo = QuitProgram,
                    DisplayString = "quit program"
                }
            };
        }
        private void GoToLoginView()
        {
            InitializeLoginView();
            loginView.Display();
        }
        private void InitializeLoginView()
        {
            loginView = (loginView is null) ? new LoginView() : loginView;
            loginView.LoginValidationMethod = LoginUser;
            loginView.LoginSuccessCallback = GoToUserInterface;
            loginView.LoginFailCallback = GoToLoginMenu;
        }
        private void GoToUserInterface()
        {
            InitializeUserInterfaceMenu();
            menuView.Display();
        }
        private void InitializeUserInterfaceMenu()
        {
            menuView = (menuView is null) ? new MultiChoiceMenuView() : menuView;
            menuView.MenuItems = new List<NavigationMenuItem>()
            {
                new NavigationMenuItem()
                {
                    GoesTo = GoToPostEntryView,
                    DisplayString = "write a new entry"
                },
                new NavigationMenuItem()
                {
                    GoesTo = GoToDisplayAllEntriesView,
                    DisplayString = "view all entries"
                },
                new NavigationMenuItem()
                {
                    GoesTo = GoToDisplayUsersEntriesView,
                    DisplayString = "view all my entries"
                },
                new NavigationMenuItem()
                {
                    GoesTo = GoToLogOutUserView,
                    DisplayString = "log out user"
                },
                new NavigationMenuItem()
                {
                    GoesTo = QuitProgram,
                    DisplayString = "quit program"
                }
            };
        }
        private void GoToRegisterUserView()
        {
            InitializeRegisterUserView();
            registerUserView.Display();
        }
        private void InitializeRegisterUserView()
        {
            registerUserView = (registerUserView is null) ? new RegisterUserView() : registerUserView;
            registerUserView.UsernameValidation = registerUserView.ValidateUsername;
            registerUserView.PasswordValidation = registerUserView.ValidatePassword;
            registerUserView.AliasValidation = registerUserView.ValidateAlias;
            registerUserView.CheckIfUnique = IsUsernameUnique;
            registerUserView.RegisterUserCallback = CreateUser;
            registerUserView.NextView = GoToLoginMenu;
        }
        private void GoToPostEntryView()
        {
            InitializePostEntryView();
            postEntryView.Display();
        }
        private void InitializePostEntryView()
        {
            postEntryView = (postEntryView is null) ? new PostEntryView() : postEntryView;
            postEntryView.EntryValidation = postEntryView.ValidateEntryText;
            postEntryView.PostEntryCallback = AddEntryToDatabase;
            postEntryView.NextView = GoToUserInterface;
        }
        private void GoToDisplayAllEntriesView()
        {
            InitializeDisplayAllEntriesView();
            displayEntriesView.Display();
        }
        private void InitializeDisplayAllEntriesView()
        {
            displayEntriesView = (displayEntriesView is null) ? new DisplayEntriesView() : displayEntriesView;
            displayEntriesView.DisplayMessage = "Displaying all past entries by date in descending order";
            displayEntriesView.Entries = GetAllEntries();
            displayEntriesView.NextView = GoToUserInterface;
        }
        private void GoToDisplayUsersEntriesView()
        {
            InitializeDisplayUsersEntriesView();
            displayEntriesView.Display();
        }

        private void InitializeDisplayUsersEntriesView()
        {
            displayEntriesView = (displayEntriesView is null) ? new DisplayEntriesView() : displayEntriesView;
            displayEntriesView.DisplayMessage = "Displaying all your entries by date in descending order";
            displayEntriesView.Entries = GetUsersEntries();
            displayEntriesView.NextView = GoToUserInterface;
        }
        private void GoToLogOutUserView()
        {
            InitializeLogOutUserView();
            messageView.Display();
        }

        private void InitializeLogOutUserView()
        {
            CurrentAuthor = null;
            messageView = (messageView is null) ? new InfoMessageView() : messageView;
            messageView.Message = "You are now logged out";
            messageView.NextView = GoToMainMenu;
        }
        private void CreateUser(RegistrationInput input)
        {
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(input.Password);

            var author = new Author(input.Username, passwordHash, input.Alias);

            context.Authors.Add(author);
            context.SaveChanges();
        }
        private Result IsUsernameUnique(string userName)
        {
            var result = new Result();
            var query = context.Authors.Where(user => user.Username == userName);

            result.Success = !query.ToList().Any();

            if (result.Success)
            {
                return result;
            }
            else
            {
                result.ValidationMessages.Add("That username is already taken, please choose another");
                return result;
            }
        }
        private Result LoginUser(LoginInput input)
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
        private void AddEntryToDatabase(string entryText)
        {
            var entryToAdd = new Entry()
            {
                Id = Guid.NewGuid().ToString(),
                DateOfEntry = DateTime.Now,
                EntryText = entryText
            };
            if (CurrentAuthor.Entries == null)
            {
                CurrentAuthor.Entries = new List<Entry> { entryToAdd };
            }
            else
            {
                CurrentAuthor.Entries.Add(entryToAdd);
            }
            context.SaveChanges();

        }
        private void QuitProgram()
        {
        }
        private IEnumerable<Entry> GetAllEntries() => 
            from entry in context.Entries
            orderby entry.DateOfEntry ascending
            select entry;

        private IEnumerable<Entry> GetUsersEntries() =>
            from entry in context.Entries.ToList()
            where entry.Author.Id == CurrentAuthor.Id
            orderby entry.DateOfEntry ascending
            select entry;
        // query = context.Authors.Where(a => a.Username == input.Username)
    }
}
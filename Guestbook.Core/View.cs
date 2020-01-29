using System;

namespace Guestbook.Core
{
    internal class View
    {
        public View()
        {
        }
        public Action<string> SendToController { get; set; }
        public void PrintWelcome()
        {
            Console.WriteLine("Welcome to this guestbook!");
        }
        public void PrintLoginMenu()
        {
            Console.WriteLine("If you are an existing user press 1, to register a new user press 2, to quit press 3.");
        }
        public ConsoleKey GetConsoleKey()
        {
            return Console.ReadKey().Key;
        }
        public void Clear()
        {
            Console.Clear();
        }
    }
}
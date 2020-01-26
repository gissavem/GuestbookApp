using System;

namespace Guestbook.Console.Features.Login
{
    public class InputHandler
    {

        public Input Handle()
        {
            return new Input()
            {
                Username = TryUsername(),
                Password = TryPassword()
            };
        }
        private static string TryPassword()
        {
            System.Console.Write($"please enter your password: ");
            string password = null;
            while (true)
            {
                var key = System.Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter)
                    break;
                password += key.KeyChar;
                System.Console.Write("*");
            }
            System.Console.Clear();
            return password;
        }

        private static string TryUsername()
        {
            System.Console.Write($"please enter your username: ");
            return System.Console.ReadLine();
        }

    }
}
using System;

namespace Guestbook.Core.Features.Login
{
    public class InputHandler
    {
        /*
         * THESE METHODS ARE NOW IN THE VIEW. SHOULD THE VIEW BE EXPANDED TO SEVERAL CLASSES?
         */
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
                var key = Console.ReadKey(true);
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
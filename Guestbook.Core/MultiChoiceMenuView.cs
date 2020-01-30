using System;
using System.Collections.Generic;
using System.Text;

namespace Guestbook.Core
{
    class MultiChoiceMenuView : View
    {
        public List<NavigationMenuItem> MenuItems { get; set; }
        public void Display()
        {
            Clear();
            DisplayNavigationMenuItems();
            var input = ReTryParseKey();
            MenuItems[input - 1].GoesTo();
        }
        private void DisplayNavigationMenuItems()
        {
            for (int i = 0; i < MenuItems.Count; i++)
            {
                Console.WriteLine($"  Press {i + 1} for {MenuItems[i].DisplayString}.\n");
            }
        }
        private int ReTryParseKey()
        {
            char input;
            int inputAsInt = -1;
            while (inputAsInt > MenuItems.Count || inputAsInt < 1)
            {
                input = Console.ReadKey(true).KeyChar;
                try
                {
                    inputAsInt = (int)input;
                }
                catch (Exception)
                {
                    continue;
                }
            }
            return inputAsInt;
        }

    }
}

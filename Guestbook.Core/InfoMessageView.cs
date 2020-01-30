using System;
using System.Collections.Generic;
using System.Text;

namespace Guestbook.Core
{
    class InfoMessageView : View
    {
        public string Message { get; set; }
               
        public void Display()
        {
            Console.WriteLine(Message);
            Console.Write("Press any key to continue: ");
            Console.ReadKey(true);
            NextView();
        }
    }
}

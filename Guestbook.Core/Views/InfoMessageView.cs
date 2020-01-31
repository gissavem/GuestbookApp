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
            Clear();
            Console.WriteLine(Message);
            WaitForKeyPress();
            NextView();
        }
    }
}

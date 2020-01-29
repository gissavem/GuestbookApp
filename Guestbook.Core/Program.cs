using Guestbook.Core.Persistance;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guestbook.Core
{
    class Program
    {
        void Main()
        {
            var view = new View();
            var context = new EntryContext();
            context.Database.EnsureCreated();
            var controller = new Controller(context, view);
            controller.Run();
        }
        
    }
}

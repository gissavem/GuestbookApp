using Guestbook.Core.Persistance;

namespace Guestbook.Core
{
    class Program
    {
        void Main()
        {
            var context = new EntryContext();
            context.Database.EnsureCreated();
            var controller = new Controller(context);
            controller.Run();
        }
        
    }
}

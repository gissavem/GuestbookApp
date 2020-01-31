
namespace Guestbook.Core
{
    class Program
    {
        static void Main()
        {
            var context = new EntryContext();
            context.Database.EnsureCreated();
            var controller = new Controller(context);
            controller.Run();
        }
        
    }
}

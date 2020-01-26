using Guestbook.Core.Entities;

namespace Guestbook.Core.Features.Login
{
    public class CommandResult
    {
        public bool Success { get; set; }
        public Author Author { get; set; }
    }
}
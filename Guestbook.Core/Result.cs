using System.Collections.Generic;

namespace Guestbook.Core
{
    public class Result
    {
        
        public bool Success { get; set; } = true;
        public List<string> ValidationMessages { get; set; } = new List<string>();
        
    }
}

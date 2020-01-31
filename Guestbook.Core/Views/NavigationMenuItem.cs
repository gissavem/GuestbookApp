using System;
using System.Collections.Generic;
using System.Text;

namespace Guestbook.Core
{
    public class NavigationMenuItem
    {
        public Action GoesTo { get; set; }
        public string DisplayString { get; set; }
    }
}

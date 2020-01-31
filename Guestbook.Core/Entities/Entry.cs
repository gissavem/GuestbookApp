using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Guestbook.Core
{
    public class Entry
    {
        [Column("id")]
        public string Id { get; set; }
        public string EntryText { get; set; }
        public DateTime DateOfEntry { get; set; }
        public virtual Author Author { get; set; }
    }
}

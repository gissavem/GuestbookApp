using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AzureLab
{
    public class Entries
    {
        [Column("id")]
        public string Id { get; set; }
        public string EntryText { get; set; }
        public DateTime DateOfEntry { get; set; }
        public virtual Authors Author { get; set; }
    }
}

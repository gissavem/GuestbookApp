using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AzureLab
{
    public class Author
    {
        [Column("id")]
        public string Id { get; set; }
        public string Alias { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public virtual ICollection<Entry> Entries { get; set; }
    }
}
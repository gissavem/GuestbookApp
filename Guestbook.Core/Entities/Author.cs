using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Guestbook.Core.Entities
{
    public class Author
    {
        public Author()
        {
            
        }

        public Author(string userName, string passwordHash, string alias)
        {
            
            if (string.IsNullOrWhiteSpace(userName))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(userName));
            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(passwordHash));
            if (string.IsNullOrWhiteSpace(alias))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(alias));
            Username = userName;
            PasswordHash = passwordHash;
            Alias = alias;
            Id = Guid.NewGuid().ToString();
        }
        [Column("id")]
        public string Id { get; set; }
        public string Alias { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public virtual ICollection<Entry> Entries { get; set; }
    }
}
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzureLab
{
    class EntryContext : DbContext
    {
        private string connectionString = "https://localhost:8081";
        private string primaryKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
        private string databaseName = "EntryDatabase";

        public DbSet<Entry> Entries { get; set; }
        public DbSet<Author> Authors { get; set; }

        public EntryContext()
        { 
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseCosmos(connectionString, primaryKey, databaseName);
            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}

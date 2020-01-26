using Guestbook.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Guestbook.Core.Persistance
{
    public class EntryContext : DbContext
    {
        private const string ConnectionString = "https://localhost:8081";
        private const string PrimaryKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
        private const string DatabaseName = "EntryDatabase";

        public DbSet<Entry> Entries { get; set; }
        public DbSet<Author> Authors { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseCosmos(ConnectionString, PrimaryKey, DatabaseName);
            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}

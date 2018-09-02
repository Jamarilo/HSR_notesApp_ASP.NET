using System;
using Microsoft.EntityFrameworkCore;

namespace NoteApp.Models
{
    // This represents the connection to the Database
    public class noteAppContext : DbContext
    {
        public noteAppContext(DbContextOptions<noteAppContext> options)
            : base(options)
        {
        }

        public DbSet<NoteApp.Models.Note> Note { get; set; }
    }
}

using System;
using Microsoft.EntityFrameworkCore;

namespace NoteApp.Models
{
    // This represents the connection to the Database
    public class NoteDBContext : DbContext
    {
        public NoteDBContext(DbContextOptions<NoteDBContext> options)
            : base(options)
        {
        }

        public DbSet<NoteApp.Models.Note> Note { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NoteApp.Models;
using System;
using System.Linq;

namespace Database.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new NoteDBContext(
                serviceProvider.GetRequiredService<DbContextOptions<NoteDBContext>>()))
            {

                // Look for any notes
                if (context.Note.Any())
                {
                    return;   // DB has been seeded
                }

                context.Note.AddRange(
                     new Note
                     {
                         CreatedDate = DateTime.Parse("2018-9-2"),
                         Title = "Erste Notiz",
                         Text = "Das ist der Text der ersten Note",
                         Importance = 5,
                         FinishDate = DateTime.Parse("2017-9-2"),
                         Finished = true
                     },
                    new Note
                    {
                        CreatedDate = DateTime.Parse("2018-1-9"),
                        Title = "Zweite Notiz",
                        Text = "Das ist der Text der zweiten Note",
                        Importance = 1,
                        FinishDate = DateTime.Parse("2018-11-9")
                    }

                );
                context.SaveChanges();
            }
        }
    }
}
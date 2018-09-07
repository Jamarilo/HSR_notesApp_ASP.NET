﻿using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NoteApp.Models;

namespace notizenapp.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new noteAppContext(
                serviceProvider.GetRequiredService<DbContextOptions<noteAppContext>>()))
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
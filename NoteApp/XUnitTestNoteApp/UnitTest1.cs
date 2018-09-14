using System;
using Xunit;
using NoteApp.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using NoteApp.Services;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

using System.Threading.Tasks;




namespace XUnitTestNoteApp
{
    public class UnitTest1
    {

    private readonly ServiceProvider _serviceProvider;
    private readonly NoteDBContext _dbContext;

    public UnitTest1()
        {
            var services = new ServiceCollection();
            services.AddDbContext<NoteDBContext>(opt => opt.UseInMemoryDatabase());
            services.AddSingleton<IStyle>(sortOrder => new Style());
            services.AddSingleton<ISortOrder>(sortOrder => new SortOrder());
            services.AddSingleton<IFilter>(filter => new Filter());


            _serviceProvider = services.BuildServiceProvider();
            _dbContext = _serviceProvider.GetService<NoteDBContext>();

        }

        [Fact]
        public void updateANote()
        {

            Note note = Note.CreateNew();
            note.Title = "Zweite Notiz";
            note.Text = "Das ist der elende Text";

            _dbContext.Add(note);
            _dbContext.SaveChanges();


            var query = from n in _dbContext.Note
                        where n.Title == "Zweite Notiz"
                        select n;
            int count = (from x in _dbContext.Note select x).Count();

           
            System.Diagnostics.Debug.WriteLine("RESULTAT " + count);

            var noteSQlResultrat = query.Single();

            
            System.Diagnostics.Debug.WriteLine("RESULTAT " + noteSQlResultrat.Text);
            //var data = _dbContext.Database.SqlQuery(query);

            //_dbContext.Note.FirstOrDefaultAsync

            //Note note = Note.CreateNew();
            //note.Text = " EliseoTest";

        }
    }
}

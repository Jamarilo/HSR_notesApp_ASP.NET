using Xunit;
using NoteApp.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using NoteApp.Services;
using System.Linq;
using NoteApp.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace XUnitTestNoteApp
{
    public class UnitTest1
    {

        private readonly ServiceProvider _serviceProvider;
        private readonly NoteDBContext _dbContext;
        private ServiceCollection services = new ServiceCollection();

        public UnitTest1()
        {
            services = new ServiceCollection();
            services.AddDbContext<NoteDBContext>(opt => opt.UseInMemoryDatabase());
            services.AddSingleton<IStyle>(sortOrder => new Style());
            services.AddSingleton<ISortOrder>(sortOrder => new SortOrder());
            services.AddSingleton<IFilter>(filter => new Filter());


            _serviceProvider = services.BuildServiceProvider();
            _dbContext = _serviceProvider.GetService<NoteDBContext>();

            //services.AddMvc();

            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

        }

        [Fact]
        public void updateANote()
        {

            Note note = Note.CreateNew();
            note.ID = 1;
            note.Title = "Titel 1.Notiz";
            note.Text = "Text der 1. Notiz";

            _dbContext.Add(note);
            _dbContext.SaveChanges();
            var style = _serviceProvider.GetService<IStyle>();



            var controllerTest = new NoteController(_dbContext, style);
            IActionResult resultat = controllerTest.Edit(1);
            resultat.ToString();
            System.Diagnostics.Debug.WriteLine("RESULTAT " + resultat.ToString());

            string modifizierterTitle = "Modifizierter Titel";

            var modifiedNode = Note.CreateNew();
            modifiedNode.ID = 1;
            modifiedNode.Title = modifizierterTitle;
            modifiedNode.Text = "Modifiziertet Text";
            controllerTest.Edit(1, modifiedNode);


            var query = from n in _dbContext.Note
                        where n.Title == modifizierterTitle
                        select n;

            var updatedNote = query.Single();

            //int count = (from x in _dbContext.Note select x).Count();

            Assert.Equal(1, updatedNote.ID);
            Assert.Equal(modifizierterTitle, updatedNote.Title);

            //System.Diagnostics.Debug.WriteLine("RESULTAT " + count);

            //var noteSQlResultrat = query.Single();


            //System.Diagnostics.Debug.WriteLine("RESULTAT " + noteSQlResultrat.Text);
            //var data = _dbContext.Database.SqlQuery(query);

            //_dbContext.Note.FirstOrDefaultAsync

            //Note note = Note.CreateNew();
            //note.Text = " EliseoTest";

        }
    }
}

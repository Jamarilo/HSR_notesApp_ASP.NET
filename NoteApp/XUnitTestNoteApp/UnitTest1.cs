using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NoteApp.Controllers;
using NoteApp.Models;
using NoteApp.Services;
using System.Linq;
using System.Net;
using Xunit;

namespace XUnitTestNoteApp
{
    public class NoteAppUnitTests
    {

        private readonly ServiceProvider _serviceProvider;
        private readonly NoteDBContext _dbContext;
        private ServiceCollection services = new ServiceCollection();

        public NoteAppUnitTests()
        {
            services = new ServiceCollection();
            services.AddDbContext<NoteDBContext>(opt => opt.UseInMemoryDatabase());
            services.AddSingleton<IStyle>(sortOrder => new Style());
            services.AddSingleton<ISortOrder>(sortOrder => new SortOrder());
            services.AddSingleton<IFilter>(filter => new Filter());

            _serviceProvider = services.BuildServiceProvider();
            _dbContext = _serviceProvider.GetService<NoteDBContext>();
        }

        /*
         * Ueberpruefen ob ein Update einer Notiz funktioniert
         */
        [Fact]
        public void TestUpdateANote()
        {
            //DB mit Daten füllen
            int noteID = 1;
            FillDBWithNode(noteID);

            var style = _serviceProvider.GetService<IStyle>();
            var noteController = new NoteController(_dbContext, style);

            //Note editieren
            IActionResult result = noteController.Edit(noteID);
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            Note noteModel = Assert.IsType<Note>(viewResult.Model);

            string modifizierterTitle = "Modifizierter Titel";
            noteModel.Title = modifizierterTitle;
            noteController.Edit(1, noteModel);

            //DB query auf modifizierte Note
            var query = from n in _dbContext.Note
                        where n.ID == noteID
                        select n;

            var updatedNote = query.Single();

            Assert.Equal(1, updatedNote.ID);
            Assert.Equal(modifizierterTitle, updatedNote.Title);
        }


        /*
         * Ueberpruefen ob 404 zurück gegeben wird wenn die eine Notiz 
         * nicht vorhanden ist
         */
        [Fact]
        public void TestGetting404Error()
        {       
            var style = _serviceProvider.GetService<IStyle>();
            var noteController = new NoteController(_dbContext, style);

            int noteId = 99;
            var result = noteController.Edit(noteId);
            StatusCodeResult statusCodeResult = Assert.IsAssignableFrom<StatusCodeResult>(result);

            Assert.Equal((int)HttpStatusCode.NotFound, statusCodeResult.StatusCode);
        }


        /*
         * Füllt die DB mit einem neuen Datensatz
         */
        private void FillDBWithNode(int id)
        {
            Note note = Note.CreateNew();
            note.ID = id;
            note.Title = "Titel 1.Notiz";
            note.Text = "Text der 1. Notiz";

            _dbContext.Add(note);
            _dbContext.SaveChanges();
        }
    }
}

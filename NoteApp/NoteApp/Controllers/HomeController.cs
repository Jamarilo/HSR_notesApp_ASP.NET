using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NoteApp.Models;
using NoteApp.ViewModels;

namespace NoteApp.Controllers

{
    public class HomeController : Controller
    {
        private readonly NoteDBContext _context;

        public HomeController(NoteDBContext context)
        {
            _context = context;
        }

        //Diese Methode wird automatisch als erstes Aufgerufen aufgrund der 
        //Zeile 60 in Startup.cs
        public IActionResult Index()
        {

            //Nodes auslesen
            NoteViewModel viewModelWithNotes = fillModelWithNodes();


            //foreach (Note n in _context.Note)
            //{
            //    Console.WriteLine(n.Text);
            //    return View(n);

            //    //return Content("Test");
            //}

            return View(viewModelWithNotes);
        }

        //Füllt das Model welches dann an die View übergeben wird
        private NoteViewModel fillModelWithNodes()
        {
            var viewModelWithNotes = new NoteViewModel();

            var allNodes = from n in _context.Note
                           select n;

            viewModelWithNotes.notes = allNodes.ToList();

            return viewModelWithNotes;

        }

        //Durch das einfügen dieser "leeren" Methode wird die View Create aufgerufen
        //Nur Gott alleine weiss warum
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(NewNoteViewModel newNote)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            Note note = new Note
            {
                Title = newNote.Title,
                Text = newNote.Text,
                Importance = newNote.Importance,
                FinishDate = newNote.FinishDate
            };

            note.CreatedDate = DateTime.Now;
            _context.Add(note);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

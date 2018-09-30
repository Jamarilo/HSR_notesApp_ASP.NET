using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NoteApp.Models;
using NoteApp.Services;
using System;
using System.Linq;

namespace NoteApp.Controllers
{
    /*
     * Controller für Notizen
     */
    public class NoteController : Controller {


        private readonly NoteDBContext _noteDBContext;
        private IStyle style;

        public NoteController(NoteDBContext noteDBContext, IStyle style) {
            this._noteDBContext = noteDBContext;
            this.style = style;
        }

        /*
         * Editieren einer Notiz
         * Views/Note/Edit.cshtml
         */    
        [HttpGet]    
        public IActionResult Edit(int? id)
        {
            ViewData["CurrentStyle"] = style.getCurrent();
            if (id == null || id <= 0) return NotFound();

            try
            {
                Note note = _noteDBContext.Note.Single(n => n.ID == id);
                return View(note);
            }
            catch (ArgumentNullException) { return NotFound(); }
            catch (InvalidOperationException) { return NotFound(); }                     
        }

        /*
         * Speichern einer editierten Notiz
         * Weiterleitung nach Home falls Valid
         * Views/Note/Edit.cshtml
         */
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int? id, [Bind("ID,CreatedDate,Title,Text,Importance,FinishDate,Finished")] Note note)
        {
            ViewData["CurrentStyle"] = style.getCurrent();
            if (id != note.ID) { return BadRequest(); }

            if (ModelState.IsValid)
            {
                try
                {
                    _noteDBContext.Update(note);
                    _noteDBContext.SaveChanges();
                }
                catch (DbUpdateConcurrencyException) { return BadRequest(); }
                catch (DbUpdateException) { return BadRequest(); }

                return RedirectToAction("Index", "Home");
            }
            return View(note);
        }

        /*
         * Neue Notiz erfassen
         * Views/Note/Create.cshtml
         */
        [HttpGet]
        public IActionResult Create()
        {
            ViewData["CurrentStyle"] = style.getCurrent();
            Note note = Note.CreateNew();
            return View(note);
        }

        /*
         * Neue Notiz speichern
         * Weiterleitung nach Home falls Valid
         * Views/Note/Create.cshtml
         */
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("CreatedDate,Title,Text,Importance,FinishDate,Finished")] Note note)
        {
            ViewData["CurrentStyle"] = style.getCurrent();
            if (note.ID > 0) return BadRequest();

            if (ModelState.IsValid) {
                try
                {
                    note.CreatedDate = DateTime.Now;
                    _noteDBContext.Add(note);
                    _noteDBContext.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
                catch (DbUpdateConcurrencyException) { return BadRequest(); }
                catch (DbUpdateException) { return BadRequest(); }
            }
            return View(note);
        }

    }
}
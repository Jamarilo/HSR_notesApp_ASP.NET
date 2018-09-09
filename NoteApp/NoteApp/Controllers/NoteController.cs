using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NoteApp.Models;

namespace NoteApp.Controllers
{
    /*
     * Kontroller für Notizen
     */
    public class NoteController : Controller {

        private readonly NoteDBContext noteDBContext;

        public NoteController(NoteDBContext noteDBContext) {
            this.noteDBContext = noteDBContext;
        }

        /*
         * Editieren einer Notiz
         * Views/Note/Edit.cshtml
         */    
        [HttpGet]    
        public IActionResult Edit(int? id)
        {
            if (id == null || id <= 0) return NotFound();

            try
            {
                Note note = noteDBContext.Note.Single(n => n.ID == id);
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
            if (id != note.ID) { return BadRequest(); }

            if (ModelState.IsValid)
            {
                try
                {
                    noteDBContext.Update(note);
                    noteDBContext.SaveChanges();
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
            if (note.ID > 0) return BadRequest();

            if (ModelState.IsValid) {
                try
                {
                    note.CreatedDate = DateTime.Now;
                    noteDBContext.Add(note);
                    noteDBContext.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
                catch (DbUpdateConcurrencyException) { return BadRequest(); }
                catch (DbUpdateException) { return BadRequest(); }
            }
            return View(note);
        }

    }
}
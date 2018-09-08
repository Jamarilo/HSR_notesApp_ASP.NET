using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NoteApp.Models;

namespace NoteApp.Controllers
{
    public class NoteController : Controller {

        private readonly NoteDBContext noteDBContext;

        public NoteController(NoteDBContext noteDBContext) {
            this.noteDBContext = noteDBContext;
        }

        [HttpGet]    
        public IActionResult Edit(int? id)
        {
            int? noteId = id;
            if(noteId == null || noteId == 0) {
                return NotFound();
            }
            var note = noteDBContext.Note.SingleOrDefault(n => n.ID == noteId);
            if (note == null){
                return NotFound();
            }

            //Viewbag: Werte z.B. note können in den Viewbag gespeichert werden um in der View drauf zuzugreifen.
            //wird eventuel nicht gebraucht
            ViewBag.Importance = note.Importance;

            return View(note);
        }

        [HttpPost]
        public IActionResult Edit(int? id, [Bind("ID,CreatedDate,Title,Text,Importance,FinishDate,Finished")] Note note)
        {
            if (id != note.ID)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    noteDBContext.Update(note);
                    noteDBContext.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (NoteExists(note))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Home");
            }
            return View(note);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        private bool NoteExists(Note note)
        {
            return noteDBContext.Note.Any(n => n.ID == note.ID);
        }
  
    }
}
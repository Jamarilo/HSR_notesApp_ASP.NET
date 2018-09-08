using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace NoteApp.Controllers
{
    public class NoteController : Controller
    {
        public IActionResult Edit(int noteId)
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NoteApp.Models;
using NoteApp.Services;
using NoteApp.ViewModels;

namespace NoteApp.Controllers

{
    public class HomeController : Controller
    {
        private readonly NoteDBContext _context;
        private ISortOrder _sortOrder;
        private IFilter _filter;

        public HomeController(NoteDBContext context, ISortOrder sortOrder, IFilter filter)
        {
            _context = context;
            _sortOrder = sortOrder;
            _filter = filter;
        }

        //Diese Methode wird automatisch als erstes Aufgerufen aufgrund der 
        //Zeile 60 in Startup.cs
        public IActionResult Index(string sortOrder, string hideFinished)
        {
            if (sortOrder != null) {
                _sortOrder.Set(sortOrder);
            }
            ViewData["SortOrderFinishText"] = getSortOrderText("Finish Date", "finish");
            ViewData["SortOrderFinishValue"] = getgetSortOrderValue("finish");

            ViewData["SortOrderCreatedText"] = getSortOrderText("Created Date", "created");
            ViewData["SortOrderCreatedValue"] = getgetSortOrderValue("created");

            ViewData["SortOrderImportanceText"] = getSortOrderText("Importance", "importance");
            ViewData["SortOrderImportanceValue"] = getgetSortOrderValue("importance");


            if (hideFinished != null)
            {
                bool hide = hideFinished.Equals("true");
                _filter.SetHideFinished(hide);
            }
            ViewData["HideFinishedText"] = getHideFinishedText();
            ViewData["HideFinishedValue"] = getHideFinishedValue();

            //Nodes auslesen
            NoteViewModel viewModelWithNotes = fillModelWithNodes();
     
            return View(viewModelWithNotes);
        }

        private string getSortOrderText(string text, string sortOrder)
        {
            if (_sortOrder.Get().StartsWith(sortOrder))
            {
                if (_sortOrder.Get().EndsWith("asc"))
                {
                    return text + " ▼";
                }
                else
                {
                    return text + " ▲";
                }
            }
            return text;
        }

        private string getgetSortOrderValue(string sortOrder)
        {
            if (_sortOrder.Get().StartsWith(sortOrder) && _sortOrder.Get().EndsWith("asc"))
            {
                return sortOrder + "_desc";
            }
            else
            {
                return sortOrder + "_asc";
            }
        }

        private string getHideFinishedText()
        {
            if (_filter.IsHideFinished())
            {
                return "Show Finished";
            }
            else
            {
                return "Hide Finished";
            }
        }

        private string getHideFinishedValue()
        {
            return (!_filter.IsHideFinished()).ToString().ToLower();
        }

        //Füllt das Model welches dann an die View übergeben wird
        private NoteViewModel fillModelWithNodes()
        {
            var viewModelWithNotes = new NoteViewModel();

            var nodes = from n in _context.Note
                           select n;

            switch (_sortOrder.Get())
            {
                case "created_desc":
                    nodes = nodes.OrderByDescending(n => n.CreatedDate);
                    break;
                case "created_asc":
                    nodes = nodes.OrderBy(n => n.Importance);
                    break;

                case "importance_desc":
                    nodes = nodes.OrderByDescending(n => n.Importance);
                    break;
                case "importance_asc":
                    nodes = nodes.OrderBy(n => n.Importance);
                    break;

                case "finish_desc":
                    nodes = nodes.OrderByDescending(n => n.FinishDate);
                    break;
                default:
                    nodes = nodes.OrderBy(n => n.FinishDate);
                    break;
            }

            if (_filter.IsHideFinished())
            {
                nodes = nodes.Where(n => n.Finished == false);
            }

            viewModelWithNotes.notes = nodes.ToList();

            return viewModelWithNotes;

        }
 
    }
}

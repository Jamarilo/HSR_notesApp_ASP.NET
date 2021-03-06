﻿using Microsoft.AspNetCore.Mvc;
using NoteApp.Models;
using NoteApp.Services;
using System.Linq;

namespace NoteApp.Controllers

{
    public class HomeController : Controller
    {
        private readonly NoteDBContext _noteDBContext;
        private IStyle _style;
        private ISortOrder _sortOrder;
        private IFilter _filter;

        public HomeController(NoteDBContext noteDBContext, IStyle style, ISortOrder sortOrder, IFilter filter)
        {
            _noteDBContext = noteDBContext;
            _style = style;
            _sortOrder = sortOrder;
            _filter = filter;
        }

        public IActionResult Index(string style, string sortOrder, string hideFinished)
        {
            if (style != null && !style.Equals(_style.getCurrent()))
            {
                _style.change();
            }
            ViewData["CurrentStyle"] = _style.getCurrent();
            ViewData["NextStyle"] = _style.getNext();

            if (sortOrder != null) {
                _sortOrder.Set(sortOrder);
            }
            ViewData["SortOrderFinishText"] = getSortOrderText("Finish Date", "finish");
            ViewData["SortOrderFinishValue"] = getSortOrderValue("finish");

            ViewData["SortOrderCreatedText"] = getSortOrderText("Created Date", "created");
            ViewData["SortOrderCreatedValue"] = getSortOrderValue("created");

            ViewData["SortOrderImportanceText"] = getSortOrderText("Importance", "importance");
            ViewData["SortOrderImportanceValue"] = getSortOrderValue("importance");


            if (hideFinished != null)
            {
                bool hide = hideFinished.Equals("true");
                _filter.SetHideFinished(hide);
            }
            ViewData["HideFinishedText"] = getHideFinishedText();
            ViewData["HideFinishedValue"] = getHideFinishedValue();

            //Nodes auslesen
            Notes viewModelWithNotes = FillModelWithNodes();
     
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

        private string getSortOrderValue(string sortOrder)
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
        private Notes FillModelWithNodes()
        {
            var viewModelWithNotes = new Notes();

            var nodes = from n in _noteDBContext.Note
                           select n;

            switch (_sortOrder.Get())
            {
                case "created_desc":
                    nodes = nodes.OrderByDescending(n => n.CreatedDate);
                    break;
                case "created_asc":
                    nodes = nodes.OrderBy(n => n.CreatedDate);
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

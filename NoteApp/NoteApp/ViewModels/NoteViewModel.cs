using NoteApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoteApp.ViewModels
{
    /*Im Video wird dieses Konzept ab der ca 50min gezeigt
     *Es geht darum das man quasi ein Model erstellt welches in
     * unserem Fall eine Liste von meherern Notes halten kann.
     * Man kann dann der View nur diese Objekt übergeben und kann es
     * dann in der View bearbeiten
     */        
    public class NoteViewModel
    {
        public List<Note> notes { get; set; }
    }
}

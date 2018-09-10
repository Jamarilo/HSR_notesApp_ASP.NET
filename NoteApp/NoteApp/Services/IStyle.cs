using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoteApp.Services
{
    public interface IStyle
    {
        void change();

        string getCurrent();

        string getNext();

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoteApp.Services
{
    public interface ISortOrder
    {
        void Set(string sortOrder);
   
        string Get();
    }
}

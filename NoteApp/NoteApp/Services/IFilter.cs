using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoteApp.Services
{
    public interface IFilter
    {
        bool IsHideFinished();

        void SetHideFinished(bool hideFinished);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoteApp.Services
{
    public class Filter : IFilter
    {
        private bool hideFinished;

        public Filter()
        {
            hideFinished = false;
        }

        public bool IsHideFinished()
        {
            return this.hideFinished;
        }

        public void SetHideFinished(bool hideFinished)
        {
            this.hideFinished = hideFinished;
        }
    }
}

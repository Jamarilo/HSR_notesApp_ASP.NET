using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoteApp.Services
{
    public class SortOrder : ISortOrder
    {
        private string sortOrder;

        public SortOrder()
        {
            this.sortOrder = "finish_asc";
        }

        public string Get()
        {
            return this.sortOrder;
        }

        public void Set(string sortOrder)
        {
            this.sortOrder = sortOrder;
        }
    }
}

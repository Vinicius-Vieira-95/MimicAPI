using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pratice.Helpers
{
    public class PaginationList<T> : List<T>
    {
        public Pagination Pagination { get; set; }

    }
}

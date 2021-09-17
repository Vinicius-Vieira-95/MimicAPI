using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pratice.Helpers
{
    public class Pagination
    {
        public int Page { get; set; }
        public int QuantPages { get; set; }
        public int TotalRegisters { get; set; }
        public int totalPages { get; set; }
    }
}

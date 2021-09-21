using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pratice.Models.DTO
{
    public class WordDTO : BaseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Record { get; set; }
        public bool Active { get; set; }
        public DateTime Create { get; set; }
        public DateTime? Update { get; set; }

    }
}

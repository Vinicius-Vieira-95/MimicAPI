using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pratice.Models
{
    public class Word
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Record { get; set; }
        public bool Active { get; set; }
        public DateTime Create { get; set; }
        public DateTime? Update { get; set; }
    }
}

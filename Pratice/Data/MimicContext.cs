using Microsoft.EntityFrameworkCore;
using Pratice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pratice.Data
{
    public class MimicContext : DbContext
    {
        public MimicContext(DbContextOptions<MimicContext> options) : base(options)
        {
        }

        public DbSet<Word> Words { get; set; } //envia dados ?? requisição??

    }

}

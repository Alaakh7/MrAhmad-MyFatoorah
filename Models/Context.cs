using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options): base(options)
        {
        }
        public DbSet<ValidationCode> ValidationCodes { get; set; }
        public DbSet<Attempt> Attempts { get; set; }
    }
}

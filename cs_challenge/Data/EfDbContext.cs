using cs_challenge.Entities;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cs_challenge.Data
{
    public class EfDbContext : DbContext
    {
        public EfDbContext(AbstractContext context) : base(context.Options)
        {
        }

        public DbSet<User> Users { get; set; }
    }

    public abstract class AbstractContext
    {
        public DbContextOptions Options { get; set; }
    }

    public class InMemoryContext : AbstractContext
    {
        public InMemoryContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder();
            optionsBuilder.UseInMemoryDatabase();

            Options = optionsBuilder.Options;
        }
    }
}
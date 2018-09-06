using Inspur.Ecp.Caf.EntityFramework.EFAdaptor;
using Microsoft.EntityFrameworkCore;
using ODataDemo.Models;

namespace ODataDemo.Repository.DbContext
{
    public class ODataDbContext:CafDbContext
    {

        public DbSet <Product> Product { get; set; }
        public DbSet <Supplier> Supplier { get; set; }
        public DbSet<SalesOrder> SalesOrder { get; set; }

        public ODataDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {

        }

        public override void OnCustomModelCreating(ModelBuilder modelBuilder)
        {
            base.OnCustomModelCreating(modelBuilder);
        }

    }
}

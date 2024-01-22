using DocSearch.Docs.DataAccess.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace DocSearch.Docs.DataAccess.Data
{
    public class DoctorsDbContext : DbContext
    {
        public DbSet<Doctor> Doctors { get; set; }

        public DoctorsDbContext(DbContextOptions<DoctorsDbContext> options) : base(options) 
        {
            if (!Doctors.Any())
            {
                DbInitializer.SeedData(this);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }
    }
}

using DocSearch.Docs.DataAccess.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DocSearch.Docs.DataAccess.Data
{
    public static class DbInitializer
    {
        public static void SeedData(DoctorsDbContext context)
        {
            var doctors = new List<Doctor>
            {
                new Doctor { Name = "Dr. Smith", City = "City1", Address = "Address1" },
                new Doctor { Name = "Dr. Johnson", City = "City2", Address = "Address2" },
                new Doctor { Name = "Dr. Brown", City = "City3", Address = "Address3" },
            };

            context.Doctors.AddRange(doctors);
            context.SaveChanges();            
        }
    }
}

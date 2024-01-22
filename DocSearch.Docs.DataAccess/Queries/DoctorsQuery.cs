using DocSearch.Docs.DataAccess.Data;
using DocSearch.Docs.DataAccess.Data.Entities;
using HotChocolate;

namespace DocSearch.Docs.DataAccess.Queries
{
    public class DoctorsQuery
    {
        [UseFiltering]
        [UseSorting]
        public IQueryable<Doctor> GetDoctors([Service] DoctorsDbContext context)
            => context.Doctors;
        
        public Doctor? GetDoctorById(int id, [Service] DoctorsDbContext context)
            => context.Doctors.FirstOrDefault(d => d.Id == id);
    }
}

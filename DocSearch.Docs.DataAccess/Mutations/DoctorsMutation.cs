using DocSearch.Docs.DataAccess.Data;
using DocSearch.Docs.DataAccess.Data.Entities;
using HotChocolate;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace DocSearch.Docs.DataAccess.Mutations
{
    public class DoctorsMutation
    {
        public async Task<Doctor> AddDoctor(Doctor doctor, [Service] DoctorsDbContext context)
        {
            context.Doctors.Add(doctor);

            await context.SaveChangesAsync();

            return doctor;
        }

        public async Task<int> DeleteDoctor([GraphQLName("doctorId")] int id, [Service] DoctorsDbContext context)
        {
            var doctor = context.Doctors.FirstOrDefault(d => d.Id == id);

            if (doctor == null)
            {
                return 0;
            }

            context.Remove(doctor);

            await context.SaveChangesAsync();

            return id;
        }

        public async Task<Doctor> UpdateDoctor(Doctor updatedDoctor, [Service] DoctorsDbContext context)
        {
            context.Attach(updatedDoctor);
            context.Entry(updatedDoctor).State = EntityState.Modified;

            await context.SaveChangesAsync();

            return updatedDoctor;
        }
    }
}

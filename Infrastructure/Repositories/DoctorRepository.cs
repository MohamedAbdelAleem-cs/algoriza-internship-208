using Core.Interfaces;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class DoctorRepository : IDoctorRepository
    {

        protected AppDbContext _Context;
        public DoctorRepository(AppDbContext context)
        {
            _Context = context;
        }

        public async Task<IEnumerable<Doctor>> GetAllAsync()
        {
            var doctors= await _Context.Doctors.Include(d=>d.Specialization).ToListAsync();
            return doctors;
        }

        public async Task<Doctor> GetByIdAsync(int id)
        {
            var res= await _Context.Doctors.Include(d=>d.Specialization).FirstOrDefaultAsync(d=>d.Id==id); 
            return res;
        }


        public async Task<bool> AddDoctorsAsync(Doctor doctor)
        {
            try
            {
                await _Context.Doctors.AddAsync(doctor);
                await _Context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }



        public async Task<bool> UpdateDoctorAsync(Doctor doctor)
        {
            _Context.Doctors.Update(doctor);
            int affected=await _Context.SaveChangesAsync();
            return affected>0;
        }
        public async Task<bool> DeleteDoctorAsync(int id)
        {

            try
            {
                var doctor = await _Context.Doctors.FindAsync(id);
                if (doctor == null) return false;

                _Context.Remove(doctor);
                int affected = await _Context.SaveChangesAsync();
                return affected > 0;
            }
            catch
            {

                return false;
            }
        }


        public async Task<IEnumerable<Appointment>> GetDoctorAppointments(int id)
        {
            var doctor = await _Context.Doctors.Include(d => d.Appointments).ThenInclude(A=>A.Times).FirstOrDefaultAsync(d=>d.Id==id);
            return doctor.Appointments;
        }

        public async Task<IEnumerable<Doctor>> GetAllDoctorsWithSpecializationAndAppointments()
        {
            var doctors = await _Context.Doctors.Include(d => d.Specialization).Include(d => d.Appointments).ThenInclude(A=>A.Times).ToListAsync();
            return doctors;
        }

        public async Task<int> GetDoctorIdUsingUserId(string userId)
        {
            var doc = await _Context.Doctors.FirstOrDefaultAsync(d => d.UserId == userId);
            return doc.Id;
        }
    }
}

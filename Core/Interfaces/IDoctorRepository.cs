using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IDoctorRepository
    {
        Task<IEnumerable<Doctor>> GetAllAsync();
        Task<Doctor> GetByIdAsync(int id);
        Task<bool> AddDoctorsAsync(Doctor doctor);
        Task<bool> UpdateDoctorAsync(Doctor doctor);
        Task<bool> DeleteDoctorAsync(int id);

        Task<IEnumerable<Appointment>> GetDoctorAppointments(int id);

        Task<IEnumerable<Doctor>> GetAllDoctorsWithSpecializationAndAppointments();

        Task<int> GetDoctorIdUsingUserId(string userId);

    }
}

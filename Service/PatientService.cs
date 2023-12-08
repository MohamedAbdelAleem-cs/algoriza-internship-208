using Core.DTOS.DoctorDTO;
using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Identity;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class PatientService : IPatientService
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public PatientService(IDoctorRepository doctorRepository, UserManager<ApplicationUser> userManager)
        {
            _doctorRepository = doctorRepository;
            _userManager = userManager;
        }

        public async Task<IEnumerable<DoctorDisplay>> GetAllDoctorsAsync()
        {
            var doctors = await _doctorRepository.GetAllDoctorsWithSpecializationAndAppointments();
            List<DoctorDisplay> res = new List<DoctorDisplay>();
            foreach (var doctor in doctors)
            {
                var user = await _userManager.FindByIdAsync(doctor.UserId);
                var aps = doctor.Appointments.Select(appointment =>
                {
                    var times = appointment.Times.Select(time => time.Time).ToList();

                    return new AppointmentDisplay
                    {
                        Day = appointment.DayOfWeek,
                        Time = times
                    };
                }).ToList();
                res.Add(new DoctorDisplay
                {
                    ID=doctor.Id,
                    Image = user.Image,
                    FullName = user.FirstName + " " + user.LastName,
                    Email = user.Email,
                    Appointments = aps,
                    Gender = user.Gender,
                    Phone = user.PhoneNumber,
                    Price = doctor.Price,
                    Specialization = doctor.Specialization,
                });
            }
            return res;

        }
    }
}

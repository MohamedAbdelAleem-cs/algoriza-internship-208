using Core.DTOS;
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
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly AppDbContext _context;

        public AppointmentRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<bool> AddAppointments(AppointmentDataAdd appointmentData, int doctorId)
        {

            

            if (appointmentData.DayOfWeeks.Count != appointmentData.TimesList.Count)
            {
                return false;
            }

            foreach (var (dayOfWeek, times) in appointmentData.DayOfWeeks.Zip(appointmentData.TimesList))
            {
                var existingAppointment = await _context.Appointments
                    .Include(a => a.Times)
                    .FirstOrDefaultAsync(a => a.DoctorId == doctorId && a.DayOfWeek == dayOfWeek);

                if (existingAppointment == null)
                {
                    var newAppointment = new Appointment
                    {
                        DoctorId = doctorId,
                        DayOfWeek = dayOfWeek,
                        Times = times.Select(time => new Times { Time = time }).ToList()
                    };

                    _context.Appointments.Add(newAppointment);
                }
                else
                {
                    foreach (var time in times)
                    {
                        if (!existingAppointment.Times.Any(t => t.Time == time))
                        {
                            existingAppointment.Times.Add(new Times { Time = time });
                        }
                    }
                }
            }

            await _context.SaveChangesAsync();
            return true;
        }



        public async Task<bool> UpdateOnTime(DayOfWeek Day, string oldTime, string newTime,int DocId)
        {
            var appointment = await _context.Appointments
                .Include(a => a.Times)
                .FirstOrDefaultAsync(a => (a.DayOfWeek == Day)&&(a.DoctorId==DocId));

            if (appointment == null)
            {
                return false; // Appointment not found
            }

            var timeToUpdate = appointment.Times.FirstOrDefault(t => t.Time == oldTime);
            if (timeToUpdate == null)
            {
                return false; // Old time not found for this appointment
            }

            // Check if the new time is already associated with the appointment
            bool newTimeExists = appointment.Times.Any(t => t.Time == newTime);
            if (newTimeExists)
            {
                return false; // New time already booked for this appointment
            }

            // Update the existing time to the new time
            timeToUpdate.Time = newTime;
            await _context.SaveChangesAsync();
            return true; // Time updated successfully
        }

        public async Task<bool> DeleteTime(DayOfWeek Day, string Time, int DocId)
        {
            var appointment = await _context.Appointments
                .Include(a => a.Times)
                .FirstOrDefaultAsync(a => (a.DayOfWeek == Day) && (a.DoctorId == DocId));

            if (appointment == null)
            {
                return false; // Appointment not found
            }

            var timeToDelete = appointment.Times.FirstOrDefault(t => t.Time == Time);
            if (timeToDelete == null)
            {
                return false; // Time not found for this appointment
            }
            // Delete the existing time 
            _context.Times.Remove(timeToDelete);
            await _context.SaveChangesAsync();
            return true; // Time Deleted successfully
        }
    }
}

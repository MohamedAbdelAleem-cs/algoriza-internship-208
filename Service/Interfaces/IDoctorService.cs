using Core.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IDoctorService
    {
        public Task<bool> AddAppointments(AppointmentDataAdd appointmentData, string UserId);
        public Task<bool> UpdateTime(DayOfWeek Day, string oldTime, string newTime,string UserId);

        public Task<bool> DeleteTime(DayOfWeek Day, string Time, string UserId);
    }
}

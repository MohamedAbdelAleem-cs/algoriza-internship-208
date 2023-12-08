using Core.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IAppointmentRepository
    {
        public Task<bool> AddAppointments(AppointmentDataAdd appointmentData, int doctorId);

        public Task<bool> UpdateOnTime(DayOfWeek Day, string oldTime, string newTime,int DocId);

        public Task<bool> DeleteTime(DayOfWeek Day, string Time , int DocId);

    }
}

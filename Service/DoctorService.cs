using Core.DTOS;
using Core.Interfaces;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class DoctorService:IDoctorService
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IAppointmentRepository _appointmentRepository;

        public DoctorService(IDoctorRepository doctorRepository, IAppointmentRepository appointmentRepository)
        {
            _doctorRepository = doctorRepository;
            _appointmentRepository = appointmentRepository;
        }

        public async Task<bool> AddAppointments(AppointmentDataAdd appointmentData,string UserId)
        {
            int docId = await _doctorRepository.GetDoctorIdUsingUserId(UserId);
            var res=await  _appointmentRepository.AddAppointments(appointmentData, docId);
            return res;
        }



        public async Task<bool> UpdateTime(DayOfWeek Day, string oldTime, string newTime,string UserId)
        {
            int docId = await _doctorRepository.GetDoctorIdUsingUserId(UserId);
            var res = await _appointmentRepository.UpdateOnTime(Day, oldTime, newTime, docId);
            return res;

        }

        public async Task<bool> DeleteTime(DayOfWeek Day, string Time, string UserId)
        {
            int docId = await _doctorRepository.GetDoctorIdUsingUserId(UserId);
            var res = await _appointmentRepository.DeleteTime(Day, Time, docId);
            return res;
        }
    }
}

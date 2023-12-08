using Core.Const;
using Core.DTOS;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IBookingRepository
    {
        
        public Task<bool> BookDoctorAsync(BookingData bookingData, string PatientId);
        public Task<IEnumerable<Bookings>> GetAllBookingsAsync();
        public Task<IEnumerable<Bookings>> GetBookingsOfUserAsync(string UserId);
        public Task<IEnumerable<Bookings>> GetBookingsOfDoctorAsync(int DocId);

        public Task<bool> ChangeBookingStatus(int Id, int DocId, BookingStatus Status);

    }
}

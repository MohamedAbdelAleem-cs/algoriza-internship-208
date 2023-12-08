using Core.Const;
using Core.DTOS;
using Core.DTOS.BookingDTOS;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IBookingService
    {
        public Task<IEnumerable<Bookings>> GetAllBookings();
        public Task<bool> BookDoctorAsync(BookingData bookingData, string userId);
        public Task<IEnumerable<BookingDataDisplayUser>> GetBookingsOfUserAsync(string UserId);
        public Task<IEnumerable<BookingDataDisplayDoctor>> GetBookingsOfDoctorAsync(string  UserId);
        public Task<bool> ChangeBookingStatus(int Id,string UserId, BookingStatus Status);

        public Task<bool> CancelBookingUser(int Id,string UserId);
        public Task<NumberOfBookings> GetNumberOfBookings();

    }
}

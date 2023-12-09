using Core.Const;
using Core.DTOS;
using Core.Interfaces;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class BookingRepository : IBookingRepository
    {

        private readonly IDoctorRepository _doctorRepository;
        private readonly IDiscountRepository _discountRepository;
        protected AppDbContext _Context;

        public BookingRepository(IDoctorRepository doctorRepository, AppDbContext context, IDiscountRepository discountRepository)
        {
            _doctorRepository = doctorRepository;
            _Context = context;
            _discountRepository = discountRepository;
        }

        public async Task<bool> BookDoctorAsync(BookingData bookingData, string PatientId)
        {
            var doctor=await _doctorRepository.GetByIdAsync(bookingData.DoctorId);

            if (doctor != null) {
                var numOfBookingsOfUser = await _Context.Bookings.Where(D => D.patientId == PatientId && (D.Status == BookingStatus.completed)).CountAsync();
                var Discounts = await _discountRepository.GetAllDiscount();
                var AvailableDiscount = Discounts.FirstOrDefault(D => D.CompletedRequests == numOfBookingsOfUser);
                decimal FinalPrice=doctor.Price;


                if (AvailableDiscount != null) {
                    var DiscountAlreadyUsed = _Context.Bookings.Any(B => B.patientId == PatientId
                    && (B.Status == BookingStatus.pending || B.Status == BookingStatus.Confirmed) 
                    && B.DiscountCode == AvailableDiscount.DiscountCode);

                    if (DiscountAlreadyUsed)
                    {
                        AvailableDiscount = null;
                    }
                    else if (AvailableDiscount.DiscountType == DiscountType.Value)
                    {
                        FinalPrice = doctor.Price - AvailableDiscount.Value;
                        if(FinalPrice < 0)
                        {
                            FinalPrice = 0;
                        }
                    }
                    else if (AvailableDiscount.DiscountType == DiscountType.Percentage)
                    {
                        FinalPrice = doctor.Price - doctor.Price * (AvailableDiscount.Value / 100);
                    } 
                }


                Bookings Booking = new Bookings()
                {
                    DoctorId = bookingData.DoctorId,
                    patientId = PatientId,
                    Status = BookingStatus.pending,
                    Day = bookingData.DayOfWeek,
                    Time = bookingData.Time,
                    Price = doctor.Price,
                    DiscountCode = AvailableDiscount?.DiscountCode ?? null,
                    FinalPrice = FinalPrice
                };

                var doctorAppointments = await _doctorRepository.GetDoctorAppointments(doctor.Id);
                var hasAppointmentOnDayAndTime = doctorAppointments
                     .SelectMany(a => a.Times) // Flatten the Times collections across all appointments
                     .Any(t => t.Time == Booking.Time);
                if (hasAppointmentOnDayAndTime)
                {
                    var allBookings = await GetAllBookingsAsync();

                    var isAlreadyBooked = await _Context.Bookings.AnyAsync(B => B.DoctorId == Booking.DoctorId
                    && (B.Status == BookingStatus.pending || B.Status == BookingStatus.Confirmed)
                    && B.Time==Booking.Time);
                    if(!isAlreadyBooked)
                    {
                        var res = await _Context.Bookings.AddAsync(Booking);
                        var affected = await _Context.SaveChangesAsync();
                        return affected > 0;
                    }
                }
                return false;
            }

            return false;
        }


        public async Task<IEnumerable<Bookings>> GetAllBookingsAsync()
        {

            return await _Context.Bookings.Include(B=>B.doctor).ThenInclude(D=>D.Specialization).ToListAsync();

        }

        public async Task<IEnumerable<Bookings>> GetBookingsOfUserAsync(string UserId)
        {
            return await _Context.Bookings.Where(B=>B.patientId==UserId).ToListAsync();
        }

        public async Task<bool> ChangeBookingStatus(int Id, int DocId, BookingStatus Status)
        {
            var BookingToChange= await _Context.Bookings.FirstOrDefaultAsync(B => (B.Id == Id) && (B.DoctorId == DocId));
            if (BookingToChange == null)
            {
                return false;
            }
            BookingToChange.Status= Status;
           var affected= await _Context.SaveChangesAsync();
            return affected>0;

        }

        public async Task<IEnumerable<Bookings>> GetBookingsOfDoctorAsync(int DocId)
        {
            return await _Context.Bookings.Where(B=>B.DoctorId==DocId).ToListAsync();
        }

        public async Task<bool> CancelBooking(int Id, string UserId)
        {
            var BookingToChange = await _Context.Bookings.FirstOrDefaultAsync(B => (B.Id == Id) && (B.patientId == UserId));
            if (BookingToChange == null)
            {
                return false;
            }
            if(BookingToChange.Status==BookingStatus.Confirmed||BookingToChange.Status==BookingStatus.completed)
            {
                return false;
            }
            BookingToChange.Status = BookingStatus.cancelled;
            var affected = await _Context.SaveChangesAsync();
            return affected > 0;
        }
    }
}

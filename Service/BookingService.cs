using Core.Const;
using Core.DTOS;
using Core.DTOS.BookingDTOS;
using Core.Helper_Functions;
using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Identity;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        public BookingService(IBookingRepository bookingRepository,IDoctorRepository doctorRepository ,UserManager<ApplicationUser> userManager)
        {
            _bookingRepository = bookingRepository;
            _userManager = userManager;
            _doctorRepository = doctorRepository;
        }


        public async Task<IEnumerable<Bookings>> GetAllBookings()
        {
            return await _bookingRepository.GetAllBookingsAsync();
        } 

        public async Task<bool> BookDoctorAsync(BookingData bookingData, string userId)
        {
            var res=await _bookingRepository.BookDoctorAsync(bookingData, userId);
            return res;
        }

        public async Task<IEnumerable<BookingDataDisplayUser>> GetBookingsOfUserAsync(string UserId)
        {
            var res = await _bookingRepository.GetBookingsOfUserAsync(UserId);
            List<BookingDataDisplayUser> result = new List<BookingDataDisplayUser>(); 
            
            foreach(var booking in res)
            {
                var doctor = await _doctorRepository.GetByIdAsync(booking.DoctorId);
                var doctorUserDetails = await _userManager.FindByIdAsync(doctor.UserId);
                result.Add(new BookingDataDisplayUser
                {
                    Id=booking.Id,
                    DoctorName=$"{doctorUserDetails.FirstName} {doctorUserDetails.LastName}",
                    Image=doctorUserDetails.Image,
                    DayOfWeek= booking.Day,
                    Specialization=doctor.Specialization.SpecializationNameEn,
                    Status=booking.Status,
                    Time=booking.Time,
                    Price=booking.Price,
                    Discount=booking.DiscountCode,
                    FinalPrice=booking.FinalPrice,
                });
            }
            return result;
        }

        public async Task<bool> ChangeBookingStatus(int Id, string UserId, BookingStatus Status)
        {
            int docId = await _doctorRepository.GetDoctorIdUsingUserId(UserId);
            var res = await _bookingRepository.ChangeBookingStatus(Id, docId, Status);
            return res;
            
        }

        public async Task<IEnumerable<BookingDataDisplayDoctor>> GetBookingsOfDoctorAsync(string UserId)
        {
            int docId = await _doctorRepository.GetDoctorIdUsingUserId(UserId);
            var res = await _bookingRepository.GetBookingsOfDoctorAsync(docId);
            List<BookingDataDisplayDoctor> result = new List<BookingDataDisplayDoctor>();
            foreach (var booking in res)
            {
               var Patient = await _userManager.FindByIdAsync(booking.patientId);
                result.Add( new BookingDataDisplayDoctor
                {
                    Id=booking.Id,
                    Email = Patient.Email,
                    Gender = Patient.Gender,
                    Phone = Patient.PhoneNumber,
                    Image = Patient.Image,
                    PatientName = $"{Patient.FirstName} {Patient.LastName}",
                    age = HelperFunctions.CalculateAge(Patient.DateOfBirth),
                    DayOfWeek=booking.Day,
                    Time=booking.Time,
                });
            }

            return result;
        }

        public async Task<NumberOfBookings> GetNumberOfBookings()
        {
            var bookings= await _bookingRepository.GetAllBookingsAsync();
            var res = new NumberOfBookings()
            {
                Total = bookings.Count(),
                Pending = bookings.Count(B => B.Status == BookingStatus.pending),
                Confirmed = bookings.Count(B => B.Status == BookingStatus.Confirmed),
                Completed= bookings.Count(B=>B.Status==BookingStatus.completed),
                Cancelled=bookings.Count(B=> B.Status==BookingStatus.cancelled)
            };
            return res;
        }

        public async Task<bool> CancelBookingUser(int Id, string UserId)
        {
            return await _bookingRepository.CancelBooking(Id, UserId);
        }
    }
}

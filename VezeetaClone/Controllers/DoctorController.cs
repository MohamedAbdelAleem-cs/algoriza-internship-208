using Core.Const;
using Core.DTOS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Service.Interfaces;

namespace VezeetaCloneWeb.Controllers
{
    [Authorize(Policy = "DoctorAccountPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {

        private readonly IBookingService _bookingService;
        private readonly IUserService _userService;
        private readonly IDoctorService _doctorService;

        public DoctorController(IBookingService bookingService, IUserService userService, IDoctorService doctorService)
        {
            _bookingService = bookingService;
            _userService = userService;
            _doctorService = doctorService;
        }

        [HttpGet("GetBookings")]
        public async Task<IActionResult> GetDoctorBookings()
        {
            var CurrentUser = await _userService.GetCurrentUserAsync(User);
            var res = await _bookingService.GetBookingsOfDoctorAsync(CurrentUser.Id);
            return Ok(res);
        }

        [HttpPatch("ConfirmBooking/{id:int}")]
        public async Task<IActionResult> ConfirmBooking([FromRoute]int id)
        {
            var CurrentUser = await _userService.GetCurrentUserAsync(User);
            var res = await _bookingService.ChangeBookingStatus(id, CurrentUser.Id, BookingStatus.Confirmed);
            return Ok(res);
        }

        [HttpPatch("CompleteBooking/{id:int}")]
        public async Task<IActionResult> CompleteBooking([FromRoute]int id)
        {
            var CurrentUser = await _userService.GetCurrentUserAsync(User);
            var res = await _bookingService.ChangeBookingStatus(id, CurrentUser.Id, BookingStatus.completed);
            return Ok(res);
        }

        [HttpPost("AddAppointments")]
        public async Task<IActionResult> AddAppointmentsAsync(AppointmentDataAdd appointmentData)
        {
            var CurrentUser = await _userService.GetCurrentUserAsync(User);
            if(appointmentData.DayOfWeeks.Count!=appointmentData.TimesList.Count)
            {
                return Ok(false);
            }
            var res = await _doctorService.AddAppointments(appointmentData, CurrentUser.Id);
            return Ok(res);
        }

        [HttpPut("UpdateTime")]
        public async Task<IActionResult> UpdateTimeAsync(AppointmentTimeUpdate update)
        {
            var CurrentUser = await _userService.GetCurrentUserAsync(User);

            var res=await _doctorService.UpdateTime(update.DayOfWeek,update.oldTime,update.newTime,CurrentUser.Id);
            return Ok(res);
        }

        [HttpDelete("DeleteTime")]
        public async Task<IActionResult> DeleteTimeAsync(AppointmentTimeDelete delete)
        {
            var CurrentUser = await _userService.GetCurrentUserAsync(User);
            var res = await _doctorService.DeleteTime(delete.Day, delete.Time, CurrentUser.Id);
            return Ok(res);
        }

    }
}

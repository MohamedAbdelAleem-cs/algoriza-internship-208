using Core.DTOS;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace VezeetaCloneWeb.Controllers
{

    [Authorize(Policy = "PatientAccountPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly IUserService _userService;
        private readonly IPatientService _patientService;

        public PatientController(IBookingService bookingService, IUserService userService, IPatientService patientService)
        {
            _bookingService = bookingService;
            _userService = userService;
            _patientService = patientService;
        }

        [HttpPost("BookDoctor")]
        public async Task<IActionResult> BookDoctorAsync(BookingData bookingData)
        {           
           var currentUser= await _userService.GetCurrentUserAsync(User);
           var res =  await _bookingService.BookDoctorAsync(bookingData, currentUser.Id);
           return Ok(res) ;
        }

        [HttpGet("GetDoctors")]
        public async Task<IActionResult> GetAllDoctorsAsync()
        {
            var res=await _patientService.GetAllDoctorsAsync();
            return Ok(res);
        }

        [HttpGet("GetBookings")]
        public async Task<IActionResult> GetUserBookingsAsync()
        {
            var CurrentUser=await _userService.GetCurrentUserAsync(User);
            var res = await _bookingService.GetBookingsOfUserAsync(CurrentUser.Id);
            return Ok(res);
        }

        [HttpPatch("CancelBooking/{Id:int}")]
        public async Task<IActionResult> CancelBookingAync(int Id)
        {
            var CurrentUser = await _userService.GetCurrentUserAsync(User);
            var res = await _bookingService.CancelBookingUser(Id, CurrentUser.Id);
            return Ok(res);
        }
    }
}

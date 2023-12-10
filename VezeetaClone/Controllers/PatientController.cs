using Core.DTOS;
using Core.DTOS.BookingDTOS;
using Core.DTOS.DoctorDTO;
using Core.DTOS.DoctorDTOS;
using Core.Helper_Functions;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
        public async Task<IActionResult> GetAllDoctorsAsync(int page,int pageSize)
        {
            var res=await _patientService.GetAllDoctorsAsync();
            string projectRoot = Directory.GetCurrentDirectory();
            IEnumerable<DoctorDisplay> modifiedRes = res.Select(D =>
            {
                D.Image = HelperFunctions.AddFullImagePath(projectRoot, D.Image);
                return D;
            });

            if (page > 0 && pageSize > 0)
            {
                modifiedRes = HelperFunctions.PaginateList<DoctorDisplay>(modifiedRes.ToList(), page, pageSize);
            }
            return Ok(modifiedRes);
        }

        [HttpGet("GetBookings")]
        public async Task<IActionResult> GetUserBookingsAsync()
        {
            var CurrentUser=await _userService.GetCurrentUserAsync(User);
            var res = await _bookingService.GetBookingsOfUserAsync(CurrentUser.Id);
            string projectRoot = Directory.GetCurrentDirectory();
            IEnumerable<BookingDataDisplayUser> modifiedRes = res.Select(B =>
            {
                B.Image = HelperFunctions.AddFullImagePath(projectRoot, B.Image);
                return B;
            });
            return Ok(modifiedRes);
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

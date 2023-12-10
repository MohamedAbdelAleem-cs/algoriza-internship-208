using Core.Const;
using Core.DTOS.AdminDTOS;
using Core.DTOS.DoctorDTO;
using Core.DTOS.DoctorDTOS;
using Core.DTOS.PatientDTOS;
using Core.Helper_Functions;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service;
using Service.Interfaces;
using System.Runtime.CompilerServices;

namespace VezeetaCloneWeb.Controllers
{
    [Authorize(Policy = "AdminAccountPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _AdminService;
        private readonly IBookingService _BookingService;

        public AdminController(IAdminService adminService, IBookingService bookingService)
        {
            _AdminService = adminService;
            _BookingService = bookingService;

        }

        #region Dashboard
        [HttpGet("NumOfDoctors")]
        public async Task<IActionResult> GetNumOfDoctors()
        {
            var res = (await _AdminService.GetAllDoctorsAsync()).Count();
            return Ok(res);
        }

        [HttpGet("NumOfPatients")]
        public async Task<IActionResult> GetNumberOfPatients()
        {
            var res = (await _AdminService.GetAllUsersAsync()).Count();
            return Ok(res);
        }
        [HttpGet("NumberOfRequests")]
        public async Task<IActionResult> GetNumberOfRequests()
        {
            var res = await _BookingService.GetNumberOfBookings();
            return Ok(res);
        }

        [HttpGet("TopSpecialization")]
        public async Task<IActionResult> GetTopSpecializations()
        {
            var Bookings = (await _BookingService.GetAllBookings()).ToList();
            var TopSpecializations = await _AdminService.GetTopSpecializationsAsync(Bookings);
            return Ok(TopSpecializations);
        }
        [HttpGet("TopDoctors")]
        public async Task<IActionResult> GetTopDoctors()
        {
            var Bookings = (await _BookingService.GetAllBookings()).ToList();
            var TopDoctors = await _AdminService.GetTopDoctorsAsync(Bookings);
            return Ok(TopDoctors);
        } 
        #endregion

        #region Doctor Related Endpoints
        [HttpGet("GetAllDoctors")]
        public async Task<IActionResult> GetAllAsync(int page,int pageSize)
        {
            var res = await _AdminService.GetAllDoctorsAsync();
            string projectRoot = Directory.GetCurrentDirectory();
            IEnumerable <DoctorDisplayAdmin> modifiedRes = res.Select(D =>
            {
                D.Image = HelperFunctions.AddFullImagePath(projectRoot,D.Image);
                return D;
            });
            if (page > 0 && pageSize > 0)
            {
                modifiedRes = HelperFunctions.PaginateList<DoctorDisplayAdmin>(modifiedRes.ToList(), page, pageSize);
            }
            return Ok(modifiedRes);
        }

        [HttpGet("Doctor/{id:int}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var res = await _AdminService.GetDoctorByIdAsync(id);
            string projectRoot = Directory.GetCurrentDirectory();
            if (res == null)
            {
                return NotFound();
            }
            res.Image = HelperFunctions.AddFullImagePath(projectRoot, res.Image);
            return Ok(res);
        }

        [HttpPost("Doctor")]
        public async Task<IActionResult> AddDoctorAsync([FromBody] DoctorDetailsCreate doctor)
        {


            string ImgPath = HelperFunctions.ProcessUploadedFile(doctor.Image);
            doctor.Image = ImgPath;
            if(ImgPath == null)
            {
                return Ok(false);
            }
            var res = await _AdminService.AddDoctorAsync(doctor);
            if (!res)
            {
                HelperFunctions.UndoUploadedFile(ImgPath);
            }
            return Ok(res);
        }

        [HttpDelete("Doctor/{id:int}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var res = await _AdminService.DeleteDoctorAsync(id);
            return Ok(res);

        }

        [HttpPut("Doctor/{doctorId:int}")]
        public async Task<IActionResult> UpdateAsync(DoctorDetailsEdit doctor, int doctorId)
        {
            var res = await _AdminService.UpdateDoctorAsync(doctor, doctorId);
            return Ok(res);
        }
        #endregion

        #region Patient Related Endpoints
        [HttpGet("GetAllPatients")]
        public async Task<IActionResult> GetAllPatientsAsync(int page,int pageSize)
        {
            var res = await _AdminService.GetAllUsersAsync();
            string projectRoot = Directory.GetCurrentDirectory();
            IEnumerable<DisplayPatientAdmin> modifiedRes = res.Select(patient =>
            {
                patient.Image = HelperFunctions.AddFullImagePath(projectRoot, patient.Image);

                patient.BookingData = patient.BookingData.Select(bookingData =>
                {
                    bookingData.Image = HelperFunctions.AddFullImagePath(projectRoot, bookingData.Image);
                    return bookingData;
                }).ToList();
                return patient;
            });

            if (page > 0 && pageSize > 0)
            {
                modifiedRes = HelperFunctions.PaginateList<DisplayPatientAdmin>(modifiedRes.ToList(), page, pageSize);
            }
            return Ok(modifiedRes);
        }

        [HttpGet("Patient/{id}")]
        public async Task<IActionResult> GetPatientByIdAsync(string id)
        {
            var res = await _AdminService.GetUserByIdAsync(id);
            string projectRoot = Directory.GetCurrentDirectory();
            res.Image=HelperFunctions.AddFullImagePath(projectRoot, res.Image);
            if (res == null)
            {
                return NotFound();
            }
            return Ok(res);
        }
        #endregion

        #region Discount
        [HttpPost("Discounts")]
        public async Task<IActionResult> AddDiscount(AddDiscountData discountData)
        {
            var res = await _AdminService.AddDiscountAsync(discountData);
            return Ok(res);
        }
        [HttpPut("Discounts/{Id:int}")]
        public async Task<IActionResult> UpdateDiscount([FromRoute] int Id, AddDiscountData discountData)
        {
            var res = await _AdminService.UpdateDiscountAsync(Id, discountData);
            return Ok(res);
        }
        [HttpDelete("Discounts/{Id:int}")]
        public async Task<IActionResult> DeleteDiscount([FromRoute] int Id)
        {
            var res = await _AdminService.DeleteDiscountAsync(Id);
            return Ok(res);
        }
        [HttpPatch("Discounts/Deactivate/{Id:int}")]
        public async Task<IActionResult> DeactivateDiscount([FromRoute] int Id)
        {
            var res = await _AdminService.DeactivateDiscountAsync(Id);
            return Ok(res);
        } 
        #endregion



        
    }
}

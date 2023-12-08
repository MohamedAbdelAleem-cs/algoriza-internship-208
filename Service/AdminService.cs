using Core.Const;
using Core.DTOS.AdminDTOS;
using Core.DTOS.DoctorDTO;
using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Identity;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class AdminService : IAdminService
    {
        private readonly IDoctorRepository _DoctorRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDiscountRepository _discountRepository;

        public AdminService(IDoctorRepository doctorRepository, UserManager<ApplicationUser> userManager, IDiscountRepository discountRepository)
        {
            _DoctorRepository = doctorRepository;
            _userManager = userManager;
            _discountRepository = discountRepository;
        }

        #region Dashboard
        public async Task<List<TopSpecialization>> GetTopSpecializationsAsync(List<Bookings> bookings)
        {
            var topSpecializations = bookings
                .GroupBy(b => b.doctor.Specialization) // Group by specialization
                .OrderByDescending(g => g.Count()) // Order by count of bookings per specialization
                .Take(5) // Take the top 5 specializations
                .ToList();

            List<TopSpecialization> result = topSpecializations.Select(r =>
            {
                var specialization = r.Key; // Get the specialization from the grouping
                return new TopSpecialization
                {
                    NumOfRequests = r.Count(), // Count of bookings for this specialization
                    SpecializationNameAr = specialization.SpecializationNameAr,
                    SpecializationNameEn = specialization.SpecializationNameEn
                };
            }).ToList(); // Convert the selected items to a List<TopSpecialization>

            return result;
        }

        public async Task<List<TopDoctors>> GetTopDoctorsAsync(List<Bookings> bookings)
        {
            var topDoctorsBookings = bookings
                .GroupBy(b => b.doctor.UserId) // Group bookings by DoctorId
                .OrderByDescending(g => g.Count()) // Order by count of bookings per doctor
                .Take(10)// Take the top 10 doctors
                .ToList();

            List<TopDoctors> result = new List<TopDoctors>();

            var doctorIds = topDoctorsBookings.SelectMany(d => d.Select(B => B.DoctorId)).Distinct().ToList();
            int count = 0;
            foreach (var doctorBooking in topDoctorsBookings)
            {
                var DocUser = await _userManager.FindByIdAsync(doctorBooking.Key);
                var doc = await _DoctorRepository.GetByIdAsync(doctorIds[count++]);

                result.Add(new TopDoctors
                {
                    Image = DocUser.Image,
                    FullName = $"{DocUser.FirstName} {DocUser.LastName}",
                    SpecializationEn = doc.Specialization.SpecializationNameEn,
                    SpecializationAr = doc.Specialization.SpecializationNameAr,
                    NumOfRequests = doctorBooking.Count() // Number of requests for this doctor
                });
            }
            return result;
        } 
        #endregion

        #region Admin-Doctor Services

        public async Task<IEnumerable<Doctor>> GetAllDoctorsAsync()
        {
            var res = await _DoctorRepository.GetAllAsync();
            return res;
        }

        public async Task<Doctor> GetDoctorByIdAsync(int doctorId)
        {
            var res = await _DoctorRepository.GetByIdAsync(doctorId);
            return res;
        }
        public async Task<bool> AddDoctorAsync(DoctorDetailsCreate Doctor)
        {
            var result = await _userManager.FindByEmailAsync(Doctor.Email);
            if (result == null)
            {
                ApplicationUser newUser = new ApplicationUser()
                {
                    Email = Doctor.Email,
                    UserName = Doctor.Email,
                    FirstName = Doctor.FirstName,
                    LastName = Doctor.LastName,
                    DateOfBirth = Doctor.DateOfBirth,
                    Gender = Doctor.Gender,
                    PhoneNumber = Doctor.PhoneNumber,
                    Image = Doctor.Image,
                    AccountType = AccountType.Doctor
                };

                var createResult = await _userManager.CreateAsync(newUser, Doctor.Password);
                var claim = new Claim("AccountType", AccountType.Doctor.ToString());
                await _userManager.AddClaimAsync(newUser, claim);
                if (createResult.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(newUser.Email);

                    var doctor = new Doctor
                    {
                        UserId = user.Id,
                        Price = Doctor.Price,
                        SpecializationID = Doctor.specializationId
                    };

                    var res = await _DoctorRepository.AddDoctorsAsync(doctor);
                    return res;
                }
                else
                {
                    // Handle the case when user creation fails
                    return false;
                }
            }
            else
            {
                // Handle the case when the user already exists
                return false;
            }
        }

        public async Task<bool> DeleteDoctorAsync(int doctorId)
        {
            var doctor = await _DoctorRepository.GetByIdAsync(doctorId);

            if (doctor != null)
            {
                var result2 = await _DoctorRepository.DeleteDoctorAsync(doctorId);
                if(result2.Equals(false)) { return false; }
                var user = await _userManager.FindByIdAsync(doctor.UserId);
                if (user != null)
                {
                    var result = await _userManager.DeleteAsync(user);
                    if (result.Succeeded)
                    {
                        return result2;
                    }
                    else
                    {
                        return false;
                    }
                }

            }
            return false;
        }
        public async Task<bool> UpdateDoctorAsync(DoctorDetailsEdit doctorUpdate,int doctorId)
        {
            var doctor = await _DoctorRepository.GetByIdAsync(doctorId);

            if (doctor != null)
            {
                var user = await _userManager.FindByIdAsync(doctor.UserId);
                if (user != null)
                {
                    doctor.SpecializationID = doctorUpdate.specializationId;
                    await _DoctorRepository.UpdateDoctorAsync(doctor);

                    user.FirstName = doctorUpdate.FirstName;
                    user.LastName = doctorUpdate.LastName;
                    user.UserName = doctorUpdate.Email;
                    user.Email = doctorUpdate.Email;
                    user.Image = doctorUpdate.Image;
                    user.PhoneNumber = doctorUpdate.PhoneNumber;
                    user.Gender= doctorUpdate.Gender;
                    user.DateOfBirth = doctorUpdate.DateOfBirth;
                    await _userManager.UpdateAsync(user);
                    return true;
                }

            }
            return false;
        }
        #endregion

        #region Admin-Patient Services
        public async Task<IEnumerable<ApplicationUser>> GetAllUsersAsync()
        {
           return await _userManager.GetUsersForClaimAsync(new Claim("AccountType", AccountType.Patient.ToString()));
        }

        public async Task<ApplicationUser> GetUserByIdAsync(string userId)
        {

            return await _userManager.FindByIdAsync(userId);
        }


        #endregion


        public async Task<bool> AddDiscountAsync(AddDiscountData discountData)
        {
            if (discountData.DiscountType == DiscountType.Percentage && discountData.value > 100)
            {
                return false;
            }
            Discount discount = new Discount {
                CompletedRequests = discountData.CompletedRequests,
                DiscountCode = discountData.DiscountCode,
                DiscountType = discountData.DiscountType,
                Value = discountData.value,
                isActivated = true
            };
            var res = await _discountRepository.AddDiscount(discount);
            return res;
        }

        public async Task<bool> UpdateDiscountAsync(int Id, AddDiscountData discountData)
        {
            if (discountData.DiscountType == DiscountType.Percentage && discountData.value > 100)
            {
                return false;
            }
            Discount discount = new Discount
            {
                Id = Id,  
                CompletedRequests = discountData.CompletedRequests,
                DiscountCode = discountData.DiscountCode,
                DiscountType = discountData.DiscountType,
                Value = discountData.value,
                isActivated = true
            };

            var res = await _discountRepository.UpdateDiscount(discount);
            return res;
        }

        public async Task<bool> DeleteDiscountAsync(int Id)
        {
            var res = await _discountRepository.DeleteDiscount(Id);
            return res;
        }

        public async Task<bool> DeactivateDiscountAsync(int Id)
        {
            var res = await _discountRepository.DeactivateDiscount(Id);
            return res;
        }
    }
}

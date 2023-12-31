﻿using Core.DTOS.AdminDTOS;
using Core.DTOS.DoctorDTO;
using Core.DTOS.DoctorDTOS;
using Core.DTOS.PatientDTOS;
using Core.Interfaces;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IAdminService
    {

        
        Task<IEnumerable<DisplayPatientAdmin>> GetAllUsersAsync();
        Task<DisplayPatientAdmin> GetUserByIdAsync(string userId);
        Task<IEnumerable<DoctorDisplayAdmin>> GetAllDoctorsAsync();
        Task<DoctorDisplayAdmin> GetDoctorByIdAsync(int doctorId);

        Task<bool> AddDoctorAsync(DoctorDetailsCreate Doctor);
        Task<bool> UpdateDoctorAsync(DoctorDetailsEdit doctor,int doctorId);
        Task<bool> DeleteDoctorAsync(int doctorId);

        Task<List<TopSpecialization>> GetTopSpecializationsAsync(List<Bookings> bookings);
        Task<List<TopDoctors>> GetTopDoctorsAsync(List<Bookings> bookings);


        Task<bool> AddDiscountAsync(AddDiscountData discountData);
        Task<bool> UpdateDiscountAsync(int Id, AddDiscountData discountData);
        Task<bool> DeleteDiscountAsync(int Id);
        Task<bool> DeactivateDiscountAsync(int Id);
    }
}

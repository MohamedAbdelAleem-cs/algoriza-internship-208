using Core.DTOS.DoctorDTO;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IPatientService
    {
        Task<IEnumerable<DoctorDisplay>> GetAllDoctorsAsync();
    }
}

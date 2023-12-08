using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTOS.AccountDTOS;

namespace Core.DTOS.DoctorDTO
{
    public class DoctorDetailsCreate : UserDetailsCreate
    {
        public decimal Price { get; set; }
        public int specializationId { get; set; }

        [Required(ErrorMessage = "Image for doctor is required")]
        public override string? Image { get; set; }
    }
}

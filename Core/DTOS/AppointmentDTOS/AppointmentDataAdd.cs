using Core.Custom_Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOS
{
    public class AppointmentDataAdd
    {
        [Required]
        public List<DayOfWeek> DayOfWeeks { get; set; }


        [Required]
        [ListOfTimesValidation(ErrorMessage = "Invalid time format. Please use HH:MM.")]
        public List<List<string>> TimesList { get; set; }
    }
}

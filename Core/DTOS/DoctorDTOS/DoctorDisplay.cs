using Core.Const;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core.DTOS.DoctorDTO
{
    public class DoctorDisplay
    {
        public int ID { get; set; } 
        public string Image { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public Specialization Specialization { get; set; }
        public decimal Price { get; set; }

        public Gender Gender { get; set; }
        public ICollection<AppointmentDisplay> Appointments { get; set; }

    }

    public class AppointmentDisplay
    {
        public DayOfWeek Day { get; set; }
        public ICollection<string> Time { get; set; }
    }
}

using Core.Const;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Doctor
    {
        public int Id { get; set; }
        public string UserId { get; set; } // Foreign key for the ApplicationUser
        public ApplicationUser User { get; set; }

        public decimal Price { get; set; }
        public int SpecializationID { get; set; }
        public Specialization Specialization { get; set; }
        public ICollection<Bookings> Bookings { get; set; }

        public ICollection<Appointment> Appointments { get; set; }

    }
}

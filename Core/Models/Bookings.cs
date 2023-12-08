using Core.Const;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Bookings
    {

        public int Id {  get; set; }
        public int DoctorId {  get; set; }
        public Doctor doctor { get; set; }

        public string patientId { get; set; }

        public ApplicationUser Patient { get; set; }
        public DayOfWeek Day {  get; set; }

        public BookingStatus Status { get; set; }

        public string Time {  get; set; }

        public decimal Price {  get; set; }
        public string? DiscountCode {  get; set; }
        public decimal FinalPrice { get; set; }

    }
}

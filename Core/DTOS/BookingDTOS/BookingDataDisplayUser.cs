using Core.Const;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOS
{
    public class BookingDataDisplayUser
    {
        public int Id { get; set; }
        public string Image {  get; set; }
        public string DoctorName { get; set; }

        public string Specialization { get; set; }

        public DayOfWeek DayOfWeek { get; set; }
        public string Time {  get; set; }
        public BookingStatus Status { get; set; }

        public decimal Price { get; set; }
        public string Discount { get; set; }
        public decimal FinalPrice {  get; set; }
    }
}

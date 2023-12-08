using Core.Const;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOS.BookingDTOS
{
    public class BookingDataDisplayDoctor
    {
        public string PatientName { get; set; }
        public string Image {  get; set; }
        public int age { get; set; }
        public Gender Gender { get; set; }
        public string Phone {  get; set; }
        public string Email { get; set; }
    }
}

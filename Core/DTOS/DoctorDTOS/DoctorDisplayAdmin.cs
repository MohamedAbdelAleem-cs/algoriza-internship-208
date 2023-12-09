using Core.Const;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOS.DoctorDTOS
{
    public class DoctorDisplayAdmin
    {
        public int Id { get; set; }
        public string Image {  get; set; }
        public string FullName {  get; set; }
        public string Email {  get; set; }
        public string Phone { get; set; }
        public Specialization Specialization { get; set; }
        public Gender Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}

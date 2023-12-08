using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOS.AdminDTOS
{
    public class TopDoctors
    {
        public string Image {  get; set; }
        public string FullName { get; set; }
        public string SpecializationEn {  get; set; }
        public string SpecializationAr { get; set; }
        public int NumOfRequests {  get; set; }
    }
}

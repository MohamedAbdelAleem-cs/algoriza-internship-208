using Core.Const;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOS.AdminDTOS
{
    public class AddDiscountData
    {
        [Required]
        public string DiscountCode { get; set; }

        [Required]
        public DiscountType DiscountType { get; set; }

        [Required]
        public int CompletedRequests { get; set; }

        [Required]
        [Range(1, 20000)]
        public int value { get; set; }
    }
}

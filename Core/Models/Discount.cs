using Core.Const;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Discount
    {
        public int Id { get; set; }
        public string DiscountCode {  get; set; }
        public int CompletedRequests { get; set; }
        public DiscountType DiscountType { get; set; }
        public decimal Value { get; set; }
        public bool isActivated { get; set; }
    }
}

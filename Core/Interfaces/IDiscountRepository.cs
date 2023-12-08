using Core.DTOS.AdminDTOS;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IDiscountRepository
    {
        Task<bool> AddDiscount(Discount discount);
        Task<bool> UpdateDiscount(Discount discount);
        Task<bool> DeleteDiscount(int Id);
        Task<bool> DeactivateDiscount(int Id);
        Task<IEnumerable<Discount>> GetAllDiscount();
    }
}

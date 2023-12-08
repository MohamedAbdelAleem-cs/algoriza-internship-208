using Core.DTOS.AdminDTOS;
using Core.Interfaces;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly AppDbContext _Context;

        public DiscountRepository(AppDbContext context)
        {
            _Context = context;
        }

        public async Task<bool> AddDiscount(Discount discount)
        {
            await _Context.Discounts.AddAsync(discount);
            var affected=await _Context.SaveChangesAsync();
            return affected > 0;
        }

        public async Task<bool> DeactivateDiscount(int Id)
        {
            var Discount = await _Context.Discounts.FirstOrDefaultAsync(D => D.Id == Id);
            Discount.isActivated = false;
            var affected = await _Context.SaveChangesAsync();
            return affected > 0;
        }

        public async Task<bool> DeleteDiscount(int Id)
        {
            var Discount = await _Context.Discounts.FirstOrDefaultAsync(D => D.Id == Id);
            _Context.Discounts.Remove(Discount);
            var affected = await _Context.SaveChangesAsync();
            return affected > 0;
        }


        public async Task<bool> UpdateDiscount(Discount discount)
        {
            _Context.Discounts.Update(discount);
            var affected = await _Context.SaveChangesAsync();
            return affected > 0;
        }

        public async Task<IEnumerable<Discount>> GetAllDiscount()
        {
            return _Context.Discounts.AsEnumerable();
        }

    }
}

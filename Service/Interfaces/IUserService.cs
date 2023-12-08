using Core.Const;
using Core.DTOS.AccountDTOS;
using Core.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IUserService
    {
        Task<IdentityResult> RegisterAsync(UserDetailsCreate userDetails);        
        //Change,reset passwords
        Task<AccountType> GetAccountTypeAsync(string Email);

        Task<ApplicationUser> GetCurrentUserAsync(ClaimsPrincipal User);

    }
}

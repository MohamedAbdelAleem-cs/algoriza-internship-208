using Core.Const;
using Core.DTOS.AccountDTOS;
using Core.Models;
using Microsoft.AspNetCore.Identity;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Service
{

    public class UserService:IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(
            UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }



        public async Task<IdentityResult> RegisterAsync(UserDetailsCreate userDetails)
        {
            var validationResult = ApplicationUserValidator.ValidateEmail(userDetails.Email);
            if (validationResult != ValidationResult.Success)
            {
                return IdentityResult.Failed(new IdentityError { Description = validationResult.ErrorMessage });
            }

            var user = new ApplicationUser()
            {
                UserName = userDetails.Email,
                Email = userDetails.Email,
                FirstName = userDetails.FirstName,
                LastName = userDetails.LastName,
                Gender = userDetails.Gender,
                PhoneNumber = userDetails.PhoneNumber,
                DateOfBirth = userDetails.DateOfBirth,
                AccountType = AccountType.Patient,
                Image = userDetails.Image,
            };
            var result =await _userManager.CreateAsync(user,userDetails.Password);
            var claim = new Claim("AccountType", AccountType.Patient.ToString());
            await _userManager.AddClaimAsync(user, claim);
            return result;
        }

        public async Task<AccountType> GetAccountTypeAsync(string Email)
        {
            var user=await _userManager.FindByNameAsync(Email);
            return user.AccountType;
        }

        public async Task<ApplicationUser> GetCurrentUserAsync(ClaimsPrincipal User)
        {
            var CurrentUserEmail = User.FindFirst(ClaimTypes.Email).Value;
            ApplicationUser currentUser=await _userManager.FindByEmailAsync(CurrentUserEmail);
            return currentUser;
        }
    }
}

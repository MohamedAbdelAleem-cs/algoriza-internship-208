using Core.DTOS.AccountDTOS;
using Core.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using Service;
using Service.Interfaces;
using System.Security.Claims;
using System.IO;

namespace VezeetaCloneWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _UserService;
        public SignInManager<ApplicationUser> _SignInManager { get; set; }

        public AccountController(IUserService UserService, SignInManager<ApplicationUser> signInManager)
        {
            _UserService = UserService;
            _SignInManager = signInManager;

        }

        [HttpPost("Register")]
        public async Task<ActionResult> RegisterAsync([FromBody]UserDetailsCreate user)
        {

            string ImgPath = ProcessUploadedFile(user.Image);
            user.Image = ImgPath;
            var res = await _UserService.RegisterAsync(user);
            if (!res.Succeeded)
            {
                UndoUploadedFile(ImgPath);
            }
            return Ok(res);
        }


        [HttpPost("Login")]
        public async Task<ActionResult> LoginAsync(LoginData loginData)
        {
            var result = await _SignInManager.PasswordSignInAsync(loginData.Email, loginData.Password, true, false);

            if (result.Succeeded)
            {
                var accountType = await _UserService.GetAccountTypeAsync(loginData.Email);
                //    var claims = new List<Claim>
                //{
                //new Claim(ClaimTypes.Email, loginData.Email), // Adding email as a claim
                //new Claim("AccountType",accountType.ToString())
                //};

                Claim claim = new Claim("AccountType", accountType.ToString());

                var claimsIdentity = new ClaimsIdentity(new[] { claim }, "AccountType");
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);


                HttpContext.SignInAsync(claimsPrincipal);
                HttpContext.User.AddIdentity(claimsIdentity);

                return Ok(new { Message = "Logged in successfully", Auth = User.Identity.IsAuthenticated});
            }

            return Unauthorized(new { Message = "Invalid credentials" });
        }



        [HttpPost("Logout")]
        public async Task<ActionResult> LogoutAsync()
        {
           await _SignInManager.SignOutAsync();
            return Ok("Logged out successfuly");
        }





        #region For Debugging Purposes
        /*
        [HttpGet("CurrentUser")]
        public ActionResult GetCurrentUserDetails()
        {

            if (User.Identity.IsAuthenticated)
            {
                var userEmailClaim = User.FindFirst(ClaimTypes.Email);
                if (userEmailClaim != null)
                {
                    var userEmail = userEmailClaim.Value;
                    return Ok(new { Email = userEmail });
                }
                return BadRequest(new { Message = "Email claim not found" });
            }

            return Unauthorized(new { Message = "User not authenticated" });
        }

        [HttpGet("CheckClaims")]
        public IActionResult CheckClaims()
        {
           
            var userEmailClaim = User.FindFirst(ClaimTypes.Email);
            var accountTypeClaim = User.FindFirst("AccountType");

            if (userEmailClaim != null && accountTypeClaim != null)
            {
                var userEmail = userEmailClaim.Value;
                var accountType = accountTypeClaim.Value;

                return Ok(new { Email = userEmail, AccountType = accountType });
            }

            return BadRequest(new { Message = "Claims not found" });
        }*/
        #endregion


        private string ProcessUploadedFile(string imgPath)
        {
            string uniqueFileName = null;

            if (imgPath != null && System.IO.File.Exists(imgPath))
            {
                string projectRoot = Directory.GetCurrentDirectory();
                string uploadsFolder = Path.Combine(projectRoot, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(imgPath);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Check if the directory exists, if not, create it
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                System.IO.File.Copy(imgPath, filePath); // Copy the file to the target location



            }



            return uniqueFileName;
        }

        private void UndoUploadedFile(string filePath)
        {
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath); // Delete the file
            }
        }
    }
}

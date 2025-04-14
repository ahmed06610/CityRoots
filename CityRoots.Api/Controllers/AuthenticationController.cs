using AutoMapper;
using CityRoots.Core.Const;
using CityRoots.Core.DTOs.Auth;
using CityRoots.Core.Interfaces;
using CityRoots.Core.Models;
using CityRoots.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CityRoots.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMailingService _mailingService;
        private readonly IMapper _mapper;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _webHostEnvironment; // Inject IWebHostEnvironment

        public AuthenticationController(IAuthService authService, UserManager<ApplicationUser> userManager,
            IMailingService mailingService, IMapper mapper, Microsoft.AspNetCore.Hosting.IHostingEnvironment webHostEnvironment, IUnitOfWork unitOfWork)
        {
            _authService = authService;
            _userManager = userManager;
            _mailingService = mailingService;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _unitOfWork = unitOfWork;
        }
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromForm] RegisterDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!EmailValidatorService.IsValidEmailProvider(model.Email))
                return BadRequest("Invalid Email");

            //model.UserName = model.Email;
            try
            {
                var result = await _authService.RegisterAsync(model);
                if (!result.IsAuthenticated)
                    return BadRequest(result.Message);
            }
            catch (Exception ex)
            {

                return BadRequest(ex);
            }
          
            return Ok("Successfull Register.....Please Go To Login");
        }
        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return BadRequest("UserId and token must be supplied for email confirmation.");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return BadRequest($"Unable to load user with ID '{userId}'.");
            }

            if (await _userManager.IsEmailConfirmedAsync(user))
                return BadRequest("Email is already confirmed");

            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                return Ok("Email confirmed successfully.");
            }

            return BadRequest("Error confirming email.");
        }
        [HttpPost("login")]// Login for teacher or parent or admin
        public async Task<IActionResult> LoginAsync(LoginDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _authService.LoginAsync(model);
            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (!user.EmailConfirmed)
                return BadRequest("Email not confirmed");
            var LoggedId = 0;
            var role = "";
           if(result.Roles.First()==Roles.Farmer.ToString())
            {
                LoggedId = (await _unitOfWork.Farmer.GetByAppUserIdAsync(user.Id)).FarmerId;
                role = Roles.Farmer.ToString();
            }
           else if (result.Roles.First() == Roles.Investor.ToString())
            {
                LoggedId = (await _unitOfWork.Investor.GetByAppUserIdAsync(user.Id)).InvestorId;
                role = Roles.Investor.ToString();
            }
            else if (result.Roles.First() == Roles.Merchant.ToString())
            {
                LoggedId = (await _unitOfWork.Merchant.GetByAppUserIdAsync(user.Id)).MerchantId;
                role = Roles.Merchant.ToString();
            }

            return Ok(new
            {
                token = result.Token,
                expiresOn = result.ExpiresOn,
                UserId = user.Id,
                Email = user.Email,
                Name = user.Name,
                LoggedId = LoggedId,
                Role = role,
                ImageUrl = user.ImageProfileUrl,
            });
        }

        [HttpPost("forget-password")]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return BadRequest("لم يتم العثور علي الإيميل");

            // Generate the reset code
            string resetCode = _mailingService.GenerateCode();

            // Set the reset code and expiration time
            user.ResetPasswordCode = resetCode;
            user.ResetCodeExpiry = DateTime.UtcNow.AddMinutes(30); // Expiry time of 30 minutes

            // Save the reset code and expiration time in the database
            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
                return StatusCode(500, "An error occurred while updating the user record.");

            // Load the email template
            var filePath = Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot", "ResetPassword.html");
            string emailBody = await System.IO.File.ReadAllTextAsync(filePath);

            // Customize email body with reset code if needed
            emailBody = emailBody.Replace("{ResetCode}", resetCode);

            // Send the reset code via email
            await _mailingService.SendEmailAsync(
                model.Email,
                "Code For Reset Password",
                emailBody // Using the modified template with the reset code
            );

            return Ok("تم إرسال رمز التأكيد الي ايميلك");
        }
        [HttpPost("check-reset-code")]
        public async Task<IActionResult> CheckResetCode(CheckResetCodeDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return BadRequest("لم يتم العثور علي الإيميل");

            // Check if the reset code matches and has not expired
            if (user.ResetPasswordCode != model.ResetCode || user.ResetCodeExpiry < DateTime.UtcNow)
                return BadRequest("The reset code is invalid or has expired.");

            // Code is valid
            return Ok(new
            {
                Message = "Reset code is valid."
            ,
                Token = (await _authService.CheakResetPassword(model)).Token
            });
        }
        [Authorize]
        [HttpPost("reset-password")]
        public async Task<IActionResult> ChangePassword(ResetPassowrdDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return BadRequest("لم يتم العثور علي الإيميل");


            // Reset the password
            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, resetToken, model.NewPassword);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            // Clear the reset code after successful password reset
            user.ResetPasswordCode = null;
            user.ResetCodeExpiry = null;
            await _userManager.UpdateAsync(user);

            return Ok("تم تغير كلمة السر بنجاح.");
        }

        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized("User ID not found in token.");
            }

            var result = await _authService.ChangePasswordAsync(userId, model);
            if (!result.IsAuthenticated)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Message);
        }

        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetProfileInfo()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            if (userId == null || role == null)
                return Unauthorized();

            var profileInfo = await _authService.GetProfileInfoAsync(userId, role);
            if (profileInfo == null)
                return NotFound("User profile not found.");

            return Ok(profileInfo);
        }

        [HttpPut("profile")]
        [Authorize]
        public async Task<IActionResult> EditProfile([FromForm] EditProfileDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            if (userId == null || role == null)
                return Unauthorized();

            var success = await _authService.EditProfileAsync(userId, role, model);
            if (!success)
                return BadRequest("Failed to update profile.");

            return Ok("Profile updated successfully.");
        }




    }
}

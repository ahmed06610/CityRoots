using AutoMapper;
using CityRoots.Core.Const;
using CityRoots.Core.DTOs.Auth;
using CityRoots.Core.Helpers;
using CityRoots.Core.Interfaces;
using CityRoots.Core.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly JWT _jwt;
        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMailingService _mailingService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHostingEnvironment _webHostEnvironment; // Inject IWebHostEnvironment
        public AuthService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,
            IMapper mapper, IOptions<JWT> jwt, IUrlHelperFactory urlHelperFactory, IHttpContextAccessor httpContextAccessor, IMailingService mailingService, IHostingEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _jwt = jwt.Value;
            _urlHelperFactory = urlHelperFactory;
            _httpContextAccessor = httpContextAccessor;
            _mailingService = mailingService;
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<AuthDTO> RegisterAsync(RegisterDTO model)
        {
            if (await _userManager.FindByEmailAsync(model.Email) is not null)
                return new AuthDTO { Message = "Email is already registered!" };

            var user = _mapper.Map<ApplicationUser>(model);
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                var errors = string.Empty;
                foreach (var error in result.Errors)
                {
                    errors += $"{error.Description}, ";
                }
                return new AuthDTO { Message = errors };
            }

            await _userManager.AddToRoleAsync(user, model.Role);
            //Add Investor or Farmer or Merchant Account


            if (model.Role == Roles.Farmer.ToString())
            {
                var farmer = new Farmer
                {
                    ApplicationUserId = _userManager.FindByEmailAsync(model.Email).Result.Id,
                    Bio = model.Bio

                };

                await _unitOfWork.Farmer.AddAsync(farmer);
                await _unitOfWork.CompleteAsync();
            }
            else if(model.Role == Roles.Investor.ToString())
            {
                var investor = new Investor
                {
                    ApplicationUserId = _userManager.FindByEmailAsync(model.Email).Result.Id,
                    Bio = model.Bio

                };
                await _unitOfWork.Investor.AddAsync(investor);
                await _unitOfWork.CompleteAsync();
            }
            else if (model.Role == Roles.Merchant.ToString())
            {
                var merchant = new Merchant
                {
                    ApplicationUserId = _userManager.FindByEmailAsync(model.Email).Result.Id,
                    BusinessDetails = model.Bio
                };
                await _unitOfWork.Merchant.AddAsync(merchant);
                await _unitOfWork.CompleteAsync();
            }

            //
            var jwtSecurityToken = await CreateJwtToken(user);

            //Verification Code
            var userId = await _userManager.GetUserIdAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var urlHelper = _urlHelperFactory.GetUrlHelper(new ActionContext(
                _httpContextAccessor.HttpContext,
                _httpContextAccessor.HttpContext.GetRouteData(),
                new ActionDescriptor()));


            var verificationUrl = _httpContextAccessor.HttpContext.Request.Scheme + "://" + _httpContextAccessor.HttpContext.Request.Host
                + urlHelper.Action("ConfirmEmail", "Authentication", new { userId = userId, code = code });


            var filePath = Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot", "EmailTemplate.html");

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Email template not found.", filePath);
            }

            var str = new StreamReader(filePath);

            var mailText = str.ReadToEnd();
            str.Close();

            mailText = mailText.Replace("[name]", user.Name).Replace("[email]", user.Email).Replace("[link]", verificationUrl);
            await _mailingService.SendEmailAsync(user.Email, "verification Code", mailText);

            ////////////////////////////////////////

            var role = "";

            string roleValue = model.Role.Replace(" ", ""); // Remove spaces

            if (string.Equals(roleValue, Roles.Investor.ToString(), StringComparison.OrdinalIgnoreCase))
                role = Roles.Investor.ToString();
            else if (string.Equals(roleValue, Roles.Farmer.ToString(), StringComparison.OrdinalIgnoreCase))
                role = Roles.Farmer.ToString();
            else
                role = Roles.Merchant.ToString();

            return new AuthDTO
            {
                Email = user.Email,
                IsAuthenticated = true,
                ExpiresOn = jwtSecurityToken.ValidTo,
                Roles = new List<string> { role },
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken)
            };
        }

        public async Task<AuthDTO> LoginAsync(LoginDTO model)
        {
            var authModel = new AuthDTO();
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                authModel.Message = "Email or Password is incorrect!";
                return authModel;
            }
            return await CreateAuthModelAsync(user);

        }

        public async Task<AuthDTO> CheakResetPassword(CheckResetCodeDTO model)
        {
            var authModel = new AuthDTO();
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null)
            {
                authModel.Message = "Email is incorrect!";
                return authModel;
            }
            return await CreateAuthModelAsync(user);

        }
        private async Task<AuthDTO> CreateAuthModelAsync(ApplicationUser user)
        {
            var authModel = new AuthDTO();

            // Retrieve roles and create JWT token
            var roles = await _userManager.GetRolesAsync(user);
            var jwtSecurityToken = await CreateJwtToken(user);

            // Populate the AuthDTO object
            authModel.Email = user.Email;
            
            authModel.ExpiresOn = jwtSecurityToken.ValidTo;
            authModel.IsAuthenticated = true;
            authModel.Roles = roles.ToList();
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return authModel;
        }

        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            // Retrieve user claims and roles
            var userClaims = await _userManager.GetClaimsAsync(user);
            var userRoles = await _userManager.GetRolesAsync(user);

            // Create role claims
            var roleClaims = userRoles.Select(role => new Claim(ClaimTypes.Role, role)).ToList();
            var userIdLogged = "";
            if (userRoles.Contains(Roles.Farmer.ToString()))
            {
                userIdLogged = ((await _unitOfWork.Farmer.GetByAppUserIdAsync(user.Id.ToString())).FarmerId).ToString();
            }
            else if (userRoles.Contains(Roles.Investor.ToString()))
            {
                userIdLogged = ((await _unitOfWork.Investor.GetByAppUserIdAsync(user.Id.ToString())).InvestorId).ToString();
            }
            else
            {
                userIdLogged = ((await _unitOfWork.Merchant.GetByAppUserIdAsync(user.Id.ToString())).MerchantId).ToString();
            }

            // Combine all claims into a single list
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),  // User ID
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),     // Username
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // JWT ID
                new Claim(JwtRegisteredClaimNames.Email, user.Email),      // Email
                new Claim("LoggedId",userIdLogged.ToString())              // Id Of the LoggedIn User
            }
            .Union(userClaims)       // Include additional claims from user
            .Union(roleClaims);      // Include role claims

            // Create the signing key using the secret key
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            // Create the JWT token
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                expires: DateTime.UtcNow.AddDays(_jwt.DurationInDays), // Use UTC for expiration
                signingCredentials: signingCredentials,
                claims: claims
            );

            return jwtSecurityToken;
        }

    }

}

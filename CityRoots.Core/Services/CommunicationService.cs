using AutoMapper;
using CityRoots.Core.DTOs.FeedBack;
using CityRoots.Core.Helpers;
using CityRoots.Core.Interfaces;
using CityRoots.Core.Interfaces.Services;
using CityRoots.Core.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Org.BouncyCastle.Bcpg;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CityRoots.Core.Services
{
    public class CommunicationService:ICommunicationService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IMailingService mailingService;
        private readonly IHostingEnvironment webHostEnvironment;
        private readonly IConfiguration _configuration;
        public CommunicationService(IUnitOfWork unitOfWork,IMapper mapper,IHttpContextAccessor httpContextAccessor,IMailingService mailingService,IHostingEnvironment webHostEnvironment,IConfiguration configuration) { 
        this.unitOfWork = unitOfWork;
            this.mapper=mapper;
            this.httpContextAccessor=httpContextAccessor;
            this.mailingService=mailingService;
            this.webHostEnvironment=webHostEnvironment;
            this._configuration = configuration;
        
        }

        public async Task<FeedBackDisplay> Add(FeedBackRequest feedBack,string userId)
        {
            //var userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;//??throw new Exception("User authentication failed. User ID not found.");
            //if (string.IsNullOrEmpty(userId))
            //{
            //    throw new Exception("User authentication failed. User ID not found.");
            //}

            var _FeedBack = mapper.Map<FeedBack>(feedBack);
            _FeedBack.UserId = userId;
            await unitOfWork.FeedBack.AddAsync(_FeedBack);
           await unitOfWork.CompleteAsync();
            return mapper.Map<FeedBackDisplay>(_FeedBack);
            
        }

        public async  Task Delete(int id)
        {
            var feedback = await unitOfWork.FeedBack.GetByIdAsync(id);
            if (feedback is null)
                throw new Exception("The FeedBack Isnot Found");
            await  unitOfWork.FeedBack.DeleteAsync(feedback);
            await unitOfWork.CompleteAsync();
        }

        public async Task<IEnumerable<FeedBackDisplay>> GetAll()
        {
            var feedbacks=await unitOfWork.FeedBack.FindAllWithIncludes<FeedBack>(null,f=>f.User);
            return mapper.Map<IEnumerable<FeedBackDisplay>>(feedbacks);
        }

        public async Task<FeedBackDisplay> GetById(int id)
        {
            var feedback=await unitOfWork.FeedBack.GetByIdAsync(id);
            if (feedback is null)
                throw new Exception("The FeedBack Isnot Found");
            return mapper.Map<FeedBackDisplay>(feedback);
        }

      
        public async Task<FeedBackDisplay> Update(int id, FeedBackRequest feedBack)
        {
            var _feedback = await unitOfWork.FeedBack.GetByIdAsync(id);
            if (_feedback is null)
                throw new Exception("The FeedBack Isnot Found");
            mapper.Map(feedBack, _feedback);
            unitOfWork.FeedBack.Update(_feedback);
            await unitOfWork.CompleteAsync();
            return mapper.Map<FeedBackDisplay>(_feedback);
            
        }
        public async Task SendSupportAsync(Support support)
        {

            /* var userEmail = httpContextAccessor.HttpContext.User.FindFirst(JwtRegisteredClaimNames.Email)?.Value;
             var userName = httpContextAccessor.HttpContext.User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;*/
       /*     new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),  // User ID
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),     // Username
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // JWT ID
                new Claim(JwtRegisteredClaimNames.Email, user.Email),      // Email
                new Claim("LoggedId", userIdLogged.ToString()),        // Id Of the LoggedIn User
                 new Claim("NameOfuser", user.Name) //Name of User    */   


                var userEmail = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
            var userName = httpContextAccessor.HttpContext.User.FindFirstValue("NameOfuser") ?? string.Empty;
            if (userEmail is null || userName is null)
                throw new Exception("User Doesnot authenticated ");


            var filePath = Path.Combine(webHostEnvironment.ContentRootPath, "wwwroot", "SupportEmailTemplate.html");

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Support email template not found.", filePath);
            }

            var str = new StreamReader(filePath);
            var mailText = str.ReadToEnd();
            str.Close();

            mailText = mailText.Replace("[name]", userName)
                               .Replace("[email]", userEmail)
                               .Replace("[subject]", support.subject)
                               .Replace("[description]", support.Description);


            await mailingService.SendEmailAsync(_configuration["MailSettings:Email"],
                                                  support.subject,
                                                  mailText ,null,userEmail
                                                 );  
        }

        

    }
}

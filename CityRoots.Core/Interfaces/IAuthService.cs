using CityRoots.Core.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.Interfaces
{
    public interface IAuthService
    {
        Task<AuthDTO> RegisterAsync(RegisterDTO model);
        Task<AuthDTO> LoginAsync(LoginDTO model);
        Task<AuthDTO> CheakResetPassword(CheckResetCodeDTO model);
        Task<AuthDTO> ChangePasswordAsync(string userId, ChangePasswordDTO model);
        Task<ProfileInfoDTO> GetProfileInfoAsync(string userId, string role);
        Task<bool> EditProfileAsync(string userId, string role, EditProfileDTO model);




    }
}

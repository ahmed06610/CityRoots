using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;

namespace CityRoots.Core.CustomValidation
{
    public class CustomUserValidator<TUser> : IUserValidator<TUser> where TUser : IdentityUser
    {
        public Task<IdentityResult> ValidateAsync(UserManager<TUser> manager, TUser user)
        {
            var errors = new List<IdentityError>();

            if (string.IsNullOrWhiteSpace(user.UserName))
            {
                errors.Add(new IdentityError
                {
                    Code = "UsernameIsEmpty",
                    Description = "Username cannot be empty."
                });
            }
            else
            {
                // Explicitly allow Arabic, English letters, and digits
                var usernameRegex = new Regex(@"^[\u0600-\u06FF\u0750-\u077F\u08A0-\u08FF\p{L}\p{N}_\s]+$", RegexOptions.Compiled);
                if (!usernameRegex.IsMatch(user.UserName))
                {
                    errors.Add(new IdentityError
                    {
                        Code = "InvalidUsername",
                        Description = "Username can only contain Arabic, English letters, and digits."
                    });
                }
            }

            return Task.FromResult(errors.Any() ? IdentityResult.Failed(errors.ToArray()) : IdentityResult.Success);
        }
    }
}

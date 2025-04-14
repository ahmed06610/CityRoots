using System.Security.Claims;

namespace CityRoots.Api.Helpers
{
    public static class ClaimsPrincipalExtensions
    {
        public static int? GetLoggedInId(this ClaimsPrincipal user)
        {
            var value = user.FindFirst("LoggedId")?.Value;
            return int.TryParse(value, out var id) ? id : (int?)null;
        }
    }
}

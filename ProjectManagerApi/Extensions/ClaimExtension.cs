using System.Security.Claims;

namespace ProjectManagerApi.Extensions
{
    public static class ClaimExtension
    {
        public static int GetUserId(this ClaimsPrincipal? user)
        {
            return int.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value);
        }
    }
}

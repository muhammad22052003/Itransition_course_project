using System.Security.Claims;

namespace CourseProject_backend.Interfaces.Helpers
{
    public interface IJwtTokenHelper
    {
        public string GenerateToken(IEnumerable<Claim> claims, string key, int experiseHours);

        public IEnumerable<Claim> DeserializeToken(string token, string key);
    }
}

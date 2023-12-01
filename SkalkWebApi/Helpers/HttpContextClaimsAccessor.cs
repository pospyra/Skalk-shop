using Skalk.BLL.Interfaces;
using System.Security.Claims;

namespace SkalkWebApi.WebApi.Helpers
{
    public class HttpContextClaimsAccessor : IClaimAccessor
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public HttpContextClaimsAccessor(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public async Task<IEnumerable<Claim>> GetClaims()
        {
            return _contextAccessor.HttpContext.User.Claims;
        }
    }
}

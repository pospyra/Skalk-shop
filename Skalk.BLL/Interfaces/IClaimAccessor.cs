using System.Security.Claims;

namespace Skalk.BLL.Interfaces
{
    public interface IClaimAccessor
    {
        Task<IEnumerable<Claim>> GetClaims();
    }
}

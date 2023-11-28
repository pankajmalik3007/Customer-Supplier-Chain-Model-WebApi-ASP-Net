// Project imports
using Domain.Models;

namespace WebApi.Middleware.Auth
{
    public interface IJWTAuthManager
    {
        string GenerateJWT(User user);
    }
}

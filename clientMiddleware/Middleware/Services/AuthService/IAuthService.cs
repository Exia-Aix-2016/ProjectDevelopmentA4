using Middleware.Services;

namespace Middleware
{
    public interface IAuthService : IService
    {
        bool IsValidToken(string token);
        string UserLogin(string login, string pass);
    }
}
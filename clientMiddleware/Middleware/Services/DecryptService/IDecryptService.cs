using Models;

namespace Middleware.Services
{
    public interface IDecryptService : IService
    {
        void StopOperation(Message message);
    
    }
}
using Middleware.Models;

namespace Middleware.Services.Uncryption
{
    public interface IDecryptService : IService
    {
        void StopOperation(Message message);
    
    }
}
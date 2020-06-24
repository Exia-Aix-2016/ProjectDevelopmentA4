using Middleware;

namespace Client
{
    public interface IComm
    {
        void notify(Message message);
    }
}

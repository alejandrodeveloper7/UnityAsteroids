using Asteroids.Core.Interfaces.Models;

namespace Asteroids.Core.Interfaces
{
    public interface IPushable
    {
        void Push(PushInfo pushInfo);
    }
}
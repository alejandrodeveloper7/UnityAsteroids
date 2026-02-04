using System;

namespace Asteroids.Core.Services
{
    public interface IRuntimeDataService
    {
        string AuthToken { get; set; }
        string RefreshToken { get; set; }
        DateTime TokenExpiration { get; set; }
    }
}
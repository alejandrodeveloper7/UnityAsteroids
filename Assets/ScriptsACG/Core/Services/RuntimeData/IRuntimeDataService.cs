using System;

namespace ToolsACG.Core.Services
{
    public interface IRuntimeDataService
    {
        string AuthToken { get; set; }
        string RefreshToken { get; set; }
        DateTime TokenExpiration { get; set; }
    }
}
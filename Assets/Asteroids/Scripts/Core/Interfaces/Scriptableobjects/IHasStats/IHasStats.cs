using Asteroids.Core.Interfaces.Models;
using System.Collections.Generic;

namespace Asteroids.Core.Interfaces
{
    public interface IHasStats
    {
        List<StatData> Stats { get; }
    }
}
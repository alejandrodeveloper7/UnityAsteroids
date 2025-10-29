using ToolsACG.SOCreator.Interfaces;
using UnityEngine;

namespace Asteroids.Core.Interfaces
{
    public interface ISelectorElement : IData
    {
        bool IsActive { get; }
        Sprite Sprite { get; }
    }
}
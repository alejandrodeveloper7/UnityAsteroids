using System;

namespace ToolsACG.Core.Managers
{
    public interface IInputManager 
    {
        event Action<int> RotationtKeysStateChanged;
        event Action<bool> ShootKeyStateChanged;
        event Action<bool> MoveForwardKeyStateChanged;
    }
}
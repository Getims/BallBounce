using System;
using Main.Scripts.Core.Enums;

namespace Main.Scripts.Infrastructure.ScenesManager
{
    public interface ISceneLoader
    {
        void Load(string name, Action onLoaded = null);
        void Load(Scenes scene, Action onLoaded = null);
    }
}
using System;
using Main.Scripts.Core.Enums;
using Main.Scripts.Infrastructure.ScenesManager;

namespace Main.Scripts.Infrastructure.StateMachines.States.Load
{
    public abstract class LoadSceneState
    {
        private readonly ISceneLoader _sceneLoader;

        protected LoadSceneState(ISceneLoader sceneLoader) =>
            _sceneLoader = sceneLoader;

        protected void Enter(string scene, Action onLoaded) =>
            _sceneLoader.Load(scene, onLoaded);

        protected void Enter(Scenes scene, Action onLoaded) =>
            _sceneLoader.Load(scene, onLoaded);
    }
}
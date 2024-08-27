using Main.Scripts.Core.Enums;
using Main.Scripts.Infrastructure.ScenesManager;
using Main.Scripts.Infrastructure.StateMachines.BaseStates;
using Main.Scripts.Infrastructure.StateMachines.States.Global;

namespace Main.Scripts.Infrastructure.StateMachines.States.Load
{
    public class LoadGameLoaderSceneState : LoadSceneState, IEnterState
    {
        private readonly GameStateMachine _stateMachine;

        public LoadGameLoaderSceneState(GameStateMachine stateMachine, ISceneLoader sceneLoader) : base(sceneLoader) =>
            _stateMachine = stateMachine;

        public void Enter() =>
            base.Enter(Scenes.GameLoader, OnLoaded);

        private void OnLoaded() =>
            _stateMachine.Enter<GameLoaderState>();
    }
}
using Main.Scripts.Data.Services;
using Main.Scripts.Infrastructure.Providers.Configs;
using Main.Scripts.Infrastructure.Providers.Events;
using Main.Scripts.Infrastructure.StateMachines.BaseStates;
using Main.Scripts.Infrastructure.StateMachines.States.Load;

namespace Main.Scripts.Infrastructure.StateMachines.States.Global
{
    public class GameLoaderState : IEnterState
    {
        private readonly IGameStateMachine _stateMachine;
        private readonly GlobalEventProvider _globalEventsProvider;

        public GameLoaderState(IGameStateMachine stateMachine, IGlobalConfigProvider globalConfigProvider,
            GlobalEventProvider globalEventsProvider, IProgressDataService progressDataService)
        {
            _globalEventsProvider = globalEventsProvider;
            _stateMachine = stateMachine;
            _globalEventsProvider.AddListener<GameLoadCompleteEvent>(MoveToNextState);
        }

        public void Enter()
        {
        }

        private void MoveToNextState()
        {
            _globalEventsProvider.RemoveListener<GameLoadCompleteEvent>(MoveToNextState);
            _stateMachine.Enter<LoadGameSceneState>();
        }
    }
}
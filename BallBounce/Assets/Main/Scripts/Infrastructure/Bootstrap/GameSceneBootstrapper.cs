using Main.Scripts.GameLogic.Sound;
using Main.Scripts.Infrastructure.Providers.Configs;
using Main.Scripts.Infrastructure.StateMachines;
using Main.Scripts.Infrastructure.StateMachines.States.GameScene;
using Main.Scripts.UI;
using Main.Scripts.UI.Base;
using Zenject;

namespace Main.Scripts.Infrastructure.Bootstrap
{
    public class GameSceneBootstrapper
    {
        private readonly DiContainer _container;
        private readonly ISoundService _soundService;
        private readonly IUIConfigProvider _uiConfigProvider;
        private readonly UIContainerProvider _uiContainerProvider;
        private GameStateMachine _gameLoopStateMachine;

        public GameSceneBootstrapper(DiContainer container, ISoundService soundService,
            IUIConfigProvider uiConfigProvider, UIContainerProvider uiContainerProvider)
        {
            _uiContainerProvider = uiContainerProvider;
            _uiConfigProvider = uiConfigProvider;
            _container = container;
            _soundService = soundService;
        }

        public void Initialize()
        {
            SetupMenuFactory();
            _soundService.PlayGameBackgroundMusic();

            _gameLoopStateMachine = SetupStateMachine();
            _gameLoopStateMachine.Enter<PrepareGamePlayState, bool>(false);
        }

        public void OnDestroy() =>
            _gameLoopStateMachine.Enter<StopMachineState>();

        private void SetupMenuFactory()
        {
            UIMenuFactory uiMenuFactory = new UIMenuFactory(_container, _uiConfigProvider.Config,
                _uiContainerProvider.MenuContainer);
            _container.Bind<IUIMenuFactory>().FromInstance(uiMenuFactory).AsSingle().NonLazy();
        }

        private GameStateMachine SetupStateMachine()
        {
            StateMachineFactory stateMachineFactory = _container.Instantiate<StateMachineFactory>();
            GameStateMachine gameLoopStateMachine = _container.Instantiate<GameStateMachine>();
            _container.BindInstance(gameLoopStateMachine);

            stateMachineFactory.BindState<PrepareGamePlayState>(gameLoopStateMachine);
            stateMachineFactory.BindState<GamePlayState>(gameLoopStateMachine);
            stateMachineFactory.BindState<GameOverState>(gameLoopStateMachine);
            stateMachineFactory.BindState<StopMachineState>(gameLoopStateMachine);
            return gameLoopStateMachine;
        }
    }
}
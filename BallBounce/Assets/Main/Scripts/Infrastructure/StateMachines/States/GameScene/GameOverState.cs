using Main.Scripts.Data.Services;
using Main.Scripts.Infrastructure.StateMachines.BaseStates;
using Main.Scripts.UI;
using Main.Scripts.UI.Base;
using Main.Scripts.UI.GameOver;

namespace Main.Scripts.Infrastructure.StateMachines.States.GameScene
{
    public class GameOverState : IEnterState<bool>, IExitState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly IUIMenuFactory _uiMenuFactory;
        private readonly int _additionalTime;
        private readonly UIContainerProvider _uiContainerProvider;
        private readonly IProgressDataService _progressDataService;
        private readonly IPlayerDataService _playerDataService;

        private LevelCompletePanel _levelCompletePanel;

        public GameOverState(GameStateMachine stateMachine, IUIMenuFactory uiMenuFactory,
            UIContainerProvider uiContainerProvider, IProgressDataService progressDataService,
            IPlayerDataService playerDataService)
        {
            _progressDataService = progressDataService;
            _uiContainerProvider = uiContainerProvider;
            _uiMenuFactory = uiMenuFactory;
            _stateMachine = stateMachine;
            _playerDataService = playerDataService;
        }

        public void Enter(bool isWin)
        {
            if (isWin)
            {
                CreateLevelCompletePanel();
                _levelCompletePanel.Show();
            }
            else
                ResetLevel();
        }

        public void Exit()
        {
        }

        private void CreateLevelCompletePanel()
        {
            if (_levelCompletePanel != null)
                return;

            _levelCompletePanel = _uiMenuFactory.Create<LevelCompletePanel>(_uiContainerProvider.WindowsContainer);
            _levelCompletePanel.OnSwitchButtonClickEvent += SwitchLevel;
        }

        private void SwitchLevel()
        {
            _progressDataService.SwitchToNextLevel(false);
            _progressDataService.ResetLevelProgress();
            _playerDataService.SetMoney(0);

            _stateMachine.Enter<PrepareGamePlayState, bool>(true);
        }

        private void ResetLevel()
        {
            _progressDataService.ResetLevelProgress();
            _playerDataService.SetMoney(0);

            _stateMachine.Enter<PrepareGamePlayState, bool>(true);
        }
    }
}
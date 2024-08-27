using Main.Scripts.Data.Services;
using Main.Scripts.Infrastructure.Providers.Events;
using Main.Scripts.Infrastructure.StateMachines.BaseStates;
using Main.Scripts.UI;
using Main.Scripts.UI.Base;
using Main.Scripts.UI.GameMenu;
using Main.Scripts.UI.GameMenu.Boosters;
using Main.Scripts.UI.GameMenu.Prize;
using Main.Scripts.UI.GameMenu.Settings;
using Main.Scripts.UI.GameMenu.SideMenu;

namespace Main.Scripts.Infrastructure.StateMachines.States.GameScene
{
    public class GamePlayState : IEnterState, IExitState
    {
        private readonly IUIMenuFactory _uiMenuFactory;
        private readonly GameStateMachine _stateMachine;
        private readonly GlobalEventProvider _globalEventProvider;
        private readonly UIContainerProvider _uiContainerProvider;
        private readonly IPlayerDataService _playerDataService;

        private BoostersPanel _boostersPanel;
        private SettingsPanel _settingsPanel;
        private SideMenuPanel _sideMenuPanel;
        private TopGamePanel _topGamePanel;
        private PrizePanel _prizePanel;

        public GamePlayState(GameStateMachine stateMachine, IUIMenuFactory uiMenuFactory,
            GlobalEventProvider globalEventProvider, UIContainerProvider uiContainerProvider,
            IPlayerDataService playerDataService)
        {
            _playerDataService = playerDataService;
            _globalEventProvider = globalEventProvider;
            _uiContainerProvider = uiContainerProvider;
            _stateMachine = stateMachine;
            _uiMenuFactory = uiMenuFactory;
        }

        public void Enter()
        {
            if (_boostersPanel == null)
                _boostersPanel = _uiMenuFactory.GetPanel<BoostersPanel>();

            if (_topGamePanel == null)
                _topGamePanel = _uiMenuFactory.GetPanel<TopGamePanel>();

            if (_sideMenuPanel == null)
            {
                _sideMenuPanel = _uiMenuFactory.GetPanel<SideMenuPanel>();
                _sideMenuPanel.OnSettingsClick += OnSettingsButtonClick;
            }

            _globalEventProvider.AddListener<GameOverEvent, bool>(OnGameOver);
            _globalEventProvider.AddListener<GivePrizeRewardEvent, int>(CreatePrizePanel);
        }

        public void Exit()
        {
            if (_prizePanel != null)
                _prizePanel.Hide();

            _globalEventProvider.RemoveListener<GameOverEvent, bool>(OnGameOver);
            _globalEventProvider.RemoveListener<GivePrizeRewardEvent, int>(CreatePrizePanel);
        }

        private void OnGameOver(bool isWin) =>
            _stateMachine.Enter<GameOverState, bool>(isWin);

        private void OnSettingsButtonClick()
        {
            if (_settingsPanel == null)
                _settingsPanel = _uiMenuFactory.Create<SettingsPanel>();
            _settingsPanel.Show();
        }

        private void CreatePrizePanel(int reward)
        {
            if (_prizePanel == null)
            {
                _prizePanel = _uiMenuFactory.Create<PrizePanel>(_uiContainerProvider.WindowsContainer);
                _prizePanel.OnPrizeCollectClick += OnPrizeCollect;
            }

            _prizePanel.Initialize(reward);
            _prizePanel.Show();
        }

        private void OnPrizeCollect()
        {
            _topGamePanel.ShowMoneyReward(_prizePanel.MoneyIconPosition,
                () => _playerDataService.AddMoney(_prizePanel.Reward));
        }
    }
}
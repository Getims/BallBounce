using System.Collections;
using Main.Scripts.Data.Services;
using Main.Scripts.GameLogic.GameFlow;
using Main.Scripts.Infrastructure.Bootstrap;
using Main.Scripts.Infrastructure.Providers.Configs;
using Main.Scripts.Infrastructure.StateMachines.BaseStates;
using Main.Scripts.UI;
using Main.Scripts.UI.Base;
using Main.Scripts.UI.DebugPanel;
using Main.Scripts.UI.GameMenu;
using Main.Scripts.UI.GameMenu.Boosters;
using Main.Scripts.UI.GameMenu.Controls;
using Main.Scripts.UI.GameMenu.SideMenu;
using Main.Scripts.UI.GameMenu.Timers;
using UnityEngine;

namespace Main.Scripts.Infrastructure.StateMachines.States.GameScene
{
    public class PrepareGamePlayState : IEnterState<bool>, IExitState
    {
        private const float SCENE_LOAD_TIME = 0.15f;

        private readonly GameStateMachine _stateMachine;
        private readonly IUIMenuFactory _uiMenuFactory;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly IGameFlowProvider _gameFlowProvider;
        private readonly UIContainerProvider _uiContainerProvider;
        private readonly IPlayerDataService _playerDataService;
        private readonly IProgressDataService _progressDataService;

        private TopGamePanel _topGamePanel;
        private BoostersPanel _boostersPanel;
        private ControlPanel _controlPanel;
        private SideMenuPanel _sideMenuPanel;
        private TimersPanel _timersPanel;
        private Coroutine _levelCreationCO;
        private bool _resetMoney;
        private bool _enableDebug;

        public PrepareGamePlayState(GameStateMachine stateMachine, IUIMenuFactory uiMenuFactory,
            ICoroutineRunner coroutineRunner, IGameFlowProvider gameFlowProvider,
            UIContainerProvider uiContainerProvider, IPlayerDataService playerDataService,
            IProgressDataService progressDataService, IGlobalConfigProvider globalConfigProvider)
        {
            _enableDebug = globalConfigProvider.Config.EnableDebug;
            _progressDataService = progressDataService;
            _playerDataService = playerDataService;
            _uiContainerProvider = uiContainerProvider;
            _gameFlowProvider = gameFlowProvider;
            _coroutineRunner = coroutineRunner;
            _uiMenuFactory = uiMenuFactory;
            _stateMachine = stateMachine;
        }

        public void Enter(bool resetMoney = false)
        {
            _resetMoney = resetMoney;
            if (_levelCreationCO != null)
                _coroutineRunner.StopCoroutine(_levelCreationCO);

            _levelCreationCO = _coroutineRunner.StartCoroutine(CreateLevel());
        }

        public void Exit()
        {
            if (_levelCreationCO != null)
                _coroutineRunner?.StopCoroutine(_levelCreationCO);
        }

        private void CreateTopPanel(bool show = true)
        {
            if (_topGamePanel == null)
                _topGamePanel = _uiMenuFactory.Create<TopGamePanel>(_uiContainerProvider.MenuContainer);

            if (show)
                _topGamePanel.Show();
        }

        private void CreateBoostersPanel(bool show = true)
        {
            if (_boostersPanel == null)
            {
                _boostersPanel = _uiMenuFactory.Create<BoostersPanel>(_uiContainerProvider.MenuContainer);
                _boostersPanel.Initialize(_timersPanel.CreateTimer);
            }

            if (show)
                _boostersPanel.Show();
        }

        private void CreateControlPanel(bool show = true)
        {
            if (_controlPanel == null)
            {
                _controlPanel = _uiMenuFactory.Create<ControlPanel>(_uiContainerProvider.MenuContainer);
                _controlPanel.Initialize();
            }

            if (show)
                _controlPanel.Show();
        }

        private void CreateSideMenuPanel(bool show = true)
        {
            if (_sideMenuPanel == null)
                _sideMenuPanel = _uiMenuFactory.Create<SideMenuPanel>(_uiContainerProvider.MenuContainer);

            if (show)
                _sideMenuPanel.Show();
        }

        private void CreateTimersPanel(bool show = true)
        {
            if (_timersPanel == null)
                _timersPanel = _uiMenuFactory.Create<TimersPanel>(_uiContainerProvider.MenuContainer);

            if (show)
                _timersPanel.Show();
        }

        private void CreateDebugPanel()
        {
            if (!_enableDebug)
                return;

            _enableDebug = false;
            _uiMenuFactory.Create<DebugUI>(_uiContainerProvider.WindowsContainer);
        }

        IEnumerator CreateLevel()
        {
            yield return new WaitForSeconds(SCENE_LOAD_TIME);
            while (!_gameFlowProvider.IsLoaded)
                yield return new WaitForEndOfFrame();

            _gameFlowProvider.Initialize();
            yield return new WaitForEndOfFrame();

            CreateTopPanel();
            CreateDebugPanel();
            yield return new WaitForEndOfFrame();
            CreateTimersPanel();
            CreateBoostersPanel();
            yield return new WaitForEndOfFrame();
            CreateControlPanel();
            yield return new WaitForEndOfFrame();
            CreateSideMenuPanel();
            yield return new WaitForEndOfFrame();

            if (_resetMoney)
            {
                _playerDataService.SetMoney(0);
                _progressDataService.SetLevelProgress(0);
            }

            yield return new WaitForSeconds(_topGamePanel.FadeTime);
            _gameFlowProvider.StartGame();

            _stateMachine.Enter<GamePlayState>();
        }
    }
}
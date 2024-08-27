using Main.Scripts.GameLogic.GameFlow;
using Main.Scripts.Infrastructure.Bootstrap;
using Main.Scripts.Infrastructure.Providers.Configs;
using Main.Scripts.UI;
using UnityEngine;
using Zenject;

namespace Main.Scripts.Infrastructure.Installers
{
    public class GameSceneInstaller : MonoInstaller
    {
        [SerializeField]
        private UIContainerProvider _uiContainerProvider;

        [SerializeField]
        private GameFlowController _gameFlowController;

        private GameSceneBootstrapper _bootstrapper;

        public override void InstallBindings()
        {
            BindConfigProviders();
            BindUIContainerProvider();
            BindGameFlow();

            CreateSceneBootstrapper();
        }

        private void OnDestroy()
        {
            _bootstrapper.OnDestroy();
        }

        private void BindConfigProviders()
        {
            Container.BindInterfacesTo<GameLevelsConfigProvider>().AsSingle().NonLazy();
            Container.BindInterfacesTo<BallProgressionConfigProvider>().AsSingle().NonLazy();
            Container.BindInterfacesTo<UIConfigProvider>().AsSingle().NonLazy();
        }

        private void BindUIContainerProvider() =>
            Container.BindInstance(_uiContainerProvider).AsSingle().NonLazy();

        private void BindGameFlow() =>
            Container.Bind<IGameFlowProvider>().FromInstance(_gameFlowController).AsSingle().NonLazy();

        private void CreateSceneBootstrapper()
        {
            _bootstrapper = Container.Instantiate<GameSceneBootstrapper>();
            _bootstrapper.Initialize();
        }
    }
}
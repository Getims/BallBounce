using Main.Scripts.Data;
using Main.Scripts.Data.Core;
using Main.Scripts.Data.Services;
using Main.Scripts.GameLogic.Sound;
using Main.Scripts.Infrastructure.Bootstrap;
using Main.Scripts.Infrastructure.Providers.Assets;
using Main.Scripts.Infrastructure.Providers.Configs;
using Main.Scripts.Infrastructure.Providers.Events;
using Main.Scripts.Infrastructure.ScenesManager;
using Main.Scripts.Infrastructure.StateMachines;
using Main.Scripts.UI.Base;
using Main.Scripts.UI.Loadscreen;
using UnityEngine;
using Zenject;

namespace Main.Scripts.Infrastructure.Installers
{
    public class GameCoreInstaller : MonoInstaller
    {
        [SerializeField]
        private SoundPlayer _soundPlayer;

        [SerializeField]
        private GameCoreBootstrapper _gameCoreBootstrapper;

        [SerializeField]
        private LoadingPanel _loadingPanel;

        public override void InstallBindings()
        {
            BindProviders();
            BindDatabase();
            BindServices();

            BindSoundPlayer();

            BindFactories();
            BindGameStateMachine();

            BindGameBootstrapper();
            BindSceneLoader();
        }

        private void BindProviders()
        {
            Container.BindInterfacesTo<AssetProvider>().AsSingle().NonLazy();
            Container.BindInterfacesTo<GlobalConfigProvider>().AsSingle().NonLazy();
            Container.BindInterfacesTo<SoundConfigProvider>().AsSingle().NonLazy();
            Container.Bind<GlobalEventProvider>().AsSingle().NonLazy();
        }

        private void BindDatabase()
        {
            IDatabase database = new GameDatabase();
            database.Initialize();
            Container.BindInstance(database).AsSingle().NonLazy();

#if UNITY_EDITOR
            DataEditorMediator.SetDatabase(database);
#endif
        }

        private void BindServices()
        {
            Container.BindInterfacesTo<PlayerDataService>().AsSingle().NonLazy();
            Container.BindInterfacesTo<ProgressDataService>().AsSingle().NonLazy();
        }

        private void BindFactories()
        {
            Container.Bind<StateMachineFactory>().AsSingle().NonLazy();
            Container.BindInterfacesTo<UIElementFactory>().AsSingle().NonLazy();
        }

        private void BindGameStateMachine() =>
            Container.BindInterfacesAndSelfTo<GameStateMachine>().AsSingle().NonLazy();

        private void BindGameBootstrapper() =>
            Container.Bind<ICoroutineRunner>().FromInstance(_gameCoreBootstrapper).AsSingle().NonLazy();

        private void BindSceneLoader()
        {
            Container.Bind<LoadingPanel>().FromInstance(_loadingPanel).AsSingle().NonLazy();
            Container.BindInterfacesTo<SceneLoader>().AsSingle().NonLazy();
        }

        private void BindSoundPlayer()
        {
            Container.Bind<SoundPlayer>().FromInstance(_soundPlayer).AsSingle().NonLazy();
            Container.BindInterfacesTo<SoundService>().AsSingle().NonLazy();
        }
    }
}
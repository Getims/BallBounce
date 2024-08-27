using Main.Scripts.Core.Utilities;
using Main.Scripts.Infrastructure.Providers.Events;
using Main.Scripts.UI.Loadscreen;
using UnityEngine;
using Zenject;

namespace Main.Scripts.Infrastructure.Bootstrap
{
    public class GameLoaderSceneBootstrapper : MonoBehaviour
    {
        [SerializeField]
        private LoadingProgressBarPanel _loadingProgressBarPanel;

        private GlobalEventProvider _globalEventsProvider;

        [Inject]
        public void Construct(GlobalEventProvider gameEventProvider) =>
            _globalEventsProvider = gameEventProvider;

        private void Start()
        {
            Utils.InfoPoint("You can add load screen, login screen, analytics and other logic before load game scene");
            _loadingProgressBarPanel.Fill(SendCompleteEvent);
        }

        private void SendCompleteEvent() =>
            _globalEventsProvider.Invoke<GameLoadCompleteEvent>();
    }
}
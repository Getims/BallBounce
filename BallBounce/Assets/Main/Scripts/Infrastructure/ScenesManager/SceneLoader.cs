using System;
using System.Collections;
using Main.Scripts.Core.Enums;
using Main.Scripts.Infrastructure.Bootstrap;
using Main.Scripts.UI.Loadscreen;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Main.Scripts.Infrastructure.ScenesManager
{
    public class SceneLoader : ISceneLoader
    {
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly LoadingPanel _loadingPanel;

        public SceneLoader(ICoroutineRunner coroutineRunner, LoadingPanel loadingPanel)
        {
            _loadingPanel = loadingPanel;
            _coroutineRunner = coroutineRunner;
        }

        public void Load(string name, Action onLoaded = null) =>
            TryLoadScene(name, onLoaded);

        public void Load(Scenes scene, Action onLoaded = null) =>
            TryLoadScene(ConvertToString(scene), onLoaded);

        private void TryLoadScene(string name, Action onLoaded = null) =>
            _coroutineRunner.StartCoroutine(LoadScene(name, onLoaded));

        private IEnumerator LoadScene(string nextScene, Action onLoaded = null)
        {
            _loadingPanel.Show();
            yield return new WaitForSeconds(0.25f);

            AsyncOperation waitNextScene = SceneManager.LoadSceneAsync(nextScene);

            while (!waitNextScene.isDone)
                yield return null;

            _loadingPanel.Hide();
            yield return new WaitForSeconds(0.25f);
            onLoaded?.Invoke();
        }

        private string ConvertToString(Scenes scene) =>
            scene.ToString();
    }
}
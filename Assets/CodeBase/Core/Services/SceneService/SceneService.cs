using CodeBase.Core.Services.Factory;
using CodeBase.Core.Services.StaticDataService;
using CodeBase.Core.StaticData;
using CodeBase.Common;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Core.Services.SceneService
{
    public class SceneService : ISceneService
    {
        private SceneListStaticData _scenes;
        private LoadingCurtain _loadingCurtain;

        private string _currentScene;

        public async UniTask InitAsync(IStaticDataService staticDataService, IGameFactory gameFactory)
        {
            _loadingCurtain = await gameFactory.CreateLoadingCurtain();
            _scenes = await staticDataService.AllScenes();
        }

        public string CurrentScene => _currentScene;

        public void SetCurrentScene(string sceneName)
        {
            string scene = _scenes.Scenes.Find(s => s == sceneName);

            if (scene != null)
            {
                _currentScene = scene;
            }
            else
            {
                Debug.LogError($"Scene with name '{sceneName}' not found.");
            }
        }

        public async UniTask LoadCurrentSceneAsync(bool forceReload = false)
        {
            if (SceneManager.GetActiveScene().name == CurrentScene && !forceReload)
                return;

            await SceneManager.LoadSceneAsync(CurrentScene);
        }

        public async UniTask ShowCurtain()
        {
            await _loadingCurtain.Show();
        }

        public async UniTask HideCurtain()
        {
            await _loadingCurtain.Hide();
        }
    }
}

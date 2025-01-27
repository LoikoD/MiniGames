using CodeBase.Core.Services.Factory;
using CodeBase.Core.Services.StaticDataService;
using Cysharp.Threading.Tasks;

namespace CodeBase.Core.Services.SceneService
{
    public interface ISceneService : IService
    {
        string CurrentScene { get; }

        UniTask InitAsync(IStaticDataService staticDataService, IGameFactory gameFactory);
        void SetCurrentScene(string sceneName);
        UniTask LoadCurrentSceneAsync(bool forceReload = false);
        UniTask ShowCurtain();
        UniTask HideCurtain();
    }
}
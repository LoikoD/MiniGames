using CodeBase.Core.StaticData;
using Cysharp.Threading.Tasks;

namespace CodeBase.Core.Services.StaticDataService
{
    public interface IStaticDataService : IService
    {
        AssetReferencesStaticData AssetReferences { get; }

        UniTask InitAsync();
        UniTask<SceneListStaticData> AllScenes();
    }
}
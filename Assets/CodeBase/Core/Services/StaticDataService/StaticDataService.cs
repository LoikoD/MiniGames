using CodeBase.Core.Services.RecourceManagement;
using CodeBase.Core.StaticData;
using Cysharp.Threading.Tasks;

namespace CodeBase.Core.Services.StaticDataService
{
    public class StaticDataService : IStaticDataService
    {
        private readonly IResourceLoader _resourceLoader;
        private AssetReferencesStaticData _assetReferences;

        public AssetReferencesStaticData AssetReferences => _assetReferences;

        public StaticDataService(IResourceLoader resourceLoader)
        {
            _resourceLoader = resourceLoader;
        }

        public async UniTask InitAsync()
        {
            _assetReferences = await _resourceLoader.LoadAssetReferences();
        }

        public async UniTask<SceneListStaticData> AllScenes()
        {
            return await _resourceLoader.LoadResourceAsync<SceneListStaticData>(_assetReferences.SceneListData);
        }
    }
}
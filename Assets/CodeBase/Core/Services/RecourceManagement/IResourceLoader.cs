using CodeBase.Core.StaticData;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CodeBase.Core.Services.RecourceManagement
{
    public interface IResourceLoader : IService
    {
        UniTask<AssetReferencesStaticData> LoadAssetReferences();
        UniTask<GameObject> Instantiate(AssetReferenceGameObject assetReferenceGameObject);
        UniTask<T> LoadResourceAsync<T>(AssetReference assetReference) where T : class;
        void ReleaseResource<T>(T resource);
    }
}
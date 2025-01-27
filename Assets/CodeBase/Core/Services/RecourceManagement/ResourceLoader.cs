using CodeBase.Core.StaticData;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CodeBase.Core.Services.RecourceManagement
{
    public class ResourceLoader : IResourceLoader
    {
        public async UniTask<AssetReferencesStaticData> LoadAssetReferences()
        {
            return await LoadResourceAsync<AssetReferencesStaticData>("Assets/Addressables/AssetReferencesData.asset");
        }

        public async UniTask<GameObject> Instantiate(AssetReferenceGameObject assetReferenceGameObject)
        {
            AsyncOperationHandle<GameObject> handle = assetReferenceGameObject.InstantiateAsync();

            await handle.Task;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                return handle.Result;
            }
            else
            {
                Debug.LogError($"Failed to instantiate GameObject: {assetReferenceGameObject}");
                return null;
            }
        }

        public async UniTask<T> LoadResourceAsync<T>(AssetReference assetReference) where T : class
        {
            AsyncOperationHandle<T> handle = assetReference.LoadAssetAsync<T>();

            await handle.Task;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                return handle.Result;
            }
            else
            {
                Debug.LogError($"Failed to load resource: {assetReference.SubObjectName}");
                return null;
            }
        }

        public void ReleaseResource<T>(T resource)
        {
            Addressables.Release(resource);
        }

        private async UniTask<T> LoadResourceAsync<T>(string address) where T : class
        {
            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(address);

            await handle.Task;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                return handle.Result;
            }
            else
            {
                Debug.LogError($"Failed to load resource: {address}");
                return null;
            }
        }
    }
}
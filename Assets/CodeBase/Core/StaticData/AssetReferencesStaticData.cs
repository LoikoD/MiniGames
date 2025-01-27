using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CodeBase.Core.StaticData
{
    [CreateAssetMenu(fileName = "AssetReferencesData", menuName = "StaticData/Addressables")]
    public class AssetReferencesStaticData : ScriptableObject
    {
        #region Static Data

        public AssetReferenceT<SceneListStaticData> SceneListData;
        public AssetReferenceT<CardsStaticData> DeckCardsData;

        #endregion

        public AssetReferenceGameObject LoadingCurtain;

        public AssetReferenceGameObject MainMenuCanvas;

        public AssetReferenceGameObject ClickerMainCanvas;
        public AssetReferenceGameObject ClickerUICanvas;

        public AssetReferenceGameObject TriPeaksMainCanvas;
        public AssetReferenceGameObject TriPeaksStaticCanvas;
        public AssetReferenceGameObject TriPeaksDynamicCanvas;
        public AssetReferenceGameObject TriPeaksEndGameCanvas;
    }
}
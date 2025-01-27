using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Cysharp.Threading.Tasks;
using CodeBase.Clicker;
using CodeBase.TriPeaks;
using CodeBase.Core.Services.RecourceManagement;
using CodeBase.Core.Services.StaticDataService;
using CodeBase.Common;
using CodeBase.Common.Timer;
using CodeBase.Common.Counter;
using CodeBase.TriPeaks.UI;
using CodeBase.TriPeaks.GameFieldViews;
using CodeBase.TriPeaks.CardViews;
using CodeBase.MainMenu;
using CodeBase.Core.StaticData;

namespace CodeBase.Core.Services.Factory
{
    internal class GameFactory : IGameFactory
    {
        private readonly IResourceLoader _resourceLoader;
        private readonly IStaticDataService _staticData;
        private readonly Dictionary<string, object> _loadedResources;

        public GameFactory(IResourceLoader assets, IStaticDataService staticData)
        {
            _resourceLoader = assets;
            _staticData = staticData;
            _loadedResources = new();
        }

        public async UniTask<LoadingCurtain> CreateLoadingCurtain()
        {
            GameObject loadingCurtainGO = await _resourceLoader.Instantiate(_staticData.AssetReferences.LoadingCurtain);
            return loadingCurtainGO.GetComponent<LoadingCurtain>();
        }

        public async UniTask<MenuUI> CreateMainMenuUI()
        {
            GameObject mainMenuCanvas = await _resourceLoader.Instantiate(_staticData.AssetReferences.MainMenuCanvas);
            return mainMenuCanvas.GetComponent<MenuUI>();
        }

        public async UniTask<DynamicUI> CreateClickerMainCanvas()
        {
            GameObject clickerMainCanvas = await _resourceLoader.Instantiate(_staticData.AssetReferences.ClickerMainCanvas);

            Timer timer = clickerMainCanvas.GetComponent<Timer>();

            CountHandler countHandler = new();

            ClickHandler clickCountHandler = clickerMainCanvas.GetComponentInChildren<ClickHandler>();
            clickCountHandler.Construct(countHandler);

            CounterUI counterUI = clickerMainCanvas.GetComponent<CounterUI>();
            counterUI.Construct(countHandler);

            TimerUI timerUI = clickerMainCanvas.GetComponent<TimerUI>();
            timerUI.Construct(timer);

            DynamicUI dynamicUI = clickerMainCanvas.GetComponent<DynamicUI>();
            dynamicUI.Construct(timer, countHandler);

            return dynamicUI;
        }

        public async UniTask<ClickerStaticUI> CreateClickerStaticUI()
        {
            GameObject staticCanvas = await _resourceLoader.Instantiate(_staticData.AssetReferences.ClickerUICanvas);
            return staticCanvas.GetComponent<ClickerStaticUI>();
        }

        public async UniTask<TriPeaksGameController> CreateTriPeaksGame()
        {
            CardsStaticData cardsStaticData = await GetResourceAsync<CardsStaticData>(_staticData.AssetReferences.DeckCardsData);

            GameObject triPeaksMainCanvas = await _resourceLoader.Instantiate(_staticData.AssetReferences.TriPeaksMainCanvas);

            BoardView boardView = triPeaksMainCanvas.GetComponentInChildren<BoardView>();
            boardView.Construct(cardsStaticData.BackSprite);

            StockView stockView = triPeaksMainCanvas.GetComponentInChildren<StockView>();
            stockView.Construct(cardsStaticData.BackSprite);

            WasteCardView wasteCardView = triPeaksMainCanvas.GetComponentInChildren<WasteCardView>();

            CardMover cardMover = triPeaksMainCanvas.GetComponentInChildren<CardMover>();
            cardMover.Construct(cardsStaticData.BackSprite);
            cardMover.gameObject.SetActive(false);

            TriPeaksGameController gameController = new(cardsStaticData.FullDeck.ToList(), boardView, stockView, wasteCardView, cardMover);

            return gameController;
        }

        public async UniTask<DynamicUI> CreateTriPeaksDynamicUI()
        {
            GameObject triPeaksInterfaceCanvas = await _resourceLoader.Instantiate(_staticData.AssetReferences.TriPeaksDynamicCanvas);

            CountHandler turnCountHandler = new();

            CounterUI counterUI = triPeaksInterfaceCanvas.GetComponent<CounterUI>();
            counterUI.Construct(turnCountHandler);

            Timer timer = triPeaksInterfaceCanvas.GetComponent<Timer>();
            TimerUI timerUI = triPeaksInterfaceCanvas.GetComponent<TimerUI>();
            timerUI.Construct(timer);

            DynamicUI triPeaksDynamicUI = triPeaksInterfaceCanvas.GetComponent<DynamicUI>();
            triPeaksDynamicUI.Construct(timer, turnCountHandler);

            return triPeaksDynamicUI;
        }

        public async UniTask<TriPeaksStaticUI> CreateTriPeaksStaticUI()
        {
            GameObject triPeaksStaticCanvas = await _resourceLoader.Instantiate(_staticData.AssetReferences.TriPeaksStaticCanvas);

            return triPeaksStaticCanvas.GetComponent<TriPeaksStaticUI>();
        }

        public async UniTask<EndGameUI> CreateTriPeaksEndGameUI()
        {
            GameObject endGameCanvas = await _resourceLoader.Instantiate(_staticData.AssetReferences.TriPeaksEndGameCanvas);

            return endGameCanvas.GetComponent<EndGameUI>();
        }

        public void ReleaseAllResourcesInScene()
        {
            foreach (var resource in _loadedResources.Values)
            {
                _resourceLoader.ReleaseResource(resource);
            }
            _loadedResources.Clear();
        }

        private async UniTask<T> GetResourceAsync<T>(AssetReference reference) where T : class
        {
            if (_loadedResources.ContainsKey(reference.AssetGUID))
            {
                return _loadedResources[reference.AssetGUID] as T;
            }

            T resource = await _resourceLoader.LoadResourceAsync<T>(reference);
            if (resource != null)
            {
                _loadedResources[reference.AssetGUID] = resource;
            }

            return resource;
        }
    }
}
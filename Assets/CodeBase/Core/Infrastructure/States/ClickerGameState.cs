using CodeBase.Clicker;
using CodeBase.Common;
using CodeBase.Core.Services.Factory;
using CodeBase.Core.Services.GamesSaveData;
using CodeBase.Core.Services.SceneService;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CodeBase.Infrastructure.States
{
    public class ClickerGameState : IState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly ISceneService _sceneService;
        private readonly IGameFactory _gameFactory;
        private readonly IAllGamesSaveData _allGamesData;

        private IClickerSaveData _gameData;
        private IDynamicUI _dynamicUI;
        private IClickerStaticUI _staticUI;

        public ClickerGameState(GameStateMachine gameStateMachine, ISceneService sceneService, IGameFactory gameFactory, IAllGamesSaveData allGamesData)
        {
            _stateMachine = gameStateMachine;
            _sceneService = sceneService;
            _gameFactory = gameFactory;
            _allGamesData = allGamesData;
        }
        public async void Enter()
        {
            await _sceneService.ShowCurtain();

            await _sceneService.LoadCurrentSceneAsync();

            _gameData = _allGamesData.GetGameData<ClickerSaveData>();

            await UniTask.WhenAll(
                InitMainDynamicCanvas(),
                InitUICanvas()
            );

            await _sceneService.HideCurtain();
        }

        private async UniTask InitMainDynamicCanvas()
        {
            _dynamicUI = await _gameFactory.CreateClickerMainCanvas();
            _dynamicUI.Timer.Init(_gameData.GameTime);
            _dynamicUI.Counter.Init(_gameData.ClicksCounter);

            _dynamicUI.Timer.OnTimeUpdated += OnTimeUpdated;
            _dynamicUI.Counter.OnCountChanged += OnClicksCountChanged;
        }

        private async UniTask InitUICanvas()
        {
            _staticUI = await _gameFactory.CreateClickerStaticUI();

            _staticUI.OnMenuClicked += OnMainMenu;
            _staticUI.OnTriPeaksClicked += OnStartTriPeaks;
        }

        private void OnTimeUpdated(double time)
        {
            _gameData.SetGameTime(time);
        }

        private void OnClicksCountChanged(int counter)
        {
            _gameData.SetClicksCounter(counter);
        }

        private void OnMainMenu()
        {
            _sceneService.SetCurrentScene("MainMenu");
            _stateMachine.Enter<MainMenuState>();
        }

        private void OnStartTriPeaks()
        {
            _sceneService.SetCurrentScene("TriPeaks");
            _stateMachine.Enter<TriPeaksGameState>();
        }

        public void Exit()
        {
            _gameFactory.ReleaseAllResourcesInScene();

            _dynamicUI.Timer.OnTimeUpdated -= OnTimeUpdated;
            _dynamicUI.Counter.OnCountChanged -= OnClicksCountChanged;

            _staticUI.OnMenuClicked -= OnMainMenu;
            _staticUI.OnTriPeaksClicked -= OnStartTriPeaks;
        }
    }
}

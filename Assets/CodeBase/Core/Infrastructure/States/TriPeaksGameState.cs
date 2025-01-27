using CodeBase.Common;
using CodeBase.Core.Services.Factory;
using CodeBase.Core.Services.GamesSaveData;
using CodeBase.Core.Services.SceneService;
using CodeBase.TriPeaks;
using CodeBase.TriPeaks.UI;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CodeBase.Infrastructure.States
{
    public class TriPeaksGameState : IState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly ISceneService _sceneService;
        private readonly IGameFactory _gameFactory;
        private readonly IAllGamesSaveData _allGamesData;

        private ITriPeaksSaveData _gameData;
        private ITriPeaksGameController _gameController;
        private ITriPeaksStaticUI _staticUI;
        private IDynamicUI _dynamicUI;
        private IEndGameUI _endGameUI;

        public TriPeaksGameState(GameStateMachine gameStateMachine, ISceneService sceneService, IGameFactory gameFactory, IAllGamesSaveData allGamesData)
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

            await UniTask.WhenAll(
                InitTriPeaksGameController(),
                InitStaticUI(),
                InitDynamicUI(),
                InitEndGameUI()
            );

            _gameData = _allGamesData.GetGameData<TriPeaksSaveData>();
            CreateGame();

            await _sceneService.HideCurtain();
        }

        private void CreateGame()
        {
            _gameController.LoadGame(_gameData);

            _dynamicUI.Timer.Init(_gameData.GameTime);
            _dynamicUI.Counter.Init(_gameData.Turns.Count);

            _staticUI.SetUndoActive(_gameData.Turns.Count > 0);
        }

        private void CreateNewGame()
        {
            _gameData.Clear();
            CreateGame();
        }

        private async UniTask InitTriPeaksGameController()
        {
            _gameController = await _gameFactory.CreateTriPeaksGame();

            _gameController.OnTurn += OnTurn;
            _gameController.OnVictory += OnVictory;
            _gameController.OnDefeat += OnDefeat;
        }

        private async UniTask InitStaticUI()
        {
            _staticUI = await _gameFactory.CreateTriPeaksStaticUI();

            _staticUI.MenuClicked += OnMainMenu;
            _staticUI.PlayClickerClicked += OnStartClicker;
            _staticUI.UndoClicked += OnUndo;
        }

        private async UniTask InitDynamicUI()
        {
            _dynamicUI = await _gameFactory.CreateTriPeaksDynamicUI();

            _dynamicUI.Timer.OnTimeUpdated += OnTimeUpdated;
        }

        private async UniTask InitEndGameUI()
        {
            _endGameUI = await _gameFactory.CreateTriPeaksEndGameUI();

            _endGameUI.NewGameClicked += OnNewGame;

            _endGameUI.Hide();
        }

        private void OnTurn()
        {
            _dynamicUI.Counter.Add();
            _staticUI.SetUndoActive(true);
        }

        private void OnVictory()
        {
            _dynamicUI.Timer.Stop();
            _endGameUI.ShowWin();
        }

        private void OnDefeat()
        {
            _dynamicUI.Timer.Stop();
            _endGameUI.ShowLose();
        }

        private void OnNewGame()
        {
            CreateNewGame();
        }

        private void OnUndo()
        {
            _gameController.UndoTurn();
            _dynamicUI.Counter.Subtract();

            if (_dynamicUI.Counter.GetCount() == 0)
            {
                _staticUI.SetUndoActive(false);
            }
        }

        private void OnMainMenu()
        {
            _sceneService.SetCurrentScene("MainMenu");
            _stateMachine.Enter<MainMenuState>();
        }

        private void OnStartClicker()
        {
            _sceneService.SetCurrentScene("Clicker");
            _stateMachine.Enter<ClickerGameState>();
        }

        private void OnTimeUpdated(double time)
        {
            _gameData.SetGameTime(time);
        }

        public void Exit()
        {
            _gameFactory.ReleaseAllResourcesInScene();

            _staticUI.MenuClicked -= OnMainMenu;
            _staticUI.PlayClickerClicked -= OnStartClicker;
            _staticUI.UndoClicked -= OnUndo;

            _dynamicUI.Timer.OnTimeUpdated -= OnTimeUpdated;

            _gameController.OnTurn -= OnTurn;
            _gameController.OnVictory -= OnVictory;
            _gameController.OnDefeat -= OnDefeat;

            _endGameUI.NewGameClicked -= OnNewGame;
        }
    }
}

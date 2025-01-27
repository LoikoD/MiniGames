using CodeBase.Core.Services.Factory;
using CodeBase.Core.Services.SceneService;
using CodeBase.MainMenu;
using UnityEngine;

namespace CodeBase.Infrastructure.States
{
    public class MainMenuState : IState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly ISceneService _sceneService;
        private readonly IGameFactory _gameFactory;

        private IMenuUI _menuUI;

        public MainMenuState(GameStateMachine gameStateMachine, ISceneService sceneService, IGameFactory gameFactory)
        {
            _stateMachine = gameStateMachine;
            _sceneService = sceneService;
            _gameFactory = gameFactory;
        }

        public async void Enter()
        {
            await _sceneService.ShowCurtain();

            await _sceneService.LoadCurrentSceneAsync();

            _menuUI = await _gameFactory.CreateMainMenuUI();
            _menuUI.StartClicker += OnStartClicker;
            _menuUI.StartTriPeaks += OnStartTriPeaks;

            await _sceneService.HideCurtain();
        }

        private void OnStartClicker()
        {
            _sceneService.SetCurrentScene("Clicker");
            _stateMachine.Enter<ClickerGameState>();
        }

        private void OnStartTriPeaks()
        {
            _sceneService.SetCurrentScene("TriPeaks");
            _stateMachine.Enter<TriPeaksGameState>();
        }

        public void Exit()
        {
            _menuUI.StartClicker -= OnStartClicker;
            _menuUI.StartTriPeaks -= OnStartTriPeaks;
        }
    }
}

using System;
using System.Collections.Generic;
using CodeBase.Core.Services;
using CodeBase.Core.Services.Factory;
using CodeBase.Core.Services.GamesSaveData;
using CodeBase.Core.Services.SceneService;
using CodeBase.Infrastructure.States;
using Cysharp.Threading.Tasks;

namespace CodeBase.Infrastructure
{
    public class GameStateMachine
    {
        private Dictionary<Type, IExitableState> _states;
        private IExitableState _activeState;

        public async UniTask InitAsync(AllServices services)
        {
            BootstrapState bootStrapState = new(this, services);
            await bootStrapState.InitAsync();

            ISceneService sceneService = services.Single<ISceneService>();
            IGameFactory gameFactory = services.Single<IGameFactory>();
            IAllGamesSaveData allGamesSaveData = services.Single<IAllGamesSaveData>();

            _states = new Dictionary<Type, IExitableState>
            {
                [typeof(BootstrapState)] = bootStrapState,  
                [typeof(MainMenuState)] = new MainMenuState(this, sceneService, gameFactory),
                [typeof(ClickerGameState)] = new ClickerGameState(this, sceneService, gameFactory, allGamesSaveData),
                [typeof(TriPeaksGameState)] = new TriPeaksGameState(this, sceneService, gameFactory, allGamesSaveData)
            };
        }

        public void Enter<TState>() where TState : class, IState
        {
            IState state = ChangeState<TState>();
            state.Enter();
        }

        private TState ChangeState<TState>() where TState : class, IExitableState
        {
            _activeState?.Exit();
            
            TState state = GetState<TState>();
            _activeState = state;
            
            return state;
        }

        private TState GetState<TState>() where TState : class, IExitableState => 
            _states[typeof(TState)] as TState;
    }
}
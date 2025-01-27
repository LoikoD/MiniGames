using CodeBase.Core.Services.GamesSaveData;
using CodeBase.Core.Services.SaveService;
using CodeBase.Core.Services;
using CodeBase.Infrastructure.States;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace CodeBase.Infrastructure
{
    public class GameBootstrapper : MonoBehaviour
    {
        private Game _game;

        private async void Awake()
        {
            _game = new Game();
            await _game.InitAsync();
            _game.StateMachine.Enter<BootstrapState>();
            DontDestroyOnLoad(this);
        }

        private async void OnApplicationQuit()
        {
            await SaveData();
        }

        private async UniTask SaveData()
        {
            AllServices services = AllServices.Container;
            await services.Single<ISaveService>().Save(services.Single<IAllGamesSaveData>());
        }
    }
}
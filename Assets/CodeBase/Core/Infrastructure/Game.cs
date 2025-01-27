using CodeBase.Core.Services;
using Cysharp.Threading.Tasks;

namespace CodeBase.Infrastructure
{
    public class Game
    {
        public GameStateMachine StateMachine { get; private set; }

        public Game()
        {
            StateMachine = new GameStateMachine();
        }

        public async UniTask InitAsync()
        {
            await StateMachine.InitAsync(AllServices.Container);
        }
    }
}
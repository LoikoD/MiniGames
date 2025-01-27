using CodeBase.Core.Services;
using CodeBase.Core.Services.Factory;
using CodeBase.Core.Services.GamesSaveData;
using CodeBase.Core.Services.RecourceManagement;
using CodeBase.Core.Services.SaveService;
using CodeBase.Core.Services.SceneService;
using CodeBase.Core.Services.StaticDataService;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure.States
{
    public class BootstrapState : IState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly AllServices _services;

        public BootstrapState(GameStateMachine stateMachine, AllServices allServices)
        {
            _stateMachine = stateMachine;
            _services = allServices;
        }

        public async UniTask InitAsync()
        {
            await RegisterServices();
        }

        public void Enter()
        {
            InitializeDOTween();

            string currentSceneName = SceneManager.GetActiveScene().name;

            ISceneService sceneService = _services.Single<ISceneService>();
            sceneService.SetCurrentScene(currentSceneName);

            switch (sceneService.CurrentScene)
            {
                case "MainMenu":
                    _stateMachine.Enter<MainMenuState>();
                    break;
                case "Clicker":
                    _stateMachine.Enter<ClickerGameState>();
                    break;
                case "TriPeaks":
                    _stateMachine.Enter<TriPeaksGameState>();
                    break;
                default:
                    throw new Exception("Unknown scene name.");
            }
        }

        public void Exit()
        {

        }

        private void InitializeDOTween()
        {
            DOTween.Init(
                recycleAllByDefault: true,
                useSafeMode: true,
                logBehaviour: LogBehaviour.ErrorsOnly
            );

            // Warmup DOTween Animations
            var dummyRect = new GameObject("DOTweenWarmup").AddComponent<RectTransform>();
            dummyRect.DOMoveY(0, 0.01f).SetEase(Ease.OutBounce).Kill();
            dummyRect.DOMoveY(0, 0.01f).SetEase(Ease.InQuad).Kill();
            dummyRect.DOMove(Vector3.zero, 0.01f).Kill();
            dummyRect.DORotate(Vector3.zero, 0.01f).Kill();
            DOTween.Sequence().Kill();
            UnityEngine.Object.Destroy(dummyRect.gameObject);
        }

        private async UniTask RegisterServices()
        {
            _services.RegisterSingle<IResourceLoader>(new ResourceLoader());

            await RegisterStaticData();

            

            _services.RegisterSingle<IGameFactory>(new GameFactory(
                _services.Single<IResourceLoader>(),
                _services.Single<IStaticDataService>()));


            await RegisterSceneService();

            _services.RegisterSingle<ISaveService>(new SaveService());

            ISaveService saveService = _services.Single<ISaveService>();

            IAllGamesSaveData allGamesSaveData = await saveService.Load<AllGamesSaveData>();
            _services.RegisterSingle<IAllGamesSaveData>(allGamesSaveData);
        }

        private async UniTask RegisterStaticData()
        {
            IStaticDataService staticData = new StaticDataService(_services.Single<IResourceLoader>());
            await staticData.InitAsync();
            _services.RegisterSingle<IStaticDataService>(staticData);
        }

        private async UniTask RegisterSceneService()
        {
            ISceneService sceneService = new SceneService();
            await sceneService.InitAsync(_services.Single<IStaticDataService>(), _services.Single<IGameFactory>());
            _services.RegisterSingle<ISceneService>(sceneService);
        }
    }
}
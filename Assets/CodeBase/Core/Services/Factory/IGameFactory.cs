using CodeBase.Clicker;
using CodeBase.Common;
using CodeBase.MainMenu;
using CodeBase.TriPeaks;
using CodeBase.TriPeaks.UI;
using Cysharp.Threading.Tasks;

namespace CodeBase.Core.Services.Factory
{
    public interface IGameFactory : IService
    {
        UniTask<LoadingCurtain> CreateLoadingCurtain();
        UniTask<MenuUI> CreateMainMenuUI();
        UniTask<DynamicUI> CreateClickerMainCanvas();
        UniTask<ClickerStaticUI> CreateClickerStaticUI();
        UniTask<TriPeaksGameController> CreateTriPeaksGame();
        UniTask<DynamicUI> CreateTriPeaksDynamicUI();
        UniTask<TriPeaksStaticUI> CreateTriPeaksStaticUI();
        UniTask<EndGameUI> CreateTriPeaksEndGameUI();
        void ReleaseAllResourcesInScene();
    }
}
using Cysharp.Threading.Tasks;

namespace CodeBase.Common
{
    public interface ILoadingCurtain
    {
        UniTask Hide();
        UniTask Show();
    }
}
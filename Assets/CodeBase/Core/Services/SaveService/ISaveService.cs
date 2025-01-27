using Cysharp.Threading.Tasks;

namespace CodeBase.Core.Services.SaveService
{
    public interface ISaveService : IService
    {
        UniTask Save<T>(T data);
        UniTask<T> Load<T>() where T : new();
    }
}
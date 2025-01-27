namespace CodeBase.Core.Services.GamesSaveData
{
    public interface IAllGamesSaveData : IService
    {
        TData GetGameData<TData>() where TData : class, ISaveData, new();
    }
}
using CodeBase.Common.Counter;

namespace CodeBase.Clicker
{
    public interface IClickHandler
    {
        void Construct(ICountHandler countHandler);
    }
}
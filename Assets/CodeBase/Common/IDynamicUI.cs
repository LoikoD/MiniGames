using CodeBase.Common.Counter;
using CodeBase.Common.Timer;

namespace CodeBase.Common
{
    public interface IDynamicUI
    {
        ICountHandler Counter { get; }
        ITimer Timer { get; }

        void Construct(ITimer timer, ICountHandler counter);
    }
}
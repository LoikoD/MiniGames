using CodeBase.Common.Counter;
using CodeBase.Common.Timer;
using UnityEngine;

namespace CodeBase.Common
{
    public class DynamicUI : MonoBehaviour, IDynamicUI
    {
        private ITimer _timer;
        private ICountHandler _counter;

        public ITimer Timer => _timer;
        public ICountHandler Counter => _counter;

        public void Construct(ITimer timer, ICountHandler counter)
        {
            _timer = timer;
            _counter = counter;
        }
    }
}

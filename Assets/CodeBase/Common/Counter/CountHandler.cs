using System;

namespace CodeBase.Common.Counter
{
    public class CountHandler : ICountHandler
    {
        private int _counter = 0;

        public event Action<int> OnCountChanged;

        public void Init(int counter = 0)
        {
            _counter = counter;
            OnCountChanged?.Invoke(_counter);
        }

        public void Add(int count = 1)
        {
            _counter += count;
            OnCountChanged?.Invoke(_counter);
        }

        public void Subtract(int count = 1)
        {
            _counter -= count;
            OnCountChanged?.Invoke(_counter);
        }

        public int GetCount()
        {
            return _counter;
        }
    }
}

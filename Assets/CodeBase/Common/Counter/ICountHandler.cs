using System;

namespace CodeBase.Common.Counter
{
    public interface ICountHandler
    {
        event Action<int> OnCountChanged;

        void Init(int counter = 0);
        void Add(int count = 1);
        void Subtract(int count = 1);
        int GetCount();
    }
}
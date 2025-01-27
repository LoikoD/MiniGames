using CodeBase.Core.Services.GamesSaveData;
using CodeBase.TriPeaks.Cards;
using System.Collections.Generic;

namespace CodeBase.TriPeaks
{
    public interface ITriPeaksSaveData : ISaveData
    {
        double GameTime { get; }
        Stack<Card> StartStack { get; }
        Stack<Turn> Turns { get; }

        void Clear();
        void SetGameTime(double time);
        void SetStartStack(Stack<Card> startStack);
    }
}
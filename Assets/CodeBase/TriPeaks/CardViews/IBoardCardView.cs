using System;

namespace CodeBase.TriPeaks.CardViews
{
    public interface IBoardCardView : ICardView
    {
        bool IsOpen { get; }

        event Action<IBoardCardView> BoardClicked;

        void AddBlocker(IBoardCardView card);
    }
}
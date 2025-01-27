using System;

namespace CodeBase.TriPeaks.CardViews
{
    public interface IStockCardView : ICardView
    {
        event Action<IStockCardView> StockClicked;
    }
}
using System;
using UnityEngine.EventSystems;

namespace CodeBase.TriPeaks.CardViews
{
    public class StockCardView : CardView, IStockCardView, IPointerClickHandler
    {
        public event Action<IStockCardView> StockClicked;

        public void OnPointerClick(PointerEventData eventData)
        {
            StockClicked?.Invoke(this);
        }
    }
}
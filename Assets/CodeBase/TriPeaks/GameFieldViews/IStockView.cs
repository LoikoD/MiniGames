using CodeBase.TriPeaks.Cards;
using CodeBase.TriPeaks.CardViews;
using System;
using UnityEngine;

namespace CodeBase.TriPeaks.GameFieldViews
{
    public interface IStockView
    {
        event Action<IStockCardView> StockCardClicked;

        void Construct(Sprite backSprite);
        void InitStartState(CardData topCard);
        void OnStockCardClicked(IStockCardView stockCardView);
        void SetEmpty();
        void SetTopCard(CardData cardData);
    }
}
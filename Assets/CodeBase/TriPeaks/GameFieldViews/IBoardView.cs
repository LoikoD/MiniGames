using CodeBase.TriPeaks.Cards;
using CodeBase.TriPeaks.CardViews;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.TriPeaks.GameFieldViews
{
    public interface IBoardView
    {
        event Action<IBoardCardView> BoardCardClicked;

        void Construct(Sprite backSprite);
        void InitStartState(List<CardData> boardCards);
        void DisableBoardCardView(IBoardCardView cardView);
        void DisableViewByCard(Card card);
        bool HasAdjacentOpenCardOnBoard(Card card);
        void ResetCard(Card card);
    }
}
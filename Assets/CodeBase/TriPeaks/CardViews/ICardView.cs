using CodeBase.TriPeaks.Cards;
using UnityEngine;

namespace CodeBase.TriPeaks.CardViews
{
    public interface ICardView
    {
        CardData CardData { get; }
        bool IsActive { get; }
        bool IsFaceUp { get; }
        Vector3 Position { get; }

        void SetCardData(CardData cardData, bool isFaceUp);
        void SetDefSprite();
        void SetSprite(Sprite sprite);
        void SetFaceUp(bool isFaceUp);
        void SetImageActive(bool isActive);
        void FlipUp();
    }
}
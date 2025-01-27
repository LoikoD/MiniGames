using CodeBase.TriPeaks.Cards;
using UnityEngine;

namespace CodeBase.Core.StaticData
{
    [CreateAssetMenu(fileName = "CardDeck", menuName = "StaticData/CardDeck")]
    public class CardsStaticData : ScriptableObject
    {
        public CardData[] FullDeck;
        public Sprite BackSprite;
    }
}
using System;
using UnityEngine;

namespace CodeBase.TriPeaks.Cards
{
    [Serializable]
    public class CardData
    {
        [SerializeField] private Card _card;
        [SerializeField] private Sprite _sprite;

        public Card Card => _card;
        public Sprite Sprite => _sprite;
    }
}
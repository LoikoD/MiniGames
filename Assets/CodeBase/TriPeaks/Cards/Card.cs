using Newtonsoft.Json;
using System;
using UnityEngine;

namespace CodeBase.TriPeaks.Cards
{
    [Serializable]
    public class Card
    {
        [SerializeField, JsonProperty] private CardValue _value;
        [SerializeField, JsonProperty] private CardSuit _suit;

        public CardValue Value => _value;
        public CardSuit Suit => _suit;

        public bool AreCardValuesAdjacent(Card card)
        {
            int diff = Mathf.Abs(Value - card.Value);
            return diff == 1 || diff == 12;
        }
        public bool Equals(Card card)
        {
            return _value == card.Value && _suit == card.Suit;
        }
    }
}
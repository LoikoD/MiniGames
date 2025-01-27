using CodeBase.TriPeaks.Cards;
using Newtonsoft.Json;

namespace CodeBase.TriPeaks
{
    public class Turn
    {
        [JsonProperty]
        public TurnType Type { get; private set; }

        [JsonProperty]
        public Card Card { get; private set; }

        public Turn(TurnType type, Card card)
        {
            Type = type;
            Card = card;
        }
    }
}

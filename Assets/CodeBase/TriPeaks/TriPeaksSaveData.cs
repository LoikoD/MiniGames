using CodeBase.Core.Services.GamesSaveData;
using CodeBase.TriPeaks.Cards;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CodeBase.TriPeaks
{
    public class TriPeaksSaveData : ITriPeaksSaveData
    {
        [JsonProperty]
        public double GameTime { get; private set; }

        [JsonProperty]
        public Stack<Card> StartStack { get; private set; }

        [JsonProperty]
        public Stack<Turn> Turns { get; private set; }

        public TriPeaksSaveData()
        {
            GameTime = 0;
            StartStack = new();
            Turns = new();
        }

        public void SetStartStack(Stack<Card> startStack)
        {
            StartStack = startStack;
        }

        public void SetGameTime(double time)
        {
            GameTime = time;
        }

        public void Clear()
        {
            GameTime = 0;
            StartStack.Clear();
            Turns.Clear();
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            // Restore correct order of the stacks
            StartStack = new(StartStack);
            Turns = new(Turns);
        }
    }
}

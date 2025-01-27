using CodeBase.Core.Services.GamesSaveData;
using Newtonsoft.Json;

namespace CodeBase.Clicker
{
    public class ClickerSaveData : ISaveData, IClickerSaveData
    {
        [JsonProperty]
        public int ClicksCounter { get; private set; }

        [JsonProperty]
        public double GameTime { get; private set; }

        public void SetClicksCounter(int counter)
        {
            ClicksCounter = counter;
        }
        public void SetGameTime(double time)
        {
            GameTime = time;
        }
    }
}

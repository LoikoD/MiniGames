using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace CodeBase.Core.Services.GamesSaveData
{
    public class AllGamesSaveData : IAllGamesSaveData
    {
        [JsonProperty(ItemTypeNameHandling = TypeNameHandling.All)]
        public Dictionary<Type, ISaveData> GameData { get; private set; }

        public AllGamesSaveData()
        {
            GameData = new();
        }

        public TData GetGameData<TData>() where TData : class, ISaveData, new()
        {
            if (GameData.TryGetValue(typeof(TData), out var data))
            {
                return data as TData;
            }
            else
            {
                TData newData = new();
                GameData[typeof(TData)] = newData;
                return newData;
            }
        }
    }
}

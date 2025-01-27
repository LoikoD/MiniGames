using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Core.StaticData
{
    [CreateAssetMenu(fileName = "SceneList", menuName = "StaticData/SceneList")]
    public class SceneListStaticData : ScriptableObject
    {
        public List<string> Scenes;
    }
}
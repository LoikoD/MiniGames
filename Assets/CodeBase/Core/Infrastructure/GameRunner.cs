using UnityEngine;

namespace CodeBase.Infrastructure
{
    public class GameRunner : MonoBehaviour
    {
        [SerializeField] private GameBootstrapper _bootstrapperPrefab;
        
        private void Awake()
        {
            var bootstrapper = FindFirstObjectByType<GameBootstrapper>();
            if (bootstrapper == null) 
                Instantiate(_bootstrapperPrefab);
        }
    }
}
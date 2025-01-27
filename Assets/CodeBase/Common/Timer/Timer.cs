using System;
using UnityEngine;

namespace CodeBase.Common.Timer
{
    public class Timer : MonoBehaviour, ITimer
    {
        private double _currentTime;
        private bool _isPlaying;

        public event Action<double> OnTimeUpdated;

        public void Init(double currentTime = 0)
        {
            _currentTime = currentTime;
            OnTimeUpdated?.Invoke(_currentTime);
            _isPlaying = true;
        }

        private void Update()
        {
            if (_isPlaying)
            {
                _currentTime += Time.deltaTime;
                OnTimeUpdated?.Invoke(_currentTime);
            }    
        }

        public void Stop()
        {
            _isPlaying = false;
        }
    }
}
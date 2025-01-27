using System;
using TMPro;
using UnityEngine;

namespace CodeBase.Common.Timer
{
    public class TimerUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _timerText;

        private ITimer _timer;

        public void Construct(ITimer timer)
        {
            _timer = timer;
            _timer.OnTimeUpdated += UpdateTimerText;
        }

        private void UpdateTimerText(double time)
        {
            string textTime = ConvertTimeToString(time);
            if (textTime != _timerText.text)
            {
                _timerText.text = textTime;
            }
        }

        private string ConvertTimeToString(double time)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(time);

            if ((int)timeSpan.TotalHours > 0)
            {
                return timeSpan.ToString(@"hh\:mm\:ss");
            }
            else
            {
                return timeSpan.ToString(@"mm\:ss");
            }
        }

        private void OnDestroy()
        {
            _timer.OnTimeUpdated -= UpdateTimerText;
        }
    }
}
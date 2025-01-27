using TMPro;
using UnityEngine;

namespace CodeBase.Common.Counter
{
    public class CounterUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _counterText;
        [SerializeField] private string _additionalString;

        private ICountHandler _countHandler;

        public void Construct(ICountHandler countHandler)
        {
            _countHandler = countHandler;
            _countHandler.OnCountChanged += UpdateCounterText;
        }

        private void UpdateCounterText(int newCounter)
        {
            if (string.IsNullOrEmpty(_additionalString))
                _counterText.text = newCounter.ToString();
            else
                _counterText.text = $"{newCounter} {_additionalString}";
        }

        private void OnDestroy()
        {
            _countHandler.OnCountChanged -= UpdateCounterText;
        }
    }
}
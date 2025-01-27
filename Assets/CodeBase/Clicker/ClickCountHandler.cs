using CodeBase.Common.Counter;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CodeBase.Clicker
{
    public class ClickHandler : MonoBehaviour, IPointerClickHandler, IClickHandler
    {
        [SerializeField] private Image _image;

        private ICountHandler _countHandler;

        private void Awake()
        {
            _image.alphaHitTestMinimumThreshold = 0.1f;
        }

        public void Construct(ICountHandler countHandler)
        {
            _countHandler = countHandler;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _countHandler.Add(1);
        }
    }
}
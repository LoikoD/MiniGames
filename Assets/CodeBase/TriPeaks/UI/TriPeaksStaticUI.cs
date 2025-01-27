using System;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.TriPeaks.UI
{
    public class TriPeaksStaticUI : MonoBehaviour, ITriPeaksStaticUI
    {
        [SerializeField] private Button _undoBtn;

        public event Action MenuClicked;
        public event Action PlayClickerClicked;
        public event Action UndoClicked;

        public void ClickMenu()
        {
            MenuClicked?.Invoke();
        }

        public void ClickPlayClicker()
        {
            PlayClickerClicked?.Invoke();
        }

        public void ClickUndo()
        {
            UndoClicked?.Invoke();
        }

        public void SetUndoActive(bool isActive)
        {
            if (_undoBtn.interactable != isActive)
                _undoBtn.interactable = isActive;
        }
    }
}

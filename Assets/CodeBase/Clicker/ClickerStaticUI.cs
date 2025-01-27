using System;
using UnityEngine;

namespace CodeBase.Clicker
{
    public class ClickerStaticUI : MonoBehaviour, IClickerStaticUI
    {
        public event Action OnMenuClicked;
        public event Action OnTriPeaksClicked;

        public void ClickMenu()
        {
            OnMenuClicked?.Invoke();
        }
        public void ClickPlayTriPeaks()
        {
            OnTriPeaksClicked?.Invoke();
        }
    }
}
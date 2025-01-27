using System;

namespace CodeBase.TriPeaks.UI
{
    public interface ITriPeaksStaticUI
    {
        event Action MenuClicked;
        event Action PlayClickerClicked;
        event Action UndoClicked;

        void ClickMenu();
        void ClickPlayClicker();
        void ClickUndo();
        void SetUndoActive(bool isActive);
    }
}
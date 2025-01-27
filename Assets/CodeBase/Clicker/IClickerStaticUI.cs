using System;

namespace CodeBase.Clicker
{
    public interface IClickerStaticUI
    {
        event Action OnMenuClicked;
        event Action OnTriPeaksClicked;
    }
}
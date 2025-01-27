using System;

namespace CodeBase.MainMenu
{
    public interface IMenuUI
    {
        event Action StartClicker;
        event Action StartTriPeaks;
    }
}
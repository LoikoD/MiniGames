using System;

namespace CodeBase.TriPeaks.UI
{
    public interface IEndGameUI
    {
        event Action NewGameClicked;

        void Hide();
        void ShowLose();
        void ShowWin();
        void NewGame();
    }
}
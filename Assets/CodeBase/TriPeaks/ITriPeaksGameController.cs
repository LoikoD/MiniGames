using System;

namespace CodeBase.TriPeaks
{
    public interface ITriPeaksGameController
    {
        event Action OnDefeat;
        event Action OnTurn;
        event Action OnVictory;

        void LoadGame(ITriPeaksSaveData gameData);
        void SetupGame(bool newGame);
        void UndoTurn();
    }
}
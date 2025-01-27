namespace CodeBase.Clicker
{
    public interface IClickerSaveData
    {
        int ClicksCounter { get; }
        double GameTime { get; }

        void SetClicksCounter(int counter);
        void SetGameTime(double time);
    }
}
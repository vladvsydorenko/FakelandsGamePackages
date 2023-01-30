namespace Xyz.Vasd.FakeGame.Core
{
    public interface IView
    {
        public enum Status
        {
            Closed,
            Opening,
            Open,
            Closing,
        }

        Status GetViewStatus();
        void OpenView();
        void CloseView();
    }

    public interface ILoaderView : IView
    {
        public bool IsLoaded();
    }
}

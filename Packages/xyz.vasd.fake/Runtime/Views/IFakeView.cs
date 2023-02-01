namespace Xyz.Vasd.Fake
{
    public interface IFakeView
    {
        FakeViewStatus GetViewStatus();
        bool OpenView();
        bool CloseView();
    }
}

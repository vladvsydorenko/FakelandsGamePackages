namespace Xyz.Vasd.Fake.Views
{
    public interface IFakeView
    {
        FakeViewStatus GetViewStatus();
        bool OpenView();
        bool CloseView();
    }
}

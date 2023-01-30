using System.Collections.Generic;

namespace Xyz.Vasd.FakeGame.Core
{
    public class index { }

    public interface IView
    {
        public enum ViewStage
        {
            Closed,
            Opening,
            Open,
            Closing,
        }

        ViewStage Stage { get; }
        void Open();
        void Close();
    }

    public interface IPage : IView
    {
        public string Id { get; }
    }

    public class PageManager
    {
        public List<IPage> Pages;
        public Dictionary<string, IPage> PagesMap;
        public IPage CurrentPage;

        public void OpenPage(string id)
        {
            var page = PagesMap[id];
            page.Open();
        }
    }
}

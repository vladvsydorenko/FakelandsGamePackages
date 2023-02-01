using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Xyz.Vasd.Fake
{
    public interface IFakeView
    {
        bool OpenView();
        bool CloseView();
    }

    public class FakeView : MonoBehaviour, IFakeView
    {
        bool IFakeView.CloseView()
        {
            gameObject.SetActive(false);
            return true;
        }

        bool IFakeView.OpenView()
        {
            gameObject.SetActive(true);
            return true;
        }
    }

    public class AnimatedFakeView : FakeView, IFakeView
    {
        public PlayableAsset OpenTimeline;
        public PlayableAsset IdleTimeline;
        public PlayableAsset CloseTimeline;

        public PlayableDirector Director;

        bool IFakeView.CloseView()
        {
            if (Director == null || CloseTimeline == null)
            {
                gameObject.SetActive(false);
                return true;
            };

            Director.playableAsset = OpenTimeline;
            Director.time = 0f;
            Director.RebuildGraph();
            Director.Play();

            var visibility = Director.time >= Director.duration;
            gameObject.SetActive(visibility);
            return visibility;
        }

        bool IFakeView.OpenView()
        {
            gameObject.SetActive(true);

            if (Director == null || OpenTimeline == null) return true;

            Director.playableAsset = OpenTimeline;
            Director.time = 0f;
            Director.RebuildGraph();
            Director.Play();

            return Director.time >= Director.duration;
        }
    }

    public class FakeViewSwitch : MonoBehaviour
    {
        private IFakeView _current;
        private IFakeView _previous;

        public void OpenView(IFakeView view)
        {
            if (_current == view) return;

            _previous = _current;
            _current = view;
        }

        private void Update()
        {
            if (_current != null || !_current.OpenView()) return;
            if (_previous != null) _previous.CloseView();
        }
    }
}

using UnityEngine;

namespace Xyz.Vasd.Fake
{
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
            if (_current != null && !_current.OpenView()) return;
            if (_previous != null) _previous.CloseView();
        }
    }
}

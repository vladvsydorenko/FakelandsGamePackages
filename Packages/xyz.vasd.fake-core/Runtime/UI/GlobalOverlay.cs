using UnityEngine;

namespace Xyz.Vasd.FakeCore.UI
{
    public class GlobalOverlay : MonoBehaviour
    {
        public PhotoCanvas Canvas;

        [Header("Settings")]
        public RectTransform Root;
        public Vector2 UpChange = new(0, 1);
        public Vector2 DefaultAnchorsMin = new(0, 0);
        public Vector2 DefaultAnchorsMax = new(1, 1);

        public Vector2 TargetAnchorsMin;
        public Vector2 TargetAnchorsMax;

        public Vector2 TopAnchorsMin;
        public Vector2 TopAnchorsMax;

        [SerializeField] private float _timeLoopDuration;
        private float _timeLoop;
        private bool _isShown;

        private void Start()
        {
            UpdateTopAnchors();
            MoveTop();
            _isShown = false;
        }

        [ContextMenu(nameof(Show) + "()")]
        public void Show()
        {
            UpdateTopAnchors();

            TargetAnchorsMin = DefaultAnchorsMin;
            TargetAnchorsMax = DefaultAnchorsMax;

            _isShown = true;
        }

        [ContextMenu(nameof(Hide) + "()")]
        public void Hide()
        {
            UpdateTopAnchors();

            TargetAnchorsMin = TopAnchorsMin;
            TargetAnchorsMax = TopAnchorsMax;

            _isShown = false;
        }

        private void Update()
        {
            //TargetAnchorsMin = DefaultAnchorsMin;
            //TargetAnchorsMax = DefaultAnchorsMax;

            var duration = _timeLoopDuration;
            if (!_isShown) duration *= 0.1f;
            if (_timeLoop > duration)
            {
                if (_isShown) Hide();
                else Show();

                _timeLoop = 0f;
            }
            _timeLoop += Time.deltaTime;

            var min = Root.anchorMin;
            var max = Root.anchorMax;

            min = Vector2.Lerp(min, TargetAnchorsMin, Time.deltaTime * (TargetAnchorsMin.y > 0f ? 1f : 1.5f));
            max = Vector2.Lerp(max, TargetAnchorsMax, Time.deltaTime * (TargetAnchorsMin.y > 0f ? 1.5f : 1f));

            Root.anchorMin = min;
            Root.anchorMax = max;
        }


        private void UpdateTopAnchors()
        {
            var min = DefaultAnchorsMin;
            var max = DefaultAnchorsMax;

            var upChange = UpChange;
            var scale = Canvas.CanvasScale;

            min += upChange * scale;
            max += upChange * scale;

            var diff = (min.y - 1f) * 0.5f;
            min.y -= diff;
            max.y -= diff;

            TopAnchorsMin = min;
            TopAnchorsMax = max;
        }

        [ContextMenu(nameof(MoveTop) + "()")]
        public void MoveTop()
        {
            var min = DefaultAnchorsMin;
            var max = DefaultAnchorsMax;

            var upChange = UpChange;
            var scale = Canvas.CanvasScale;

            min += upChange * scale;
            max += upChange * scale;

            var diff = (min.y - 1f) * 0.5f;
            min.y -= diff;
            max.y -= diff;

            Root.anchorMin = min;
            Root.anchorMax = max;
        }

        [ContextMenu(nameof(MoveCenter) + "()")]
        public void MoveCenter()
        {
            var min = DefaultAnchorsMin;
            var max = DefaultAnchorsMax;

            Root.anchorMin = min;
            Root.anchorMax = max;
        }
    }
}

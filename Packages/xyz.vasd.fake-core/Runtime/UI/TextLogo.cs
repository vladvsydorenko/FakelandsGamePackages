using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Xyz.Vasd.FakeCore.UI
{
    public class TextLogo : MonoBehaviour
    {
        public TextMeshProUGUI TextElement;

        [Header("Text")]
        public string Text;
        public float Duration;

        [Header("Extra")]
        public string Extra;
        public float ExtraDuration;

        [Header("Events")]
        public UnityEvent OnTextComplete;
        public UnityEvent OnExtraComplete;

        public bool IsTextComplete { get; private set; }
        public bool IsExtraTextComplete { get; private set; }

        private bool _isPlaying;
        private float _time;

        private void Update()
        {
            if (!_isPlaying) return;

            var progress = _time / Duration;
            _time += Time.deltaTime;

            if (progress <= 0f) return;
            else if (progress > 1f)
            {
                if (!IsTextComplete)
                {
                    OnTextComplete.Invoke();
                    IsTextComplete = true;
                }

                progress = (_time - Duration) / ExtraDuration;
                var length = (int)Mathf.Floor(Extra.Length * progress);
                var extra = Extra;
                if (length <= Extra.Length) extra = Extra.Substring(0, length);
                else
                {
                    if (!IsExtraTextComplete)
                    {
                        OnExtraComplete.Invoke();
                        IsExtraTextComplete = true;
                    }
                }
                TextElement.text = Text + extra;
                return;
            }

            TextElement.text = Text.Substring(0, (int)Mathf.Floor(Text.Length * progress));
        }

        [ContextMenu("Play()")]
        public void Play()
        {
            _time = 0f;
            _isPlaying = true;
        }

        public void Stop()
        {
            _time = 0f;
            _isPlaying = false;
        }
    }
}

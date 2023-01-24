using UnityEngine;

namespace Xyz.Vasd.FakeCore.UI
{
    [ExecuteInEditMode]
    public class RawAspectRatioFitter : MonoBehaviour
    {
        public RectTransform ImageRect;

        private void OnEnable()
        {
            if (ImageRect == null) ImageRect = GetComponent<RectTransform>();
        }

        private void Update()
        {
            if (ImageRect == null) return;

            var width = (float)Screen.width;
            var height = (float)Screen.height;

            var isLandscape = width > height;

            if (isLandscape)
            {
                var ratio = (width / height - 1f) * 0.5f;

                ImageRect.anchorMin = new Vector2(0f, -ratio);
                ImageRect.anchorMax = new Vector2(1f, 1 + ratio);
            }
            else
            {
                var ratio = (height / width - 1f) * 0.5f;

                ImageRect.anchorMin = new Vector2(-ratio, 0f);
                ImageRect.anchorMax = new Vector2(1 + ratio, 1f);
            }

        }
    }
}

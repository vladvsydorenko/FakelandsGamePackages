﻿using TMPro;
using UnityEngine;

namespace Xyz.Vasd.Fakelands
{
    [ExecuteInEditMode]
    [AddComponentMenu("Fakelands/[Deprecated]/[Fakelands] Text Typer")]
    public class TextTyper : MonoBehaviour
    {
        [Header("Text")]
        public string Text;
        [Range(0f, 1f)]
        public float Progress;

        [Header("Elements")]
        public Color Color = Color.black;
        public TMP_FontAsset Font;
        public TextMeshProUGUI TextElement;

        [Header("Editor")]
        public bool ExecuteInEditor;

        protected virtual void Awake()
        {
            Refresh();
        }

        private void OnValidate()
        {
            Refresh();
        }

        protected virtual void Update()
        {
            #if UNITY_EDITOR
            if (!ExecuteInEditor && !Application.isPlaying) return;
            #endif
            
            if (TextElement == null) return;

            var text = "";
            var chars = Mathf.Clamp((int)(Text.Length * Progress), 0, Text.Length);

            if (chars > 0) text = Text[..chars];

            TextElement.color = Color;
            TextElement.text = text;
        }

        private void Refresh()
        {
            if (TextElement == null) TextElement = GetComponentInChildren<TextMeshProUGUI>();
            if (Text != null)
            {
                TextElement.color = Color;
                TextElement.font = Font;
            }
        }
    }
}

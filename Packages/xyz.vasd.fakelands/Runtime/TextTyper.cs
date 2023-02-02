﻿using TMPro;
using UnityEngine;

namespace Xyz.Vasd.Fakelands
{
    [ExecuteInEditMode]
    [AddComponentMenu("Fakelands/[Fakelands] Text Typer")]
    public class TextTyper : MonoBehaviour
    {
        [Header("Text")]
        public string Text;
        [Range(0f, 1f)]
        public float Progress;

        [Header("Elements")]
        public TextMeshProUGUI TextElement;

        [Header("Editor")]
        public bool ExecuteInEditor;

        protected virtual void Update()
        {
            #if UNITY_EDITOR
            if (!ExecuteInEditor && !Application.isPlaying) return;
            #endif
            
            if (TextElement == null) return;

            var text = "";
            var chars = Mathf.Clamp((int)(Text.Length * Progress), 0, Text.Length);

            if (chars > 0) text = Text[..chars];

            TextElement.text = text;
        }
    }
}

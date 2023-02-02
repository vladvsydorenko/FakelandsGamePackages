﻿using UnityEngine;

namespace Xyz.Vasd.Fake.Views
{
    [AddComponentMenu("Fake/[Fake] Canvas")]
    [ExecuteInEditMode]
    public class FakeCanvas : MonoBehaviour
    {
        [Tooltip("Distance from camera")]
        public float Distance;

        [Tooltip("Reference camera for canvas resize and reposition")]
        public Camera Camera;

        [Tooltip("Canvas to resize and reposition")]
        public Canvas Canvas;

        public bool UseLocalPosition;

        public Vector2 CameraSize { get; private set; }
        public Vector2 CanvasScale { get; private set; }

        // marks if all data is valid for processing
        private bool _isValid;
        private Transform _cameraTransform;
        private RectTransform _canvasTransform;

        [Header("Editor")]
        public bool ExecuteInEditor;

        protected virtual void OnEnable()
        {
            Refresh();
        }

        protected virtual void OnValidate()
        {
            Refresh();
        }

        protected virtual void Update()
        {
            if (!ExecuteInEditor && !Application.isPlaying) return;

            Refresh();
            if (!_isValid) return;

            Canvas.renderMode = RenderMode.WorldSpace;
            Canvas.worldCamera = Camera;

            _canvasTransform.sizeDelta = CameraSize;

            var cameraPosition = _cameraTransform.position;
            var canvasPosition = cameraPosition + _cameraTransform.forward * Distance;

            if (UseLocalPosition)
            {
                _canvasTransform.SetLocalPositionAndRotation(canvasPosition, _cameraTransform.rotation);
            }
            else
            {
                _canvasTransform.SetPositionAndRotation(canvasPosition, _cameraTransform.rotation);
            }
        }

        public void Refresh()
        {
            FindReferences();
            if (!_isValid) return;

            UpdateCameraSize();
            UpdateCanvasMode();
            UpdateCanvasScale();
        }

        private void FindReferences()
        {
            if (!ExecuteInEditor && !Application.isPlaying) return;

            _isValid = false;

            if (Canvas == null) Canvas = GetComponentInChildren<Canvas>();
            if (Camera == null) Camera = Camera.main;

            if (Canvas == null) return;
            if (Camera == null) return;

            _cameraTransform = Camera.transform;
            _canvasTransform = Canvas.transform as RectTransform;

            _isValid = (
                Canvas != null ||
                Camera != null ||
                _canvasTransform != null ||
                _cameraTransform != null);
        }

        private void UpdateCameraSize()
        {
            var camera = Camera.main;

            if (camera == null)
            {
                CameraSize = Vector2.zero;
                return;
            }

            // don't understand well this math tbh
            var magic = Mathf.Tan(camera.fieldOfView * 0.5f * Mathf.Deg2Rad);

            var frustumHeight = 2.0f * Distance * magic;
            var frustumWidth = frustumHeight * camera.aspect;

            CameraSize = new Vector2(frustumWidth, frustumHeight);
        }

        private void UpdateCanvasMode()
        {
            if (Canvas == null) return;
            Canvas.renderMode = RenderMode.WorldSpace;
        }

        private void UpdateCanvasScale()
        {
            Vector2 scale = Vector2.one;

            // landscape
            if (CameraSize.x > CameraSize.y)
            {
                scale.y = CameraSize.x / CameraSize.y;
            }
            else
            {
                scale.x = CameraSize.y / CameraSize.x;
            }

            CanvasScale = scale;
        }

        //public Canvas TargetCanvas;

        //protected virtual void Awake()
        //{
        //    if (TargetCanvas == null) TargetCanvas = GetComponent<Canvas>();
        //}
    }
}
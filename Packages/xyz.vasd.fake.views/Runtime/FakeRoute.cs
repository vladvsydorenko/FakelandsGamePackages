using UnityEngine;

namespace Xyz.Vasd.Fake.Views
{
    [ExecuteInEditMode]
    [AddComponentMenu("Fake/[Fake] Route")]
    public class FakeRoute : MonoBehaviour
    {
        [Header("Route")]
        public string Location;
        [Tooltip("Use gameobject's name as location")]
        public bool UseObjectName;

        [Header("View")]
        public GameObject Root;

        [Header("Editor")]
        public bool ExecuteInEditor;

        public IFakeView View;

        protected virtual void Awake()
        {
            Refresh();
        }

        protected virtual void Update()
        {
            Refresh();
        }

        protected virtual void OnValidate()
        {
            Refresh();
        }

        private void Refresh()
        {
            if (!ExecuteInEditor && !Application.isPlaying) return;

            if (Root == null) Root = gameObject;
            View = Root.GetComponentInChildren<IFakeView>(includeInactive: true);
            if (UseObjectName) Location = gameObject.name;
        }
    }
}

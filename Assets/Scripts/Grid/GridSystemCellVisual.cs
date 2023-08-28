using UnityEngine;

namespace Grid
{
    public class GridSystemCellVisual : MonoBehaviour
    {
        [SerializeField]
        private MeshRenderer meshRenderer;

        public void Show(Material m)
        {
            if (m)
            {
                meshRenderer.material = m;
            }

            meshRenderer.enabled = true;
        }

        public void Hide() { meshRenderer.enabled = false; }
    }
}
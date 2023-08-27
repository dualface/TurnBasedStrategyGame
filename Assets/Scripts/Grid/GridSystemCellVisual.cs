using UnityEngine;

namespace Grid
{
    public class GridSystemCellVisual : MonoBehaviour
    {
        [SerializeField]
        private MeshRenderer meshRenderer;

        public void Show(Material material)
        {
            if (material)
            {
                meshRenderer.material = material;
            }
            meshRenderer.enabled = true;
        }

        public void Hide()
        {
            meshRenderer.enabled = false;
        }
    }
}

using UnityEngine;

namespace UnitSystem
{
    public class Ground : MonoBehaviour
    {
        [SerializeField]
        private LayerMask groundLayer;

        [SerializeField]
        private GameObject mousePointer;

        public Vector3 MousePosition { get; private set; }

        public bool IsMouseOnGround { get; private set; }

        private void Update()
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit, float.MaxValue, groundLayer))
            {
                IsMouseOnGround = true;
                MousePosition = hit.point;
                mousePointer.SetActive(true);
                mousePointer.transform.position = hit.point;
            }
            else
            {
                IsMouseOnGround = false;
                mousePointer.SetActive(false);
            }
        }
    }
}

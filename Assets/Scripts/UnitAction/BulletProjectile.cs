using UnityEngine;

namespace UnitAction
{
    public class BulletProjectile : MonoBehaviour
    {
        [SerializeField]
        private TrailRenderer trailRenderer;

        [SerializeField]
        private float moveSpeed = 100f;

        [SerializeField]
        private GameObject bulletHitVFXPrefab;

        private Vector3 _targetPosition;

        private void Update()
        {
            var p = transform.position;
            var moveDir = (_targetPosition - p).normalized;
            var dist = Vector3.Distance(p, _targetPosition);
            p += moveSpeed * Time.deltaTime * moveDir;

            transform.position = p;
            var newDistance = Vector3.Distance(p, _targetPosition);
            if (newDistance > dist)
            {
                OnHit();
            }
        }

        public void Setup(Vector3 targetPosition) { _targetPosition = targetPosition; }

        private void OnHit()
        {
            transform.position = _targetPosition;
            trailRenderer.transform.parent = null;
            Destroy(gameObject);
            Instantiate(bulletHitVFXPrefab, _targetPosition, Quaternion.identity);
        }
    }
}
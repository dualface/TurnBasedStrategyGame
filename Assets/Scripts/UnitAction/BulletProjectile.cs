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

        public void Setup(Vector3 targetPosition)
        {
            _targetPosition = targetPosition;
        }

        private void Update()
        {
            var moveDirection = (_targetPosition - transform.position).normalized;
            var distance = Vector3.Distance(transform.position, _targetPosition);
            transform.position += moveSpeed * Time.deltaTime * moveDirection;
            var newDistance = Vector3.Distance(transform.position, _targetPosition);
            if (newDistance > distance)
            {
                OnHit();
            }
        }

        private void OnHit()
        {
            transform.position = _targetPosition;
            trailRenderer.transform.parent = null;
            Destroy(gameObject);

            Instantiate(bulletHitVFXPrefab, _targetPosition, Quaternion.identity);
        }
    }
}

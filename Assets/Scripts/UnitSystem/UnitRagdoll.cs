using UnityEngine;

namespace UnitSystem
{
    public class UnitRagdoll : MonoBehaviour
    {
        [SerializeField]
        private Transform rootBone;

        public void Setup(Transform originRootBone)
        {
            CopyChildTransforms(originRootBone, rootBone);
            ApplyExplosion(rootBone, 300f, transform.position, 10f);
        }

        private static void CopyChildTransforms(Transform originBone, Transform bone)
        {
            foreach (Transform child in originBone)
            {
                var targetChild = bone.Find(child.name);
                if (!targetChild)
                {
                    continue;
                }

                targetChild.SetPositionAndRotation(child.position, child.rotation);
                CopyChildTransforms(child, targetChild);
            }
        }

        private static void ApplyExplosion(Transform bone,
                                           float explosionForce,
                                           Vector3 explosionPosition,
                                           float explosionRadius)
        {
            foreach (Transform child in bone)
            {
                if (child.TryGetComponent<Rigidbody>(out var rigidbody))
                {
                    rigidbody.AddExplosionForce(explosionForce, explosionPosition, explosionRadius);
                }

                ApplyExplosion(child, explosionForce, explosionPosition, explosionRadius);
            }
        }
    }
}

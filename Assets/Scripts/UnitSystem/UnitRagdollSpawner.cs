using UnityEngine;

namespace UnitSystem
{
    public class UnitRagdollSpawner : MonoBehaviour
    {
        [SerializeField]
        private GameObject ragdollPrefab;

        [SerializeField]
        private Transform originRagdollRootBone;

        private void Awake()
        {
            if (TryGetComponent<UnitHealth>(out var unitHealth))
            {
                unitHealth.OnDead += OnDead;
            }
        }

        private void OnDead()
        {
            var o = Instantiate(ragdollPrefab, transform.position, transform.rotation);
            o.GetComponent<UnitRagdoll>().Setup(originRagdollRootBone);
        }
    }
}

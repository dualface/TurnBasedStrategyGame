using UnitAction;
using UnityEngine;

namespace UnitSystem
{
    public class UnitAnimator : MonoBehaviour
    {
        private static readonly int IsWalking = Animator.StringToHash("IsWalking");
        private static readonly int Shoot = Animator.StringToHash("Shoot");

        [SerializeField]
        private Animator animator;

        [SerializeField]
        private Transform shootPoint;

        [SerializeField]
        private GameObject bulletProjectilePrefab;


        private void Awake()
        {
            if (TryGetComponent<MoveAction>(out var moveAction))
            {
                moveAction.OnMoveStart += () => animator.SetBool(IsWalking, true);
                moveAction.OnMoveComplete += () => animator.SetBool(IsWalking, false);
            }

            if (TryGetComponent<ShootAction>(out var shootAction))
            {
                shootAction.OnShooting += OnShoot;
            }
        }

        private void OnShoot(ShootAction.OnShootingArgs args)
        {
            animator.SetTrigger(Shoot);
            var bullet = Instantiate(bulletProjectilePrefab, shootPoint.position, Quaternion.identity);
            var targetPosition = args.TargetUnit.WorldPosition;
            targetPosition.y = shootPoint.position.y;
            bullet.GetComponent<BulletProjectile>().Setup(targetPosition);
        }
    }
}

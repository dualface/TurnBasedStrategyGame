using System;
using System.Collections.Generic;
using Grid;
using UnitSystem;
using UnityEngine;

namespace UnitAction
{
    public class ShootAction : BaseAction
    {
        [SerializeField]
        private int maxShootDistance = 5;

        [SerializeField]
        private float aimingDuration = 1f;

        [SerializeField]
        private float shootDuration = 0.2f;

        [SerializeField]
        private float coolOffDuration = 0.5f;

        [SerializeField]
        private float rotateSpeed = 10f;

        private ShootState _state = ShootState.Aiming;
        private float _stateTimer;

        public Unit TargetUnit { get; private set; }

        private void Update()
        {
            if (!IsActive)
            {
                return;
            }

            if (_state == ShootState.Aiming)
            {
                var aimDir = (TargetUnit.WorldPosition - transform.position).normalized;
                transform.forward = Vector3.Lerp(transform.forward, aimDir, rotateSpeed * Time.deltaTime);
            }

            _stateTimer -= Time.deltaTime;
            if (_stateTimer <= 0)
            {
                NextState();
            }
        }

        public event Action<ShootingArgs> OnShooting;

        public override List<GridPosition> GetValidActionPositions()
        {
            var level = LevelGrid.Instance;
            var positions = new List<GridPosition>();
            for (var c = -maxShootDistance; c <= maxShootDistance; c++)
            {
                for (var r = -maxShootDistance; r <= maxShootDistance; r++)
                {
                    var dist = Vector2Int.Distance(new Vector2Int(c, r), Vector2Int.zero);
                    if (dist > maxShootDistance)
                    {
                        continue;
                    }

                    var offset = new GridPosition(r, c);
                    var test = OwnerUnit.GridPosition + offset;

                    if (!level.IsValidGridPosition(test))
                    {
                        continue;
                    }

                    if (OwnerUnit.GridPosition == test)
                    {
                        continue;
                    }

                    var units = level.GetUnitListAtPosition(test);
                    if (units.Count == 0)
                    {
                        continue;
                    }

                    foreach (var unit in units)
                    {
                        if (unit.IsEnemy != OwnerUnit.IsEnemy)
                        {
                            positions.Add(test);
                        }
                    }
                }
            }

            return positions;
        }

        public List<GridPosition> GetRangePositions()
        {
            var level = LevelGrid.Instance;

            var positions = new List<GridPosition>();
            for (var c = -maxShootDistance; c <= maxShootDistance; c++)
            {
                for (var r = -maxShootDistance; r <= maxShootDistance; r++)
                {
                    var dist = Vector2Int.Distance(new Vector2Int(c, r), Vector2Int.zero);
                    if (dist > maxShootDistance)
                    {
                        continue;
                    }

                    var offset = new GridPosition(r, c);
                    var test = OwnerUnit.GridPosition + offset;

                    if (!level.IsValidGridPosition(test))
                    {
                        continue;
                    }

                    if (OwnerUnit.GridPosition == test)
                    {
                        continue;
                    }

                    positions.Add(test);
                }
            }

            return positions;
        }

        public override void TakeAction(GridPosition p, Action onActionComplete)
        {
            var units = LevelGrid.Instance.GetUnitListAtPosition(p);
            if (units.Count == 0)
            {
                return;
            }

            TargetUnit = units[0];
            _state = ShootState.Aiming;
            _stateTimer = aimingDuration;
            Debug.Log($"Unit {OwnerUnit.name} is enter state {_state}");

            ActionStart(onActionComplete);
        }

        protected override string GetActionName() => "Shoot";

        private void NextState()
        {
            switch (_state)
            {
            case ShootState.Aiming:
                _state = ShootState.Shooting;
                _stateTimer = shootDuration;
                break;
            case ShootState.Shooting:
                _state = ShootState.CoolOff;
                _stateTimer = coolOffDuration;
                Shoot();
                break;
            case ShootState.CoolOff:
                _state = ShootState.Idle;
                ActionComplete();
                break;
            }

            Debug.Log($"Unit {OwnerUnit.name} is enter state {_state}");
        }

        private void Shoot()
        {
            TargetUnit.TakeDamage(40);
            OnShooting?.Invoke(new ShootingArgs(TargetUnit));
        }

        public class ShootingArgs
        {
            public ShootingArgs(Unit targetUnit) { TargetUnit = targetUnit; }

            public Unit TargetUnit { get; }
        }

        private enum ShootState
        {
            Aiming,
            Shooting,
            CoolOff,
            Idle
        }
    }
}

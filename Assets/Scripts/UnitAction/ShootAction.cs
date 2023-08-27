using System;
using System.Collections.Generic;
using Grid;
using UnityEngine;
using UnitSystem;

namespace UnitAction
{
    public class ShootAction : BaseAction
    {
        public event Action<OnShootingArgs> OnShooting;

        public class OnShootingArgs
        {
            public OnShootingArgs(Unit shootingUnit, Unit targetUnit)
            {
                ShootingUnit = shootingUnit;
                TargetUnit = targetUnit;
            }

            public Unit TargetUnit { get; }
            public Unit ShootingUnit { get; }
        }

        public Unit TargetUnit { get; private set; }

        [SerializeField]
        private int maxShootDistance = 5;

        [SerializeField]
        private float aimingDuration = 1f;

        [SerializeField]
        private float shootDuration = 0.2f;

        [SerializeField]
        private float cooloffDuration = 0.5f;

        [SerializeField]
        private float rotateSpeed = 10f;

        private enum ShootState
        {
            Aiming,
            Shooting,
            Cooloff,
            Idle,
        }

        private ShootState _state = ShootState.Aiming;
        private float _stateTimer;

        public override List<GridPosition> GetValidActionGridPositionList()
        {
            var level = LevelGrid.Instance;

            var validPositionList = new List<GridPosition>();
            for (var column = -maxShootDistance; column <= maxShootDistance; column++)
            {
                for (var row = -maxShootDistance; row <= maxShootDistance; row++)
                {
                    var distance = Vector2Int.Distance(new Vector2Int(column, row), Vector2Int.zero);
                    if (distance > maxShootDistance)
                    {
                        continue;
                    }

                    var offsetPosition = new GridPosition(row, column);
                    var testPosition = OwnerUnit.GridPosition + offsetPosition;

                    if (!level.IsValidGridPosition(testPosition))
                    {
                        continue;
                    }

                    if (OwnerUnit.GridPosition == testPosition)
                    {
                        continue;
                    }

                    var unitList = level.GetUnitListAtPosition(testPosition);
                    if (unitList.Count == 0)
                    {
                        continue;
                    }

                    foreach (var unit in unitList)
                    {
                        if (unit.IsEnemy != OwnerUnit.IsEnemy)
                        {
                            validPositionList.Add(testPosition);
                        }
                    }
                }
            }

            return validPositionList;
        }

        public List<GridPosition> GetRangeGridPositionList()
        {
            var level = LevelGrid.Instance;

            var rangePositionList = new List<GridPosition>();
            for (var column = -maxShootDistance; column <= maxShootDistance; column++)
            {
                for (var row = -maxShootDistance; row <= maxShootDistance; row++)
                {
                    var distance = Vector2Int.Distance(new Vector2Int(column, row), Vector2Int.zero);
                    if (distance > maxShootDistance)
                    {
                        continue;
                    }

                    var offsetPosition = new GridPosition(row, column);
                    var testPosition = OwnerUnit.GridPosition + offsetPosition;

                    if (!level.IsValidGridPosition(testPosition))
                    {
                        continue;
                    }

                    if (OwnerUnit.GridPosition == testPosition)
                    {
                        continue;
                    }

                    rangePositionList.Add(testPosition);
                }
            }

            return rangePositionList;
        }

        public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
        {
            var unitList = LevelGrid.Instance.GetUnitListAtPosition(gridPosition);
            if (unitList.Count == 0)
            {
                return;
            }

            TargetUnit = unitList[0];
            _state = ShootState.Aiming;
            _stateTimer = aimingDuration;
            Debug.Log($"Unit {OwnerUnit.name} is enter state {_state}");

            ActionStart(onActionComplete);
        }

        protected override string GetActionName() => "Shoot";

        private void Update()
        {
            if (!IsActive)
            {
                return;
            }

            if (_state == ShootState.Aiming)
            {
                var aimDirection = (TargetUnit.WorldPosition - transform.position).normalized;
                transform.forward = Vector3.Lerp(transform.forward, aimDirection, rotateSpeed * Time.deltaTime);
            }

            _stateTimer -= Time.deltaTime;
            if (_stateTimer <= 0)
            {
                NextState();
            }
        }

        private void NextState()
        {
            switch (_state)
            {
                case ShootState.Aiming:
                    _state = ShootState.Shooting;
                    _stateTimer = shootDuration;
                    break;
                case ShootState.Shooting:
                    _state = ShootState.Cooloff;
                    _stateTimer = cooloffDuration;
                    Shoot();
                    break;
                case ShootState.Cooloff:
                    _state = ShootState.Idle;
                    ActionComplete();
                    break;
            }
            Debug.Log($"Unit {OwnerUnit.name} is enter state {_state}");
        }

        private void Shoot()
        {
            TargetUnit.TakeDamage(40);
            OnShooting?.Invoke(new OnShootingArgs(OwnerUnit, TargetUnit));
        }
    }
}

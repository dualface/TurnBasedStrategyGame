using System;
using System.Collections.Generic;
using Grid;
using UnityEngine;

namespace UnitAction
{
    public class MoveAction : BaseAction
    {
        public event Action OnMoveStart;
        public event Action OnMoveComplete;

        [SerializeField]
        private int maxMoveDistance = 2;

        [SerializeField]
        private float moveSpeed = 4.0f;

        [SerializeField]
        private float rotateSpeed = 10.0f;

        private Vector3 _targetPosition;

        public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
        {
            _targetPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
            OnMoveStart?.Invoke();
            ActionStart(onActionComplete);
        }

        public override List<GridPosition> GetValidActionGridPositionList()
        {
            var level = LevelGrid.Instance;

            var validPositionList = new List<GridPosition>();
            for (var column = -maxMoveDistance; column <= maxMoveDistance; column++)
                for (var row = -maxMoveDistance; row <= maxMoveDistance; row++)
                {
                    var offsetPosition = new GridPosition(row, column);
                    var testPosition = OwnerUnit.GridPosition + offsetPosition;

                    if (!level.IsValidGridPosition(testPosition))
                    {
                        continue;
                    }

                    if (level.HasAnyUnitAtGridPosition(testPosition))
                    {
                        continue;
                    }

                    if (OwnerUnit.GridPosition == testPosition)
                    {
                        continue;
                    }

                    validPositionList.Add(testPosition);
                }

            return validPositionList;
        }

        protected override string GetActionName() => "Move";

        private void Update()
        {
            if (!IsActive)
            {
                return;
            }

            var beforeDistance = Vector3.Distance(transform.position, _targetPosition);
            var moveDirection = (_targetPosition - transform.position).normalized;
            transform.position += moveSpeed * Time.deltaTime * moveDirection;
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, rotateSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, _targetPosition) > beforeDistance)
            {
                transform.position = _targetPosition;
                Arrived();
            }
        }

        private void Arrived()
        {
            OnMoveComplete?.Invoke();
            ActionComplete();
        }
    }
}

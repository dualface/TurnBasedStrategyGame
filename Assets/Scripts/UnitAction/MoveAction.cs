using System;
using System.Collections.Generic;
using Grid;
using UnityEngine;

namespace UnitAction
{
    public class MoveAction : BaseAction
    {
        [SerializeField]
        private int maxMoveDistance = 2;

        [SerializeField]
        private float moveSpeed = 4.0f;

        [SerializeField]
        private float rotateSpeed = 10.0f;

        private Vector3 _targetPosition;

        private void Update()
        {
            if (!IsActive)
            {
                return;
            }

            var p = transform.position;
            var beforeDist = Vector3.Distance(p, _targetPosition);
            var moveDir = (_targetPosition - p).normalized;
            p += moveSpeed * Time.deltaTime * moveDir;

            transform.position = p;
            transform.forward = Vector3.Lerp(transform.forward, moveDir, rotateSpeed * Time.deltaTime);
            if (!(Vector3.Distance(transform.position, _targetPosition) > beforeDist))
            {
                return;
            }

            transform.position = _targetPosition;
            Arrived();
        }

        public event Action OnMoveStart;
        public event Action OnMoveComplete;

        public override void TakeAction(GridPosition p, Action onActionComplete)
        {
            _targetPosition = LevelGrid.Instance.GetWorldPosition(p);
            OnMoveStart?.Invoke();
            ActionStart(onActionComplete);
        }

        public override List<GridPosition> GetValidActionPositions()
        {
            var level = LevelGrid.Instance;
            var positions = new List<GridPosition>();
            for (var c = -maxMoveDistance; c <= maxMoveDistance; c++)
            {
                for (var r = -maxMoveDistance; r <= maxMoveDistance; r++)
                {
                    var offset = new GridPosition(r, c);
                    var test = OwnerUnit.GridPosition + offset;

                    if (!level.IsValidGridPosition(test))
                    {
                        continue;
                    }

                    if (level.HasAnyUnitAtGridPosition(test))
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

        protected override string GetActionName() => "Move";

        private void Arrived()
        {
            OnMoveComplete?.Invoke();
            ActionComplete();
        }
    }
}

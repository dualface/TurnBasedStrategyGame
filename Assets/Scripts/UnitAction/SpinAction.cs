using System;
using System.Collections.Generic;
using Grid;
using UnityEngine;

namespace UnitAction
{
    public class SpinAction : BaseAction
    {
        private float _totalSpinAmount;

        private void Update()
        {
            if (!IsActive)
            {
                return;
            }

            var spinAmount = 360.0f * Time.deltaTime;
            _totalSpinAmount += spinAmount;
            transform.Rotate(Vector3.up, spinAmount);
            if (_totalSpinAmount >= 360.0f)
            {
                SpinEnded();
            }
        }

        protected override string GetActionName() => "Spin";

        public override List<GridPosition> GetValidActionGridPositionList()
        {
            return new List<GridPosition> { OwnerUnit.GridPosition };
        }

        public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
        {
            _totalSpinAmount = 0;
            ActionStart(onActionComplete);
        }

        private void SpinEnded()
        {
            ActionComplete();
        }
    }
}

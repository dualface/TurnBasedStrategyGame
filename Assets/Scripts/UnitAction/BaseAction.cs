using System;
using System.Collections.Generic;
using Grid;
using UnityEngine;
using UnitSystem;

namespace UnitAction
{
    public abstract class BaseAction : MonoBehaviour
    {
        public bool IsSelected { get; private set; }

        protected bool IsActive;

        protected Action OnActionComplete;

        protected Unit OwnerUnit;

        public string ActionName => GetActionName();

        protected virtual void Awake()
        {
            OwnerUnit = GetComponent<Unit>();
        }

        protected abstract string GetActionName();

        protected void ActionStart(Action onActionComplete)
        {
            IsActive = true;
            OnActionComplete = onActionComplete;
        }

        protected void ActionComplete()
        {
            IsActive = false;
            OnActionComplete?.Invoke();
            OnActionComplete = null;
        }

        public bool IsValidActionPosition(GridPosition position) => GetValidActionGridPositionList().Contains(position);

        public abstract List<GridPosition> GetValidActionGridPositionList();

        public abstract void TakeAction(GridPosition gridPosition, Action onActionComplete);

        public virtual int GetActionPointsCost() => 1;

        public void SetSelected(bool selected)
        {
            IsSelected = selected;
        }
    }
}

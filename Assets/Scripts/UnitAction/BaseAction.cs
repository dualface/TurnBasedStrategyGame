using System;
using System.Collections.Generic;
using Grid;
using UnityEngine;
using UnitSystem;

namespace UnitAction
{
    public abstract class BaseAction : MonoBehaviour
    {
        public static event Action<BaseAction> OnAnyActionStarted;
        public static event Action<BaseAction> OnAnyActionCompleted;

        public Unit OwnerUnit { get; private set; }

        public bool IsSelected { get; set; }

        public string ActionName => GetActionName();

        public bool IsValidActionPosition(GridPosition position) => GetValidActionGridPositionList().Contains(position);

        public abstract List<GridPosition> GetValidActionGridPositionList();

        public abstract void TakeAction(GridPosition gridPosition, Action onActionComplete);

        public virtual int GetActionPointsCost() => 1;

        protected bool IsActive;

        protected Action OnActionComplete;

        protected virtual void Awake()
        {
            OwnerUnit = GetComponent<Unit>();
        }

        protected abstract string GetActionName();

        protected void ActionStart(Action onActionComplete)
        {
            IsActive = true;
            OnActionComplete = onActionComplete;
            OnAnyActionStarted?.Invoke(this);
        }

        protected void ActionComplete()
        {
            IsActive = false;
            OnActionComplete?.Invoke();
            OnActionComplete = null;
            OnAnyActionCompleted?.Invoke(this);
        }
    }
}

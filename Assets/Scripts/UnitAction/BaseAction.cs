using System;
using System.Collections.Generic;
using Grid;
using UnitSystem;
using UnityEngine;

namespace UnitAction
{
    public abstract class BaseAction : MonoBehaviour
    {
        private Action _actionComplete;

        protected bool IsActive;

        public Unit OwnerUnit { get; private set; }

        public bool IsSelected { get; set; }

        public string ActionName => GetActionName();

        protected virtual void Awake() { OwnerUnit = GetComponent<Unit>(); }

        public static event Action<BaseAction> OnAnyActionStarted;
        public static event Action<BaseAction> OnAnyActionCompleted;

        public bool IsValidActionPosition(GridPosition p) => GetValidActionPositions().Contains(p);

        public abstract List<GridPosition> GetValidActionPositions();

        public abstract void TakeAction(GridPosition p, Action onActionComplete);

        public virtual int GetActionPointsCost() => 1;

        protected abstract string GetActionName();

        protected void ActionStart(Action onActionComplete)
        {
            IsActive = true;
            _actionComplete = onActionComplete;
            OnAnyActionStarted?.Invoke(this);
        }

        protected void ActionComplete()
        {
            IsActive = false;
            _actionComplete?.Invoke();
            _actionComplete = null;
            OnAnyActionCompleted?.Invoke(this);
        }
    }
}

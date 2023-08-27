using System;
using System.Collections.Generic;
using UnitAction;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UnitSystem
{
    public class UnitActionSystem : MonoBehaviour
    {
        [SerializeField]
        private LayerMask unitLayer;

        [SerializeField]
        private Ground ground;

        private readonly List<Unit> _units = new();

        private bool _isBusy;

        private BaseAction _selectedAction;

        public static UnitActionSystem Instance { get; private set; }

        public Unit SelectedUnit { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("There's more than one LevelManager in the scene! " + transform + " - " + Instance);
                Destroy(gameObject);
                return;
            }

            Instance = this;

            TurnSystem.Instance.OnStartNewTurn += OnStartNewTurn;
        }

        private void Start()
        {
            ClearBusy();
        }

        private void Update()
        {
            if (_isBusy)
            {
                return;
            }

            if (!TurnSystem.Instance.IsPlayerTurn)
            {
                return;
            }

            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            if (TryHandleUnitSelection())
            {
                return;
            }

            HandleSelectedAction();
        }

        public event Action<Unit> OnSelectedUnitChanged;

        public event Action<BaseAction> OnSelectedActionChanged;

        public event Action<bool> OnBusyChanged;

        private bool TryHandleUnitSelection()
        {
            if (!Input.GetMouseButtonDown(0))
            {
                return false;
            }

            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out var hit, float.MaxValue, unitLayer))
            {
                return false;
            }

            if (!hit.transform.TryGetComponent(out Unit unit))
            {
                return false;
            }

            if (unit.IsEnemy)
            {
                return false;
            }

            if (unit == SelectedUnit)
            {
                return false;
            }

            SetSelectedUnit(unit);
            return true;
        }

        private void HandleSelectedAction()
        {
            if (!Input.GetMouseButtonDown(0))
            {
                return;
            }

            if (!_selectedAction)
            {
                return;
            }

            var pos = LevelGrid.Instance.GetGridPosition(ground.MousePosition);
            if (!_selectedAction.IsValidActionPosition(pos))
            {
                return;
            }

            if (!SelectedUnit.TrySpendActionPointToTakeAction(_selectedAction))
            {
                return;
            }

            SetBusy();
            _selectedAction.TakeAction(pos, ClearBusy);
        }

        private void SetSelectedUnit(Unit unit)
        {
            if (SelectedUnit == unit)
            {
                return;
            }

            if (SelectedUnit)
            {
                SelectedUnit.SetSelected(false);
            }

            SelectedUnit = unit;
            SelectedUnit.SetSelected(true);
            SetSelectedAction(SelectedUnit.DefaultAction);

            OnSelectedUnitChanged?.Invoke(SelectedUnit);
            Debug.Log($"Select unit {SelectedUnit.gameObject.name}");
        }

        private void SetBusy()
        {
            _isBusy = true;
            OnBusyChanged?.Invoke(_isBusy);
        }

        private void ClearBusy()
        {
            _isBusy = false;
            UpdateGridVisual();
            OnBusyChanged?.Invoke(_isBusy);
        }

        private void OnStartNewTurn(bool isPlayerTurn, int round)
        {
            foreach (var unit in _units)
            {
                unit.StartNewTurn(isPlayerTurn);
            }
        }

        public void SetSelectedAction(BaseAction action)
        {
            if (action == _selectedAction)
            {
                return;
            }

            if (_selectedAction)
            {
                _selectedAction.SetSelected(false);
            }

            Debug.Log($"Select action {action.ActionName}");
            _selectedAction = action;
            _selectedAction.SetSelected(true);
            OnSelectedActionChanged?.Invoke(_selectedAction);
            UpdateGridVisual();
        }

        private void UpdateGridVisual()
        {
            var visual = LevelGrid.Instance.GetVisual();
            visual.HideAll();
            if (_selectedAction)
            {
                visual.ShowPositionList(_selectedAction.GetValidActionGridPositionList());
            }
        }

        public void AddUnit(Unit unit)
        {
            _units.Add(unit);
        }

        internal void RemoveUnit(Unit unit)
        {
            _units.Remove(unit);
        }
    }
}

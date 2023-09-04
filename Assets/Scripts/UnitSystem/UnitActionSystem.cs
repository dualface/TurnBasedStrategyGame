using System;
using System.Collections.Generic;
using Grid;
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

        public List<Unit> EnemyUnits { get; private set; } = new();

        public List<Unit> FriendlyUnits { get; private set; } = new();

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

        private void Start() { ClearBusy(); }

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
            SetSelectedAction(SelectedUnit.defaultAction);

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
            UpdateSelectedActionGridVisual();
            OnBusyChanged?.Invoke(_isBusy);
        }

        private void OnStartNewTurn(TurnSystem.StartNewTurnArgs a)
        {
            foreach (var unit in _units)
            {
                unit.StartNewTurn(a.IsPlayerTurn);
            }

            UpdateSelectedActionGridVisual();
        }

        private void UpdateSelectedActionGridVisual()
        {
            if (!_selectedAction)
            {
                return;
            }

            var visual = LevelGrid.Instance.GridVisual;
            visual.HideAll();

            var canTakeAction = SelectedUnit.ActionPoints >= _selectedAction.GetActionPointsCost();
            var color = canTakeAction ? GridSystemVisual.CellColor.White : GridSystemVisual.CellColor.Gray;

            switch (_selectedAction)
            {
            case SpinAction:
                if (canTakeAction)
                {
                    color = GridSystemVisual.CellColor.Blue;
                }

                break;
            case ShootAction shootAction:
                var positions = shootAction.GetRangePositions();
                if (canTakeAction)
                {
                    visual.ShowPositions(positions, GridSystemVisual.CellColor.RedSoft);
                    color = GridSystemVisual.CellColor.Red;
                }
                else
                {
                    visual.ShowPositions(positions, color);
                }

                break;
            case MoveAction:
            default:
                break;
            }

            visual.ShowPositions(_selectedAction.GetValidActionPositions(), color);
        }

        public void SetSelectedAction(BaseAction action)
        {
            if (action == _selectedAction)
            {
                return;
            }

            if (_selectedAction)
            {
                _selectedAction.IsSelected = false;
            }

            _selectedAction = action;
            _selectedAction.IsSelected = true;
            UpdateSelectedActionGridVisual();

            OnSelectedActionChanged?.Invoke(_selectedAction);
            Debug.Log($"Select action {action.ActionName}");
        }

        public void AddUnit(Unit unit)
        {
            _units.Add(unit);
            if (unit.IsEnemy)
            {
                EnemyUnits.Add(unit);
            }
            else
            {
                FriendlyUnits.Add(unit);
            }
        }

        public void RemoveUnit(Unit unit)
        {
            _units.Remove(unit);
            if (unit.IsEnemy)
            {
                EnemyUnits.Remove(unit);
            }
            else
            {
                FriendlyUnits.Remove(unit);
            }
        }
    }
}

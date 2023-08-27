using System.Collections.Generic;
using UnitAction;
using UnityEngine;
using UnitSystem;

namespace UI
{
    public class UnitActionSystemUI : MonoBehaviour
    {
        [SerializeField]
        private GameObject actionButtonPrefab;

        [SerializeField]
        private Transform actionButtonsContainer;

        [SerializeField]
        private GameObject busyUI;

        [SerializeField]
        private ActionPointsUI actionPointsUI;

        private readonly List<ActionButtonUI> _buttons = new();

        private UnitActionSystem _sys;

        private void Start()
        {
            _sys = UnitActionSystem.Instance;
            _sys.OnSelectedUnitChanged += OnSelectedUnitChanged;
            _sys.OnSelectedActionChanged += OnSelectedActionChanged;
            _sys.OnBusyChanged += OnBusyChanged;

            TurnSystem.Instance.OnStartNewTurn += OnStartNewTurn;

            actionPointsUI.HideActionPoints();

            if (_sys.SelectedUnit)
            {
                CreateActionButtons(_sys.SelectedUnit);
            }
        }

        private void OnSelectedUnitChanged(Unit unit)
        {
            CreateActionButtons(unit);

            if (unit)
            {
                actionPointsUI.UpdateActionPoints(unit);
            }
            else
            {
                actionPointsUI.HideActionPoints();
            }
        }

        private void OnSelectedActionChanged(BaseAction action)
        {
            UpdateSelectedVisual();
        }

        private void OnBusyChanged(bool busy)
        {
            busyUI.SetActive(busy);
            UpdateActionPoints(!busy);
        }

        private void UpdateActionPoints(bool show)
        {
            if (show)
            {
                actionPointsUI.UpdateActionPoints(_sys.SelectedUnit);
            }
            else
            {
                actionPointsUI.HideActionPoints();
            }
        }

        private void CreateActionButtons(Unit unit)
        {
            foreach (Transform button in actionButtonsContainer)
            {
                Destroy(button.gameObject);
            }

            _buttons.Clear();
            foreach (var action in unit.Actions)
            {
                var obj = Instantiate(actionButtonPrefab, actionButtonsContainer);
                var button = obj.GetComponent<ActionButtonUI>();
                button.SetAction(action);
                button.Button.onClick.AddListener(() =>
                {
                    _sys.SetSelectedAction(button.Action);
                });
                _buttons.Add(button);
            }

            UpdateSelectedVisual();
        }

        private void UpdateSelectedVisual()
        {
            foreach (var button in _buttons)
            {
                button.UpdateSelectedVisual();
            }
        }

        private void OnStartNewTurn(bool isPlayerTurn, int round)
        {
            UpdateActionPoints(isPlayerTurn);
        }
    }
}

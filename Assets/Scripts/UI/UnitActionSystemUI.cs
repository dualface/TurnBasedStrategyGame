using System.Collections.Generic;
using UnitAction;
using UnitSystem;
using UnityEngine;

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

        private void Start()
        {
            TurnSystem.Instance.OnStartNewTurn += OnStartNewTurn;

            UnitActionSystem.Instance.OnSelectedUnitChanged += OnSelectedUnitChanged;
            UnitActionSystem.Instance.OnSelectedActionChanged += OnSelectedActionChanged;
            UnitActionSystem.Instance.OnBusyChanged += OnBusyChanged;
            var selected = UnitActionSystem.Instance.SelectedUnit;
            if (selected)
            {
                CreateActionButtons(selected);
            }

            actionPointsUI.HideActionPoints();
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

        private void OnSelectedActionChanged(BaseAction action) { UpdateSelectedVisual(); }

        private void OnBusyChanged(bool busy)
        {
            busyUI.SetActive(busy);
            UpdateActionPoints(!busy);
        }

        private void UpdateActionPoints(bool show)
        {
            if (show)
            {
                actionPointsUI.UpdateActionPoints(UnitActionSystem.Instance.SelectedUnit);
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
                var o = Instantiate(actionButtonPrefab, actionButtonsContainer);
                var button = o.GetComponent<ActionButtonUI>();
                button.SetAction(action);
                button.Button.onClick.AddListener(() => { UnitActionSystem.Instance.SetSelectedAction(button.Action); });
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

        private void OnStartNewTurn(TurnSystem.StartNewTurnArgs a) { UpdateActionPoints(a.IsPlayerTurn); }
    }
}

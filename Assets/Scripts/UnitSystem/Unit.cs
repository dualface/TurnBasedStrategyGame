using System;
using Grid;
using UnitAction;
using UnityEngine;

namespace UnitSystem
{
    public class Unit : MonoBehaviour
    {
        public const int DefaultActionPoints = 2;

        [SerializeField]
        private GameObject selectionVisual;

        [SerializeField]
        private Color selectedColor;

        [SerializeField]
        private bool isEnemy;

        public int ActionPoints { get; private set; } = DefaultActionPoints;

        public GridPosition GridPosition { get; private set; }

        public Vector3 WorldPosition => transform.position;

        public MoveAction DefaultAction { get; private set; }

        public BaseAction[] Actions { get; private set; }

        public bool IsEnemy => isEnemy;

        private UnitHealth _health;

        private void Awake()
        {
            DefaultAction = GetComponent<MoveAction>();
            Actions = GetComponents<BaseAction>();
            _health = GetComponent<UnitHealth>();
        }

        private void Start()
        {
            var level = LevelGrid.Instance;
            GridPosition = level.GetGridPosition(transform.position);
            level.AddUnitAtPosition(GridPosition, this);
            transform.position = level.GetWorldPosition(GridPosition);

            UnitActionSystem.Instance.AddUnit(this);
            SetSelected(UnitActionSystem.Instance.SelectedUnit == this);

            _health.OnDead += OnDead;
        }

        private void Update()
        {
            var newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
            if (newGridPosition == GridPosition)
            {
                return;
            }

            LevelGrid.Instance.UnitMoved(this, GridPosition, newGridPosition);
            GridPosition = newGridPosition;
        }

        private void SpendActionPoints(int amount)
        {
            ActionPoints -= amount;
        }

        private void OnDead()
        {
            LevelGrid.Instance.RemoveUnitAtPosition(GridPosition, this);
            UnitActionSystem.Instance.RemoveUnit(this);
            Destroy(gameObject);
        }

        public void SetSelected(bool selected)
        {
            selectionVisual.SetActive(selected);
        }

        public bool TrySpendActionPointToTakeAction(BaseAction action)
        {
            if (!CanSpendActionPointToTakeAction(action))
            {
                return false;
            }

            SpendActionPoints(action.GetActionPointsCost());
            return true;
        }

        public bool CanSpendActionPointToTakeAction(BaseAction action)
        {
            return ActionPoints >= action.GetActionPointsCost();
        }

        public void StartNewTurn(bool isPlayerTurn)
        {
            if (isPlayerTurn && !isEnemy || !isPlayerTurn && isEnemy)
            {
                ActionPoints = DefaultActionPoints;
            }
        }

        public void TakeDamage(int damage)
        {
            _health.TakeDamage(damage);
        }

        public override string ToString() => $"Unit {gameObject.name}";
    }
}
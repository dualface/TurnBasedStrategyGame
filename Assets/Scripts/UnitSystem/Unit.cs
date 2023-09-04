using System;
using Grid;
using UnitAction;
using UnityEngine;
using UnityEngine.Serialization;

namespace UnitSystem
{
    public class Unit : MonoBehaviour
    {
        private const int DefaultActionPoints = 2;

        [SerializeField]
        private GameObject selectionVisual;

        [SerializeField]
        private bool isEnemy;

        [SerializeField]
        public BaseAction defaultAction;

        public int ActionPoints { get; private set; } = DefaultActionPoints;

        public GridPosition GridPosition { get; private set; }

        public Vector3 WorldPosition => transform.position;


        public BaseAction[] Actions { get; private set; }

        public bool IsEnemy => isEnemy;

        public UnitHealth Health { get; private set; }

        private void Awake()
        {
            Actions = GetComponents<BaseAction>();
            Health = GetComponent<UnitHealth>();
        }

        private void Start()
        {
            var level = LevelGrid.Instance;
            GridPosition = level.GetGridPosition(transform.position);
            level.AddUnitAtPosition(GridPosition, this);
            transform.position = level.GetWorldPosition(GridPosition);

            UnitActionSystem.Instance.AddUnit(this);
            SetSelected(UnitActionSystem.Instance.SelectedUnit == this);

            Health.OnDead += OnDead;
        }

        private void Update()
        {
            var newPosition = LevelGrid.Instance.GetGridPosition(transform.position);
            if (newPosition == GridPosition)
            {
                return;
            }

            LevelGrid.Instance.UnitMoved(this, GridPosition, newPosition);
            GridPosition = newPosition;
        }

        public event Action OnActionPointsChanged;

        private void SpendActionPoints(int amount)
        {
            ActionPoints -= amount;
            OnActionPointsChanged?.Invoke();
        }

        private void OnDead()
        {
            LevelGrid.Instance.RemoveUnitAtPosition(GridPosition, this);
            UnitActionSystem.Instance.RemoveUnit(this);
            Destroy(gameObject);
        }

        public void SetSelected(bool selected) { selectionVisual.SetActive(selected); }

        public bool TrySpendActionPointToTakeAction(BaseAction action)
        {
            if (!CanSpendActionPointToTakeAction(action))
            {
                return false;
            }

            SpendActionPoints(action.GetActionPointsCost());
            return true;
        }

        public bool CanSpendActionPointToTakeAction(BaseAction action) => ActionPoints >= action.GetActionPointsCost();

        public void StartNewTurn(bool isPlayerTurn)
        {
            if ((!isPlayerTurn || isEnemy) && (isPlayerTurn || !isEnemy))
            {
                return;
            }

            ActionPoints = DefaultActionPoints;
            OnActionPointsChanged?.Invoke();
        }

        public void TakeDamage(int damage) { Health.TakeDamage(damage); }

        public override string ToString() => $"Unit {gameObject.name}";
    }
}

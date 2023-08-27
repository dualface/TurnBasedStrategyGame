using System.Collections.Generic;
using Grid;
using UnityEngine;
using UnitSystem;

namespace Grid
{
    public class LevelGrid : MonoBehaviour
    {
        [SerializeField]
        private int gridRows = 10;

        [SerializeField]
        private int gridColumns = 10;

        [SerializeField]
        private float cellSize = 2.0f;

        [SerializeField]
        private GridSystemVisual gridVisual;

        [SerializeField]
        private GameObject debugRenderer;

        [SerializeField]
        private GameObject gridDebugObjectPrefab;

        private GridSystem _gridSystem;

        public static LevelGrid Instance { get; private set; }

        private void Awake()
        {
            if (Instance)
            {
                Debug.LogError("There's more than one LevelGrid in the scene! " + transform + " - " + Instance);
                Destroy(gameObject);
            }

            Instance = this;
            _gridSystem = new GridSystem(gridRows, gridColumns, cellSize);
            _gridSystem.CreateDebugObjects(gridDebugObjectPrefab, debugRenderer.transform);
        }

        private void Start()
        {
            gridVisual.CreateGridVisual(_gridSystem);
            gridVisual.HideAll();
        }

        public void AddUnitAtPosition(GridPosition position, Unit unit)
        {
            _gridSystem.GetGridObject(position).AddUnit(unit);
        }

        public List<Unit> GetUnitListAtPosition(GridPosition position) => _gridSystem.GetGridObject(position).GetUnitList();

        public void RemoveUnitAtPosition(GridPosition position, Unit unit)
        {
            _gridSystem.GetGridObject(position).RemoveUnit(unit);
        }

        public void UnitMoved(Unit unit, GridPosition from, GridPosition to)
        {
            RemoveUnitAtPosition(from, unit);
            AddUnitAtPosition(to, unit);
        }

        public GridPosition GetGridPosition(Vector3 position) => _gridSystem.GetGridPosition(position);

        public Vector3 GetWorldPosition(GridPosition position) => _gridSystem.GetWorldPosition(position);

        public bool IsValidGridPosition(GridPosition position) => _gridSystem.IsValidGridPosition(position);

        public bool HasAnyUnitAtGridPosition(GridPosition position) => _gridSystem.GetGridObject(position).HasAnyUnit();

        public int Rows => _gridSystem.Rows;

        public int Columns => _gridSystem.Columns;

        public GridSystemVisual GetVisual() => gridVisual;
    }
}

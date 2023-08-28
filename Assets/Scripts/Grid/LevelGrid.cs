using System.Collections.Generic;
using UnitSystem;
using UnityEngine;

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

        public int Rows => _gridSystem.Rows;

        public int Columns => _gridSystem.Columns;

        public GridSystemVisual GridVisual => gridVisual;

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

        public void AddUnitAtPosition(GridPosition p, Unit unit) { _gridSystem.GetGridObject(p).AddUnit(unit); }

        public List<Unit> GetUnitListAtPosition(GridPosition p) => _gridSystem.GetGridObject(p).UnitList;

        public void RemoveUnitAtPosition(GridPosition p, Unit unit) { _gridSystem.GetGridObject(p).RemoveUnit(unit); }

        public void UnitMoved(Unit unit, GridPosition from, GridPosition to)
        {
            RemoveUnitAtPosition(from, unit);
            AddUnitAtPosition(to, unit);
        }

        public GridPosition GetGridPosition(Vector3 p) => _gridSystem.GetGridPosition(p);

        public Vector3 GetWorldPosition(GridPosition p) => _gridSystem.GetWorldPosition(p);

        public bool IsValidGridPosition(GridPosition p) => _gridSystem.IsValidGridPosition(p);

        public bool HasAnyUnitAtGridPosition(GridPosition p) => _gridSystem.GetGridObject(p).HasAnyUnit();
    }
}
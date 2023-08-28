using UnityEngine;

namespace Grid
{
    public class GridSystem
    {
        private readonly GridObject[,] _objectMatrix;

        public GridSystem(int rows, int columns, float cellSize)
        {
            Rows = rows;
            Columns = columns;
            CellSize = cellSize;

            _objectMatrix = new GridObject[rows, columns];
            for (var r = 0; r < Rows; r++)
            {
                for (var c = 0; c < Columns; c++)
                {
                    _objectMatrix[r, c] = new GridObject(new GridPosition(r, c));
                }
            }
        }

        public int Rows { get; }

        public int Columns { get; }

        public float CellSize { get; }

        public GridPosition GetGridPosition(Vector3 p) => new(row: Mathf.RoundToInt(p.z / CellSize), column: Mathf.RoundToInt(p.x / CellSize));

        public GridObject GetGridObject(GridPosition p) => _objectMatrix[p.Row, p.Column];

        public Vector3 GetWorldPosition(GridPosition p) => new(x: p.Column * CellSize, 0, z: p.Row * CellSize);

        public bool IsValidGridPosition(GridPosition p) => p.Row >= 0 && p.Row < Rows && p.Column >= 0 && p.Column < Columns;

        public void CreateDebugObjects(GameObject debugPrefab, Transform renderer)
        {
            for (var r = 0; r < Rows; r++)
            {
                for (var c = 0; c < Columns; c++)
                {
                    var p = new GridPosition(r, c);
                    var o = Object.Instantiate(debugPrefab, position: GetWorldPosition(p), Quaternion.identity, renderer);
                    var debug = o.GetComponent<GridDebugObject>();
                    debug.SetGridObject(GetGridObject(p));
                }
            }
        }
    }
}
using UnityEngine;

namespace Grid
{
    public class GridSystem
    {
        public int Rows { get; private set; }

        public int Columns { get; private set; }

        public float CellSize { get; private set; }

        private readonly GridObject[,] _objects;

        public GridSystem(int rows, int columns, float cellSize)
        {
            Rows = rows;
            Columns = columns;
            CellSize = cellSize;
            _objects = new GridObject[rows, columns];
            for (var row = 0; row < Rows; row++)
            {
                for (var column = 0; column < Columns; column++)
                {
                    _objects[row, column] = new GridObject(new GridPosition(row, column));
                }
            }
        }

        public GridPosition GetGridPosition(Vector3 position) =>
            new(Mathf.RoundToInt(position.z / CellSize), Mathf.RoundToInt(position.x / CellSize));

        public GridObject GetGridObject(GridPosition position) => _objects[position.Row, position.Column];

        public Vector3 GetWorldPosition(GridPosition position) =>
            new(position.Column * CellSize, 0, position.Row * CellSize);

        public bool IsValidGridPosition(GridPosition position) =>
            position.Row >= 0 && position.Row < Rows && position.Column >= 0 && position.Column < Columns;

        public void CreateDebugObjects(GameObject gridDebugObjectPrefab, Transform renderer)
        {
            for (var row = 0; row < Rows; row++)
            {
                for (var column = 0; column < Columns; column++)
                {
                    var pos = new GridPosition(row, column);
                    var obj = Object.Instantiate(
                        gridDebugObjectPrefab,
                        GetWorldPosition(pos),
                        Quaternion.identity,
                        renderer
                    );
                    var debug = obj.GetComponent<GridDebugObject>();
                    debug.SetGridObject(GetGridObject(pos));
                }
            }
        }
    }
}

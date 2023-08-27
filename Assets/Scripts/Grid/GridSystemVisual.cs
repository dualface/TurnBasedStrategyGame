using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
    public class GridSystemVisual : MonoBehaviour
    {
        [SerializeField]
        private GameObject gridCellVisualPrefab;

        private GridSystem _grid;
        private GridSystemCellVisual[,] _cellArray;

        public void CreateGridVisual(GridSystem grid)
        {
            _grid = grid;
            _cellArray = new GridSystemCellVisual[grid.Rows, grid.Columns];

            for (int row = 0; row < grid.Rows; row++)
            {
                for (int column = 0; column < grid.Columns; column++)
                {
                    Vector3 spawnPosition = grid.GetWorldPosition(new GridPosition(row, column));
                    GameObject visualNode = Instantiate(gridCellVisualPrefab, spawnPosition, Quaternion.identity);
                    visualNode.transform.parent = transform;

                    _cellArray[row, column] = visualNode.GetComponent<GridSystemCellVisual>();
                }
            }
        }

        public void HideAll()
        {
            for (var row = 0; row < _grid.Rows; row++)
            {
                for (var column = 0; column < _grid.Columns; column++)
                {
                    _cellArray[row, column].Hide();
                }
            }
        }

        public void ShowPositionList(List<GridPosition> positionList)
        {
            foreach (var position in positionList)
            {
                _cellArray[position.Row, position.Column].Show();
            }
        }
    }
}

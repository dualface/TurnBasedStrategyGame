using System;
using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
    public class GridSystemVisual : MonoBehaviour
    {
        public enum CellColor
        {
            White,
            Red,
            RedSoft,
            Green,
            Blue,
            Orange,
            Gray,
        }

        [Serializable]
        public struct CellColorMaterial
        {
            public CellColor color;
            public Material material;
        }

        [SerializeField]
        private GameObject gridCellVisualPrefab;

        [SerializeField]
        private List<CellColorMaterial> cellColorMaterials;

        private GridSystem _grid;
        private GridSystemCellVisual[,] _cellArray;

        private Material GetMaterialByColor(CellColor color)
        {
            foreach (var m in cellColorMaterials)
            {
                if (m.color == color)
                {
                    return m.material;
                }
            }

            Debug.LogError($"Material for color {color} not found");

            return null;
        }

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

        public void ShowPositionList(List<GridPosition> positionList, CellColor color)
        {
            var material = GetMaterialByColor(color);
            foreach (var position in positionList)
            {
                _cellArray[position.Row, position.Column].Show(material);
            }
        }
    }
}

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
            Gray
        }

        [SerializeField]
        private GameObject gridCellVisualPrefab;

        [SerializeField]
        private List<CellColorMaterial> cellColorMaterials;

        private GridSystemCellVisual[,] _cellMatrix;

        private GridSystem _grid;

        public void CreateGridVisual(GridSystem grid)
        {
            _grid = grid;
            _cellMatrix = new GridSystemCellVisual[grid.Rows, grid.Columns];

            for (var r = 0; r < grid.Rows; r++)
            {
                for (var c = 0; c < grid.Columns; c++)
                {
                    var p = grid.GetWorldPosition(new GridPosition(r, c));
                    var o = Instantiate(gridCellVisualPrefab, p, Quaternion.identity);
                    o.transform.parent = transform;
                    _cellMatrix[r, c] = o.GetComponent<GridSystemCellVisual>();
                }
            }
        }

        public void HideAll()
        {
            for (var r = 0; r < _grid.Rows; r++)
            {
                for (var c = 0; c < _grid.Columns; c++)
                {
                    _cellMatrix[r, c].Hide();
                }
            }
        }

        public void ShowPositions(List<GridPosition> positions, CellColor color)
        {
            var m = GetMaterialByColor(color);
            foreach (var p in positions)
            {
                _cellMatrix[p.Row, p.Column].Show(m);
            }
        }

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

        [Serializable]
        public struct CellColorMaterial
        {
            public CellColor color;
            public Material material;
        }
    }
}
using TMPro;
using UnityEngine;

namespace Grid
{
    public class GridDebugObject : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text label;

        public void SetGridObject(GridObject gridObject)
        {
            label.text = gridObject.ToString();
            gridObject.OnCellChanged += OnGridObjectChanged;
        }

        private void OnGridObjectChanged(GridObject gridObject) { label.text = gridObject.ToString(); }
    }
}
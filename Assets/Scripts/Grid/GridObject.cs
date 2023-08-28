using System;
using System.Collections.Generic;
using UnitSystem;

namespace Grid
{
    public class GridObject
    {
        public event Action<GridObject> OnCellChanged;

        public List<Unit> UnitList { get; } = new();

        public GridObject(GridPosition position) { Position = position; }

        public GridPosition Position { get; }

        public void AddUnit(Unit unit)
        {
            UnitList.Add(unit);
            OnChanged();
        }

        public bool HasAnyUnit() => UnitList.Count > 0;

        public void RemoveUnit(Unit unit)
        {
            UnitList.Remove(unit);
            OnChanged();
        }

        public override string ToString()
        {
            var text = $"({Position.Row}, {Position.Column})";
            if (UnitList.Count <= 0)
            {
                return text;
            }

            foreach (var unit in UnitList)
            {
                text += $"\n{unit}";
            }

            return text;
        }

        private void OnChanged() { OnCellChanged?.Invoke(this); }
    }
}

using System;
using System.Collections.Generic;
using UnitSystem;

namespace Grid
{
    public class GridObject
    {
        public event EventHandler OnCellChanged;

        public GridPosition Position { get; private set; }

        private readonly List<Unit> _unitList = new();

        public GridObject(GridPosition position)
        {
            Position = position;
        }

        public void AddUnit(Unit unit)
        {
            _unitList.Add(unit);
            OnChanged();
        }

        public List<Unit> GetUnitList() => _unitList;

        public bool HasAnyUnit() => _unitList.Count > 0;

        public void RemoveUnit(Unit unit)
        {
            _unitList.Remove(unit);
            OnChanged();
        }

        public override string ToString()
        {
            var text = $"({Position.Row}, {Position.Column})";
            if (_unitList.Count <= 0)
            {
                return text;
            }

            foreach (var unit in _unitList)
            {
                text += $"\n{unit}";
            }
            return text;
        }

        private void OnChanged()
        {
            OnCellChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}

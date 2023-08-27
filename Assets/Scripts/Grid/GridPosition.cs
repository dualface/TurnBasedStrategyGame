using System;

namespace Grid
{
    public readonly struct GridPosition : IEquatable<GridPosition>
    {
        public readonly int Row;
        public readonly int Column;

        public GridPosition(int row = 0, int column = 0)
        {
            Row = row;
            Column = column;
        }

        public override bool Equals(object obj) => obj is GridPosition pos && Row == pos.Row && Column == pos.Column;

        public override int GetHashCode() => HashCode.Combine(Row, Column);

        public bool Equals(GridPosition other) => Row == other.Row && Column == other.Column;

        public static bool operator ==(GridPosition a, GridPosition b) => a.Row == b.Row && a.Column == b.Column;

        public static bool operator !=(GridPosition a, GridPosition b) => a.Row != b.Row || a.Column != b.Column;

        public static GridPosition operator +(GridPosition a, GridPosition b) => new(a.Row + b.Row, a.Column + b.Column);

        public static GridPosition operator -(GridPosition a, GridPosition b) => new(a.Row - b.Row, a.Column - b.Column);

        public override string ToString() => $"(row:{Row}, column:{Column})";
    }
}

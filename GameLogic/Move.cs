using Core.Games;

namespace GameLogic
{
    public class Move : IMove
    {
        public Direction Direction { get; set; }

        public Move(Direction direction)
        {
            Direction = direction;
        }

        public override bool Equals(object obj)
        {
            return obj is Move move && Direction == move.Direction;
        }

        public override int GetHashCode()
        {
            return 997 + 101 * Direction.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("({0})", Direction);
        }
    }
}

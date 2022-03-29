
namespace Components
{
    public enum Direction
    {
        Stopped,
        Up,
        Down,
        Left,
        Right
    }

    public class Movable : Component
    {
        public Direction facing;
        public uint segmentsToAdd = 0;
        public uint moveInterval { get; private set; }
        public uint elapsedInterval = 0;

        public Movable(Direction facing, uint moveInterval)
        {
            this.facing = facing;
            this.moveInterval = moveInterval;
        }
    }
}

using Microsoft.Xna.Framework;
using System;

namespace Systems
{
    /// <summary>
    /// This system is responsible for handling the movement of any
    /// entity with a movable & position components.
    /// </summary>
    class Movement : System
    {
        public Movement()
            : base(
                  typeof(Components.Movable),
                  typeof(Components.Position)
                  )
        {
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var entity in m_entities.Values)
            {
                moveEntity(entity, gameTime);
            }
        }

        private void moveEntity(Entities.Entity entity, GameTime gameTime)
        {
            var movable = entity.GetComponent<Components.Movable>();
            movable.elapsedInterval += (uint)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (movable.elapsedInterval >= movable.moveInterval)
            {
                movable.elapsedInterval -= movable.moveInterval;
                switch (movable.facing)
                {
                    case Components.Direction.Up:
                        move(entity, 0, -1);
                        break;
                    case Components.Direction.Down:
                        move(entity, 0, 1);
                        break;
                    case Components.Direction.Left:
                        move(entity, -1, 0);
                        break;
                    case Components.Direction.Right:
                        move(entity, 1, 0);
                        break;
                }
            }
        }

        private void move(Entities.Entity entity, int xIncrement, int yIncrement)
        {
            var movable = entity.GetComponent<Components.Movable>();
            var position = entity.GetComponent<Components.Position>();

            //
            // Remember current front position, so it can be added back in as the move
            var front = position.segments[0];

            //
            // Remove the tail, but only if there aren't new segments to add
            if (movable.segmentsToAdd == 0 && position.segments.Count > 0)
            {
                position.segments.RemoveAt(position.segments.Count - 1);
            }
            else
            {
                movable.segmentsToAdd--;
            }

            //
            // Update the front of the entity with the segment moving into the new spot
            Point newFront = new Point(front.X + xIncrement, front.Y + yIncrement);
            position.segments.Insert(0, newFront);
        }
    }
}

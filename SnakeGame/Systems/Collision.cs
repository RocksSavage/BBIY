using Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Systems
{
    public class Collision : System
    {
        private Action<Entity> m_foodConsumed;
        public Collision(Action<Entity> foodConsumed)
            : base(
                  typeof(Components.Position)
                  )
        {
            this.m_foodConsumed = foodConsumed;
        }

        /// <summary>
        /// Check to see if any movable components collide with any other
        /// collision components.
        ///
        /// Step 1: find all movable components first
        /// Step 2: Test the movable components for collision with other (but not self) collision components
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            var movable = findMovable(m_entities);

            foreach (var entity in m_entities.Values)
            {
                foreach (var entityMovable in movable)
                {
                    if (collides(entity, entityMovable))
                    {
                        //
                        // If food, that's okay
                        if (entity.ContainsComponent<Components.Food>())
                        {
                            entityMovable.GetComponent<Components.Movable>().segmentsToAdd = 3;
                            m_foodConsumed(entity);
                        }
                        else
                        {
                            entityMovable.GetComponent<Components.Movable>().facing = Components.Direction.Stopped;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Public interface that allows an entity with a single cell position
        /// to be tested for collision with anything else in the game.
        /// </summary>
        public bool collidesWithAny(Entity proposed)
        {
            var aPosition = proposed.GetComponent<Components.Position>();

            foreach (var entity in m_entities.Values)
            {
                if (entity.ContainsComponent<Components.Collision>() && entity.ContainsComponent<Components.Position>())
                {
                    var ePosition = entity.GetComponent<Components.Position>();

                    foreach (var segment in ePosition.segments)
                    {
                        if (aPosition.x == segment.X && aPosition.y == segment.Y)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Returns a collection of all the movable entities.
        /// </summary>
        private List<Entity> findMovable(Dictionary<uint, Entity> entities)
        {
            var movable = new List<Entity>();

            foreach (var entity in m_entities.Values)
            {
                if (entity.ContainsComponent<Components.Movable>() && entity.ContainsComponent<Components.Position>())
                {
                    movable.Add(entity);
                }
            }

            return movable;
        }
        /// <summary>
        /// We know that only the snake is moving and that we only need
        /// to check its head for collision with other entities.  Therefore,
        /// don't need to look at all the segments in the position, with the
        /// exception of the movable itself...a movable can collide with itself.
        /// </summary>
        private bool collides(Entity a, Entity b)
        {
            var aPosition = a.GetComponent<Components.Position>();
            var bPosition = b.GetComponent<Components.Position>();

            //
            // A movable can collide with itself: Check segment against the rest
            if (a == b)
            {
                //
                // Have to skip the first segment, that's why using a counted for loop
                for (int segment = 1; segment < aPosition.segments.Count; segment++)
                {
                    if (aPosition.x == aPosition.segments[segment].X && aPosition.y == aPosition.segments[segment].Y)
                    {
                        return true;
                    }
                }

                return false;
            }

            return aPosition.x == bPosition.x && aPosition.y == bPosition.y;
        }
    }
}

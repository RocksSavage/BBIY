using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace Systems
{
    /// <summary>
    /// This system knows how to accept keyboard input and use that
    /// to move an entity, based on the entities 'KeyboardControlled'
    /// component settings.
    /// </summary>
    class KeyboardInput : System
    {
        public KeyboardInput()
            : base(typeof(Components.KeyboardControlled))
        {
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var entity in m_entities.Values)
            {
                var movable = entity.GetComponent<Components.Movable>();
                var input = entity.GetComponent<Components.KeyboardControlled>();

                foreach (var key in Keyboard.GetState().GetPressedKeys())
                {
                    if (input.keys.ContainsKey(key))
                    {
                        bool canTurn = true;
                        // Protect agains turning back onto itself
                        // BUG: Note the Keys are hardcoded here and if they are changed to
                        //      something else in the game model when the snake entity is created
                        //      those keys won't be recognized here.
                        if ((movable.facing == Components.Direction.Up) && key == Keys.Down)
                        {
                            canTurn = false;
                        }
                        if ((movable.facing == Components.Direction.Down) && key == Keys.Up)
                        {
                            canTurn = false;
                        }
                        if ((movable.facing == Components.Direction.Left) && key == Keys.Right)
                        {
                            canTurn = false;
                        }
                        if ((movable.facing == Components.Direction.Right) && key == Keys.Left)
                        {
                            canTurn = false;
                        }

                        if (canTurn)
                        {
                            movable.facing = input.keys[key];
                        }
                    }
                }
            }
        }
    }
}

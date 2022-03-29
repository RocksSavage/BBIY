
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Components
{
    class KeyboardControlled : Component
    {
        public Dictionary<Keys, Components.Direction> keys;

        public KeyboardControlled(Dictionary<Keys, Components.Direction> keys)
        {
            this.keys = keys;
        }
    }
}

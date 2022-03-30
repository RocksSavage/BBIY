﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
// using CS5410.Input;


namespace CS5410
{
    public interface IGameState
    {
        void initializeSession();
        void initialize(GraphicsDevice graphicsDevice, GraphicsDeviceManager graphics, List<Keys> controls);
        void loadContent(ContentManager contentManager);
        GameStateEnum processInput(GameTime gameTime);
        void update(GameTime gameTime);
        void render(GameTime gameTime);
    }
}
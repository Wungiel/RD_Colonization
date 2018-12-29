using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Screens;
using System;

namespace RD_Colonization.Code
{
    public abstract class DefaultScreen : Screen
    {
        protected DefaultScreen(Game game)
        {
            Game = game;
        }

        public Game Game { get; }
        public ContentManager Content => Game.Content;
        public GraphicsDevice GraphicsDevice => Game.GraphicsDevice;
        public GameServiceContainer Services => Game.Services;

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            InputManager.updateState(Keyboard.GetState(), Mouse.GetState());
        }
    }
}
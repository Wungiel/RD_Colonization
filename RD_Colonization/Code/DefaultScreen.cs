using GeonBit.UI;
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

        SpriteBatch spriteBatch;

        protected DefaultScreen(Game game)
        {
            Game = game;
        }

        public Game Game { get; }
        public ContentManager Content => Game.Content;
        public GraphicsDevice GraphicsDevice => Game.GraphicsDevice;
        public GameServiceContainer Services => Game.Services;
        private bool runGame = false;

        public override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            runGame = true;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            UserInterface.Active.Update(gameTime);
            InputManager.updateState(Keyboard.GetState(), Mouse.GetState());
        }

        public override void Draw(GameTime gameTime)
        {
            if (runGame)
            {
                UserInterface.Active.Draw(spriteBatch);
            }
            base.Draw(gameTime);
        }
    }
}
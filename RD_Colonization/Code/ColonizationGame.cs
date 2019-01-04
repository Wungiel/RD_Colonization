using GeonBit.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Screens;
using MonoGame.Extended.ViewportAdapters;
using RD_Colonization.Code;
using System;

namespace RD_Colonization
{
    public class ColonizationGame : Game
    {
        public GraphicsDeviceManager graphics { get; }

        public ColonizationGame()
        {
            Content.RootDirectory = "Content";
            IsFixedTimeStep = true;
            TargetElapsedTime = TimeSpan.FromMilliseconds(1000.0f / 60);


            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 600;
            graphics.PreferredBackBufferWidth = 800;
            graphics.SynchronizeWithVerticalRetrace = false;
            

            Window.AllowUserResizing = false;
            Window.Title = "RD's Colonization";

            ScreenGameComponent screenGameComponent = new ScreenGameComponent(this);
            screenGameComponent.Register(new MainMenuScreen(this, graphics));
            screenGameComponent.Register(new GameScreen(this, graphics));
            Components.Add(screenGameComponent);

        }

        protected override void Initialize()
        {
            UserInterface.Initialize(Content,BuiltinThemes.hd);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }


        protected override void Update(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}

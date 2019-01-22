using GeonBit.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RD_Colonization.Code;
using RD_Colonization.Code.Managers;
using RD_Colonization.Code.Screens;
using System;
using System.Diagnostics;
using static RD_Colonization.Code.StringList;

namespace RD_Colonization
{
    public class ColonizationGame : Game
    {
        public GraphicsDeviceManager graphics { get; }
        public SpriteBatch spriteBatch;

        public ColonizationGame()
        {
            Content.RootDirectory = "Content";
            IsFixedTimeStep = true;
            TargetElapsedTime = TimeSpan.FromMilliseconds(1000.0f / 60);

            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 600;
            graphics.PreferredBackBufferWidth = 800;
            graphics.SynchronizeWithVerticalRetrace = false;
            graphics.ApplyChanges();

            Window.AllowUserResizing = false;
            Window.Title = "RD's Colonization";
        }

        protected override void Initialize()
        {
            UserInterface.Initialize(Content, BuiltinThemes.hd);
            ScreenManager.registerScreen(mainMenuScreenString, new MainMenuScreen(this));
            ScreenManager.registerScreen(gameSetUpScreenString, new GameSetUpScreen(this));
            ScreenManager.registerScreen(gameScreenString, new GameScreen(this));
            ScreenManager.initialize();
            ScreenManager.setScreen(mainMenuScreenString);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            ScreenManager.loadContent();
            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            ScreenManager.activeScreen.Update(gameTime);
            //if (InputManager.isSinglePress(Keys.Space))
            //{
            //    graphics.ToggleFullScreen();
            //}
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            ScreenManager.activeScreen.Draw();
        }
    }
}
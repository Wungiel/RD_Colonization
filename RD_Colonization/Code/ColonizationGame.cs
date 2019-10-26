using GeonBit.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RD_Colonization.Code;
using RD_Colonization.Code.Managers;
using RD_Colonization.Code.Screens;
using System;
using static RD_Colonization.Code.StringList;

namespace RD_Colonization
{
    public class ColonizationGame : Game
    {
        public GraphicsDeviceManager Graphics { get; }
        public SpriteBatch spriteBatch;

        public ColonizationGame()
        {
            Content.RootDirectory = "Content";
            IsFixedTimeStep = true;
            TargetElapsedTime = TimeSpan.FromMilliseconds(1000.0f / 60);

            Graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferHeight = 600,
                PreferredBackBufferWidth = 800,
                SynchronizeWithVerticalRetrace = false
            };
            Graphics.ApplyChanges();

            Window.AllowUserResizing = false;
            Window.Title = "RD's Colonization";
        }

        protected override void Initialize()
        {
            UserInterface.Initialize(Content, BuiltinThemes.hd);
            ScreenManager manager = ScreenManager.Instance;
            manager.RegisterScreen(mainMenuScreenString, new MainMenuScreen(this));
            manager.RegisterScreen(gameSetUpScreenString, new GameSetUpScreen(this));
            manager.RegisterScreen(gameScreenString, new GameScreen(this));
            manager.Initialize();
            manager.SetScreen(mainMenuScreenString);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            ScreenManager.Instance.LoadContent();
            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            if (IsActive)
            {
                base.Update(gameTime);
                ScreenManager.Instance.activeScreen.Update(gameTime);
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            ScreenManager.Instance.activeScreen.Draw();
        }
    }
}
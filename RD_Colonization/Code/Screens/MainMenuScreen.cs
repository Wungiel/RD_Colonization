using GeonBit.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.ViewportAdapters;
using RD_Colonization.Code;
using System;
using System.Diagnostics;

namespace RD_Colonization.Code
{
    public class MainMenuScreen : DefaultScreen
    {

        private SpriteBatch spriteBatch;
        private Texture2D background;
        private SpriteFont font;

        public MainMenuScreen(Game game, GraphicsDeviceManager graphics) : base(game)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

        }

        public override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            background = this.Content.Load<Texture2D>("Images\\MainMenuArt");
            font = this.Content.Load<SpriteFont>("Font\\MainFont");
            
        }

        public override void Update(GameTime gameTime)
        {
            if (InputManager.isSinglePress(Keys.Space))
                Show<GameScreen>();

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Red);
            spriteBatch.Begin();
            spriteBatch.Draw(background, new Rectangle(0, 0, background.Width, background.Height), Color.White);
            spriteBatch.End();
            UserInterface.Active.Draw(spriteBatch);
            base.Draw(gameTime);

        }


    }
}
using GeonBit.UI;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.ViewportAdapters;
using RD_Colonization.Code;
using RD_Colonization.Code.Managers;
using System;
using System.Diagnostics;

namespace RD_Colonization.Code
{
    public class MainMenuScreen : DefaultScreen
    {
        
        private Texture2D background;

        public MainMenuScreen(ColonizationGame game) : base(game)
        {
        }

        public override void Initialize()
        {
        }


        public override void LoadContent()
        {
            background = this.Content.Load<Texture2D>("Images\\MainMenuArt");
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (InputManager.isSinglePress(Keys.Space))
                ScreenManager.setScreen("game");
        }

        public override void Draw()
        {
            GraphicsDevice.Clear(Color.Red);
            spriteBatch.Begin();
            spriteBatch.Draw(background, new Rectangle(0, 0, background.Width, background.Height), Color.White);
            spriteBatch.End();
            UserInterface.Active.Draw(spriteBatch);
        }
    }
}
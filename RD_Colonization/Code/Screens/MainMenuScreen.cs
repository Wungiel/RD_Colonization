using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RD_Colonization.Code;
using System;

namespace RD_Colonization.Code
{
    public class MainMenuScreen : DefaultScreen
    {

        SpriteBatch spriteBatch;
        Texture2D background;

        public MainMenuScreen(Game game) : base(game)
        {
        }

        public override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            background = this.Content.Load<Texture2D>("Images\\MainMenuArt");

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
            base.Draw(gameTime);

        }


    }
}
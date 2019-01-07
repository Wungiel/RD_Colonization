using GeonBit.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RD_Colonization.Code;

namespace RD_Colonization
{
    public class GameScreen : DefaultScreen
    {
        public GameScreen(ColonizationGame game) : base(game)
        {
        }

        public override void LoadContent()
        {
        }

        public override void Initialize()
        {
        }

        public override void LoadScreen()
        {
        }

        public override void UnloadScreen()
        {
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw()
        {
            GraphicsDevice.Clear(Color.Green);
            spriteBatch.Begin();
            spriteBatch.End();
            UserInterface.Active.Draw(spriteBatch);
        }
    }
}
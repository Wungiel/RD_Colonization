using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeonBit.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using RD_Colonization.Code.Managers;
using static RD_Colonization.Code.StringList;

namespace RD_Colonization.Code.Screens
{
    class GameSetUpScreen : DefaultScreen
    {

        public GameSetUpScreen(ColonizationGame game) : base(game)
        {
        }

        public override void Initialize()
        {
        }        

        public override void LoadContent()
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
            if (InputManager.isSinglePress(Keys.Space))
                ScreenManager.setScreen(mainMenuScreenString);
            base.Update(gameTime);
        }

        public override void Draw()
        {
            GraphicsDevice.Clear(Color.SaddleBrown);
            spriteBatch.Begin();
            spriteBatch.End();
            UserInterface.Active.Draw(spriteBatch);
        }

    }
}

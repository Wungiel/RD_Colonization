using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RD_Colonization.Code;
using System;
using MonoGame.Extended;
using System.Diagnostics;
using GeonBit.UI;
using GeonBit.UI.Entities;
using RD_Colonization.Code.Managers;
using static RD_Colonization.Code.StringList;

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
            UserInterface.Active.Update(gameTime);
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
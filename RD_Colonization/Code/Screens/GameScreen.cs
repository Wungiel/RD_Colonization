using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RD_Colonization.Code;
using System;
using MonoGame.Extended;
using System.Diagnostics;
using GeonBit.UI;

namespace RD_Colonization
{
    public class GameScreen : DefaultScreen
    {

        SpriteBatch spriteBatch;


        public GameScreen(Game game, GraphicsDeviceManager graphics) : base(game)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);            
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Green);
            spriteBatch.Begin();            
            spriteBatch.End();
            base.Draw(gameTime);

        }

    }
}
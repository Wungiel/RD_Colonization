using GeonBit.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RD_Colonization.Code
{
    public abstract class DefaultScreen
    {
        protected ColonizationGame Game { get; }
        protected ContentManager Content => Game.Content;
        protected GraphicsDevice GraphicsDevice => Game.GraphicsDevice;
        protected GameServiceContainer Services => Game.Services;
        protected SpriteBatch SpriteBatch => Game.spriteBatch;
        protected GameWindow Window => Game.Window;
        protected GraphicsDeviceManager Graphics => Game.Graphics;

        protected DefaultScreen(ColonizationGame game)
        {
            Game = game;
        }

        public abstract void LoadContent();

        public abstract void Initialize();

        public abstract void Draw();

        public abstract void LoadScreen();

        public abstract void UnloadScreen();

        public virtual void Update(GameTime gameTime)
        {
            UserInterface.Active.Update(gameTime);
            InputManager.Instance.UpdateState(Keyboard.GetState(), Mouse.GetState());
        }
    }
}
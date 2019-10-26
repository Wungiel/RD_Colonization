using System.Diagnostics;
using System.IO;
using GeonBit.UI;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RD_Colonization.Code.Managers;
using static RD_Colonization.Code.StringList;

namespace RD_Colonization.Code
{
    public class MainMenuScreen : DefaultScreen
    {
        private Texture2D background;
        private Panel mainPanel;

        public MainMenuScreen(ColonizationGame game) : base(game)
        {
        }

        public override void Initialize()
        {
            PrepareGUI();
            mainPanel.Visible = false;
        }

        private void PrepareGUI()
        {
            mainPanel = new Panel(new Vector2(300, 500), PanelSkin.Default, Anchor.Center, new Vector2(10, 10));
            UserInterface.Active.AddEntity(mainPanel);

            Button newGameButton = new Button(newGameString);
            newGameButton.OnClick += (Entity entity) =>
            {
                ScreenManager.Instance.SetScreen(gameSetUpScreenString);
            };

            Button testGameButton = new Button(testGameString);
            testGameButton.OnClick += (Entity entity) =>
            {
                ScreenManager.Instance.SetScreen(testScreenString);
            };

            Button aboutButton = new Button(aboutString);
            aboutButton.OnClick += (Entity entity) =>
            {
                GeonBit.UI.Utils.MessageBox.ShowMsgBox("About", "Made by Robert Dulemba. \nUsed frameworks: Monogame, GeonBit.UI, \nPerlin Noise from https://gist.github.com/Flafla2/1a0b9ebef678bbce3215 ");
            };

            Button exitButton = new Button(exitString);
            exitButton.OnClick += (Entity entity) =>
            {
                Game.Exit();
            };

            mainPanel.Size = new Vector2(mainPanel.Size.X, newGameButton.Size.Y * 5);
            mainPanel.AddChild(newGameButton);
            mainPanel.AddChild(testGameButton);
            mainPanel.AddChild(aboutButton);
            mainPanel.AddChild(exitButton);
        }

        public override void LoadContent()
        {
            background = Content.Load<Texture2D>("Images\\MainMenuArt");
        }

        public override void LoadScreen()
        {
            mainPanel.Visible = true;
        }

        public override void UnloadScreen()
        {
            mainPanel.Visible = false;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw()
        {
            GraphicsDevice.Clear(Color.Red);
            SpriteBatch.Begin();
            SpriteBatch.Draw(background, new Rectangle(0, 0, background.Width, background.Height), Color.White);
            SpriteBatch.End();
            UserInterface.Active.Draw(SpriteBatch);
        }
    }
}
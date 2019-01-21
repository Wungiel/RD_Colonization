﻿using System;
using GeonBit.UI;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;
using RD_Colonization.Code;
using static RD_Colonization.Code.StringList;

namespace RD_Colonization
{
    public class GameScreen : DefaultScreen
    {

        private Camera2D camera;
        private MapDrawer mapDrawer;
        private Panel mainPanel, escapePanel;
        private bool isEscapeMenuActive = false;
        private const float movementSpeed = 200;

        public GameScreen(ColonizationGame game) : base(game)
        {
            mapDrawer = new MapDrawer();
        }

        public override void LoadContent()
        {
            Texture2D tileset = Content.Load<Texture2D>("Images\\TileSet");
            mapDrawer.setTileset(tileset);
        }

        public override void Initialize()
        {
            camera = new Camera2D(GraphicsDevice);
            PrepareGUI();
            mainPanel.Visible = false;

        }

        private void PrepareGUI()
        {
            mainPanel = new Panel(new Vector2(300, 100), PanelSkin.Default, Anchor.BottomLeft, new Vector2(10, 10));
            //UserInterface.Active.AddEntity(mainPanel);

            setEscapePanel();
            UserInterface.Active.AddEntity(escapePanel);
        }

        private void setEscapePanel()
        {
            escapePanel = new Panel(new Vector2(300, 300), PanelSkin.Default, Anchor.Center, new Vector2(10, 10));
            escapePanel.Visible = false;
            Button saveGame = new Button(saveGameString);
            saveGame.OnClick += (Entity entity) =>
            {
            };
            Button loadGame = new Button(loadGameString);
            loadGame.OnClick += (Entity entity) =>
            {
            };
            Button exit = new Button(exitString);
            exit.OnClick += (Entity entity) =>
            {
                Game.Exit();
            };
            escapePanel.AddChild(saveGame);
            escapePanel.AddChild(loadGame);
            escapePanel.AddChild(exit);
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
            var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (InputManager.isSinglePress(Keys.Escape))
            {
                escapePanel.Visible = !escapePanel.Visible;
                isEscapeMenuActive = !isEscapeMenuActive;   
            }
            if (!isEscapeMenuActive)
            {
                if (InputManager.IsKeyDown(Keys.Up))
                    camera.Move(new Vector2(0, -movementSpeed) * delta);
                if (InputManager.IsKeyDown(Keys.Down))
                    camera.Move(new Vector2(0, movementSpeed) * delta);
                if (InputManager.IsKeyDown(Keys.Left))
                    camera.Move(new Vector2(-movementSpeed, 0) * delta);
                if (InputManager.IsKeyDown(Keys.Right))
                    camera.Move(new Vector2(movementSpeed, 0) * delta);
            }
        }

        public override void Draw()
        {
            GraphicsDevice.Clear(Color.Green);
            mapDrawer.Draw(spriteBatch, camera);
            UserInterface.Active.Draw(spriteBatch);
        }
    }
}
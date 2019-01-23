using GeonBit.UI;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using RD_Colonization.Code;
using RD_Colonization.Code.Data;
using RD_Colonization.Code.Managers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using static RD_Colonization.Code.StringList;

namespace RD_Colonization
{
    public class GameScreen : DefaultScreen
    {
        private Camera2D camera;
        private MapDrawer mapDrawer;
        private List<Entity> rootEntities = new List<Entity>();
        private Panel escapePanel;
        private bool isEscapeMenuActive = false;
        private const float movementSpeed = 300;

        public GameScreen(ColonizationGame game) : base(game)
        {
            mapDrawer = new MapDrawer();
        }

        public override void LoadContent()
        {
            Texture2D mapTileset = Content.Load<Texture2D>("Images\\TileSet");
            Texture2D unitTileset = Content.Load<Texture2D>("Images\\UnitSet");
            mapDrawer.setTileset(mapTileset, unitTileset);
        }

        public override void Initialize()
        {
            camera = new Camera2D(GraphicsDevice);
            PrepareGUI();
            foreach(Entity e in rootEntities)
                e.Visible = false;
        }

        private void PrepareGUI()
        {
            Panel mainPanel = new Panel(new Vector2(500, 100), PanelSkin.Default, Anchor.BottomLeft, new Vector2(10, 10));

            Button nextTurn = new Button(nextTurnString, size: new Vector2(170,80),anchor: Anchor.CenterLeft);
            Paragraph turnCounter = new Paragraph(String.Format("Turn: {0}", 0), anchor: Anchor.Center);
            Paragraph cashCounter = new Paragraph(String.Format("Cash: {0}", 0), anchor: Anchor.CenterRight);
            nextTurn.OnClick += (Entity entity) =>
            {
                TurnManager.increaseTurn();
                turnCounter.Text = String.Format("Turn: {0}", TurnManager.turnNumber);
                cashCounter.Text = String.Format("Cash: {0}", CivilizationManager.cash);
            };
            mainPanel.AddChild(nextTurn);
            mainPanel.AddChild(turnCounter);
            mainPanel.AddChild(cashCounter);


            setEscapePanel();
            UserInterface.Active.AddEntity(mainPanel);
            UserInterface.Active.AddEntity(escapePanel);

            rootEntities.Add(mainPanel);
        }

        private void setEscapePanel()
        {
            escapePanel = new Panel(new Vector2(300, 300), PanelSkin.Default, Anchor.Center, new Vector2(10, 10));
            escapePanel.Visible = false;
            Button saveGame = new Button(saveGameString);
            saveGame.OnClick += (Entity entity) =>
            {
                DatabaseManager.removeData();
                DatabaseManager.saveData();
            };
            Button mainMenu = new Button(mainMenuString);
            mainMenu.OnClick += (Entity entity) =>
            {
                ScreenManager.setScreen(mainMenuScreenString);
            };
            Button exit = new Button(exitString);
            exit.OnClick += (Entity entity) =>
            {
                Game.Exit();
            };
            escapePanel.AddChild(saveGame);
            escapePanel.AddChild(mainMenu);
            escapePanel.AddChild(exit);
        }

        public override void LoadScreen()
        {
            foreach (Entity e in rootEntities)
                e.Visible = true;

            centreOnPosition(UnitManager.currentUnit.position);
        }

        public override void UnloadScreen()
        {
            foreach (Entity e in rootEntities)
                e.Visible = false;

            escapePanel.Visible = false;
            isEscapeMenuActive = false;
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
                else if (InputManager.IsKeyDown(Keys.Down))
                    camera.Move(new Vector2(0, movementSpeed) * delta);
                if (InputManager.IsKeyDown(Keys.Left))
                    camera.Move(new Vector2(-movementSpeed, 0) * delta);
                else if (InputManager.IsKeyDown(Keys.Right))
                    camera.Move(new Vector2(movementSpeed, 0) * delta);

                if (InputManager.isSinglePress(Keys.N))
                {
                    UnitManager.changeCurrentUnit();
                    centreOnPosition(UnitManager.currentUnit.position);
                }

                if (InputManager.isSinglePress(Keys.B))
                {
                    if (UnitManager.currentUnit.type.canBuild)
                    {
                        CityManager.spawnCity(UnitManager.currentUnit);
                        UnitManager.destroyUnit(UnitManager.currentUnit);
                    }
                }

                bool mouseOverGUI = false;
                foreach (Entity e in rootEntities)
                    if (e.IsMouseOver)
                        mouseOverGUI = true;

                    if (InputManager.isSingleLeftPress() && !mouseOverGUI)
                {
                    Vector2 mousePosition = camera.ScreenToWorld(new Vector2(Mouse.GetState().X, Mouse.GetState().Y));
                    if (mousePosition.X > 0 && mousePosition.Y > 0)
                    {
                        Rectangle tempRectangle = new Rectangle(((int)mousePosition.X / 64) * 64, ((int)mousePosition.Y / 64) * 64, 64, 64);
                        if (UnitManager.unitDictionary.ContainsKey(tempRectangle))
                        {
                            UnitManager.changeCurrentUnit(tempRectangle);
                        } else if (MapManager.mapDictionary.ContainsKey(tempRectangle) && UnitManager.currentUnit!=null)
                        {
                            UnitManager.checkPathfinding(tempRectangle);
                        }
                    }
                }
                if (InputManager.isSingleRightPress())
                {
                    UnitManager.currentUnit = null;
                }
            }
        }

        public override void Draw()
        {
            GraphicsDevice.Clear(Color.Green);
            mapDrawer.Draw(spriteBatch, camera);
            UserInterface.Active.Draw(spriteBatch);
        }

        private void centreOnPosition(Tile tile)
        {
            camera.Position = new Vector2((tile.position.X * 64)-400, (tile.position.Y) * 64-300);
        }
        
    }
}
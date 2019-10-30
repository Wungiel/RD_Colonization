using GeonBit.UI;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using RD_Colonization.Code;
using RD_Colonization.Code.Commands;
using RD_Colonization.Code.Data;
using RD_Colonization.Code.Entities;
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
        private Paragraph turnCounter, cashCounter;

        public GameScreen(ColonizationGame game) : base(game)
        {
            mapDrawer = new MapDrawer();
        }

        public override void LoadContent()
        {
            Texture2D mapTileset = Content.Load<Texture2D>("Images\\TileSet");
            Texture2D unitTileset = Content.Load<Texture2D>("Images\\UnitSet");
            Texture2D pixel = Content.Load<Texture2D>("Images\\pixel");
            SpriteFont font = Content.Load<SpriteFont>("Font\\MainFont");
            mapDrawer.SetGraphicData(mapTileset, unitTileset, pixel, font);
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
            turnCounter = new Paragraph(String.Format("Turn: {0}", 0), anchor: Anchor.Center);
            cashCounter = new Paragraph(String.Format("Cash: {0}", 0), anchor: Anchor.CenterRight);
            nextTurn.OnClick += (Entity _) =>
            {
                ChangeTurn();
            };
            mainPanel.AddChild(nextTurn);
            mainPanel.AddChild(turnCounter);
            mainPanel.AddChild(cashCounter);

            SetEscapePanel();
            UserInterface.Active.AddEntity(mainPanel);
            UserInterface.Active.AddEntity(escapePanel);

            rootEntities.Add(mainPanel);
        }

        private void ChangeTurn()
        {
            TurnManager.Instance.IncreaseTurn();

            turnCounter.Text = String.Format("Turn: {0}", TurnManager.Instance.TurnNumber);
            cashCounter.Text = String.Format("Cash: {0}", PlayerManager.Instance.currentPlayer.cash);

            UnitManager.Instance.ChangeCurrentUnit();
            CentreOnPosition(UnitManager.Instance.currentUnit);
        }

        private void SetEscapePanel()
        {
            escapePanel = new Panel(new Vector2(300, 300), PanelSkin.Default, Anchor.Center, new Vector2(10, 10))
            {
                Visible = false
            };

            Button mainMenu = new Button(mainMenuString);
            mainMenu.OnClick += (Entity entity) =>
            {
                ScreenManager.Instance.SetScreen(mainMenuScreenString);
            };

            Button exit = new Button(exitString);
            exit.OnClick += (Entity _) =>
            {
                Game.Exit();
            };

            escapePanel.AddChild(mainMenu);
            escapePanel.AddChild(exit);
        }

        public override void LoadScreen()
        {
            foreach (Entity e in rootEntities)
                e.Visible = true;

            CentreOnPosition(UnitManager.Instance.currentUnit);
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

            if (InputManager.Instance.IsSinglePress(Keys.Escape))
            {
                escapePanel.Visible = !escapePanel.Visible;
                isEscapeMenuActive = !isEscapeMenuActive;
            }

            if (!isEscapeMenuActive)
            {
                if (InputManager.Instance.IsKeyDown(Keys.Up))
                    camera.Move(new Vector2(0, -movementSpeed) * delta);
                else if (InputManager.Instance.IsKeyDown(Keys.Down))
                    camera.Move(new Vector2(0, movementSpeed) * delta);
                if (InputManager.Instance.IsKeyDown(Keys.Left))
                    camera.Move(new Vector2(-movementSpeed, 0) * delta);
                else if (InputManager.Instance.IsKeyDown(Keys.Right))
                    camera.Move(new Vector2(movementSpeed, 0) * delta);

                if (InputManager.Instance.IsSinglePress(Keys.N))
                {
                    UnitManager.Instance.ChangeCurrentUnit();
                    CentreOnPosition(UnitManager.Instance.currentUnit.currentTile);
                }

                if (InputManager.Instance.IsSinglePress(Keys.Q))
                {
                    ActionManager.Instance.CreateUnit(civilianString);
                }
                if (InputManager.Instance.IsSinglePress(Keys.W))
                {
                    ActionManager.Instance.CreateUnit(soldierString);
                }
                if (InputManager.Instance.IsSinglePress(Keys.E))
                {
                    ActionManager.Instance.CreateUnit(scoutString);
                }
                if (InputManager.Instance.IsSinglePress(Keys.R))
                {
                    ActionManager.Instance.CreateUnit(shipString);
                }

                if (InputManager.Instance.IsSinglePress(Keys.B))
                {
                    ActionManager.Instance.BuildCity();
                }

                if (InputManager.Instance.IsSinglePress(Keys.Space))
                {
                    ChangeTurn();
                }

                bool mouseOverGUI = false;
                foreach (Entity e in rootEntities)
                {
                    if (e.IsMouseOver)
                    {
                        mouseOverGUI = true;
                    }
                }

                if (InputManager.Instance.IsSingleLeftPress() && !mouseOverGUI)
                {
                    Vector2 mousePosition = camera.ScreenToWorld(new Vector2(Mouse.GetState().X, Mouse.GetState().Y));
                    if (mousePosition.X > 0 && mousePosition.Y > 0)
                    {
                        Rectangle tempRectangle = new Rectangle(((int)mousePosition.X / 64) * 64, ((int)mousePosition.Y / 64) * 64, 64, 64);
                        
                        if (UnitManager.Instance.unitDictionary.ContainsKey(tempRectangle))
                        {
                            if (UnitManager.Instance.IsUnitOnRectangleFriendly(tempRectangle) == true)
                            {
                                UnitManager.Instance.ChangeCurrentUnit(tempRectangle);
                            }
                            else
                            {
                                UnitManager.Instance.currentUnit.currentCommand = new MoveCommand(tempRectangle, UnitManager.Instance.currentUnit);
                            }
                        }
                        else if (MapManager.Instance.mapDictionary.ContainsKey(tempRectangle) && UnitManager.Instance.currentUnit !=null)
                        {
                            UnitManager.Instance.currentUnit.currentCommand = new MoveCommand(tempRectangle, UnitManager.Instance.currentUnit);
                        }
                    }
                }
                if (InputManager.Instance.IsSingleRightPress())
                {
                    UnitManager.Instance.DeselectUnit();
                    CityManager.Instance.DeselectCurrentCity();

                    Vector2 mousePosition = camera.ScreenToWorld(new Vector2(Mouse.GetState().X, Mouse.GetState().Y));
                    if (mousePosition.X > 0 && mousePosition.Y > 0)
                    {
                        Rectangle tempRectangle = new Rectangle(((int)mousePosition.X / 64) * 64, ((int)mousePosition.Y / 64) * 64, 64, 64);

                        if (CityManager.Instance.citytDictionary.ContainsKey(tempRectangle))
                        {
                            CityManager.Instance.ChangeCurrenCity(tempRectangle);
                        }
                    }
                    
                }
            }
        }

        public override void Draw()
        {
            GraphicsDevice.Clear(Color.Green);
            mapDrawer.Draw(SpriteBatch, camera);
            UserInterface.Active.Draw(SpriteBatch);
        }

        private void CentreOnPosition(Unit unit)
        {
            if (unit != null)
            {
                CentreOnPosition(unit.currentTile);
            }
        }

        private void CentreOnPosition(Tile tile)
        {
            camera.Position = new Vector2((tile.position.X * 64)-400, ((tile.position.Y) * 64) - 300);
        }
    }
}
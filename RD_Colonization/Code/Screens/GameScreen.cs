using GeonBit.UI;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using RD_Colonization.Code;
using RD_Colonization.Code.Data;
using RD_Colonization.Code.Managers;
using System.Diagnostics;
using static RD_Colonization.Code.StringList;

namespace RD_Colonization
{
    public class GameScreen : DefaultScreen
    {
        private Camera2D camera;
        private MapDrawer mapDrawer;
        private Panel mainPanel, escapePanel;
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
            mainPanel.Visible = true;
        }

        public override void UnloadScreen()
        {
            mainPanel.Visible = false;
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
                if (InputManager.IsKeyDown(Keys.Down))
                    camera.Move(new Vector2(0, movementSpeed) * delta);
                if (InputManager.IsKeyDown(Keys.Left))
                    camera.Move(new Vector2(-movementSpeed, 0) * delta);
                if (InputManager.IsKeyDown(Keys.Right))
                    camera.Move(new Vector2(movementSpeed, 0) * delta);

                if (InputManager.isSingleLeftPress())
                {
                    Vector2 mousePosition = camera.ScreenToWorld(new Vector2(Mouse.GetState().X, Mouse.GetState().Y));
                    if (mousePosition.X > 0 && mousePosition.Y > 0)
                    {
                        Rectangle tempRectangle = new Rectangle(((int)mousePosition.X / 64) * 64, ((int)mousePosition.Y / 64) * 64, 64, 64);
                        if (MapManager.mapDictionary.ContainsKey(tempRectangle))
                        {
                            Tile value;
                            MapManager.mapDictionary.TryGetValue(tempRectangle, out value);
                            Debug.WriteLine(value.type.name);
                        }
                    }
                }
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
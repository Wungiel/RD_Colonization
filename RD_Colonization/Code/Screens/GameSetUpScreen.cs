using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeonBit.UI;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using RD_Colonization.Code.Managers;
using static RD_Colonization.Code.StringList;

namespace RD_Colonization.Code.Screens
{
    class GameSetUpScreen : DefaultScreen
    {

        List<Entity> rootEntities = new List<Entity>();

        public GameSetUpScreen(ColonizationGame game) : base(game)
        {
        }

        public override void Initialize()
        {
            PrepareGUI();
            foreach(Entity e in rootEntities)
            {
                e.Visible = false;
            }
        }

        private void PrepareGUI()
        {           
            Button backButton = new Button("Back", ButtonSkin.Default, Anchor.BottomLeft, new Vector2(200,50));
            backButton.OnClick += (Entity entity) =>
            {
                ScreenManager.setScreen(mainMenuScreenString);
            };
            Button startGameButton = new Button("Start", ButtonSkin.Default, Anchor.BottomRight, new Vector2(200, 50));
            startGameButton.OnClick += (Entity entity) =>
            {

            };

            Panel mainPanel = new Panel(new Vector2(700, 450), PanelSkin.None, Anchor.TopCenter, new Vector2(10, 10));
            PanelTabs tabs = new PanelTabs();
            tabs.BackgroundSkin = PanelSkin.Default;
            tabs.Padding = new Vector2(10, 10);
            TabData countryTab = tabs.AddTab("Country");
            Panel listPanel = new Panel(new Vector2(200, tabs.Size.Y), anchor: Anchor.AutoInline);
            Panel descriptionPanel = new Panel(new Vector2(400, tabs.Size.Y), anchor: Anchor.AutoInline);
            countryTab.panel.AddChild(listPanel);
            listPanel.AddChild(new Header("List:"));
            listPanel.AddChild(new HorizontalLine());
            countryTab.panel.AddChild(descriptionPanel);
            descriptionPanel.AddChild(new Header("Description"));
            descriptionPanel.AddChild(new HorizontalLine());
            TabData sizeMapTab = tabs.AddTab("Map Size");
            TabData typeMapTab = tabs.AddTab("Map Type");

            UserInterface.Active.AddEntity(mainPanel);
            mainPanel.AddChild(tabs);
            UserInterface.Active.AddEntity(backButton);
            UserInterface.Active.AddEntity(startGameButton);
            
            rootEntities.Add(backButton);
            rootEntities.Add(startGameButton);
            rootEntities.Add(mainPanel);
        }

        public override void LoadContent()
        {
            
        }

        public override void LoadScreen()
        {
            foreach (Entity e in rootEntities)
            {
                e.Visible = true;
            }
        }

        public override void UnloadScreen()
        {
            foreach (Entity e in rootEntities)
            {
                e.Visible = false;
            }
        }

        public override void Update(GameTime gameTime)
        {
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

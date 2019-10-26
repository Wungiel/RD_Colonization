using GeonBit.UI;
using GeonBit.UI.Entities;
using GeonBit.UI.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RD_Colonization.Code.Managers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using static RD_Colonization.Code.StringList;

namespace RD_Colonization.Code.Screens
{
    public class TestSetUpScreen : DefaultScreen
    {
        private List<Entity> rootEntities = new List<Entity>();
        private TabData countryTab, sizeMapTab;

        public TestSetUpScreen(ColonizationGame game) : base(game)
        {
        }

        public override void Initialize()
        {
        }

        public override void LoadContent()
        {
            PrepareGUI();
            foreach (Entity e in rootEntities)
            {
                e.Visible = false;
            }

        }

        private void PrepareGUI()
        {
            Button backButton, startGameButton;
            Panel mainPanel;

            SetBackStartButtons(out backButton, out startGameButton);
            SetMainPanel(out mainPanel);

            UserInterface.Active.AddEntity(mainPanel);
            UserInterface.Active.AddEntity(backButton);
            UserInterface.Active.AddEntity(startGameButton);

            rootEntities.Add(mainPanel);
            rootEntities.Add(backButton);
            rootEntities.Add(startGameButton);
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
            SpriteBatch.Begin();
            SpriteBatch.End();
            UserInterface.Active.Draw(SpriteBatch);
        }

        private void SetBackStartButtons(out Button backButton, out Button startGameButton)
        {
            backButton = new Button("Back", ButtonSkin.Default, Anchor.BottomLeft, new Vector2(200, 50));
            backButton.OnClick += (Entity entity) =>
            {
                ScreenManager.Instance.SetScreen(mainMenuScreenString);
            };
            startGameButton = new Button("Start", ButtonSkin.Default, Anchor.BottomRight, new Vector2(200, 50));
            startGameButton.OnClick += (Entity entity) =>
            {
                MessageBox.ShowMsgBox("Start", "It will start a  test");
            };
        }

        private void SetMainPanel(out Panel mainPanel)
        {
            mainPanel = new Panel(new Vector2(800, 450), PanelSkin.None, Anchor.TopCenter, new Vector2(10, 10));

            Panel listPanel;
            Panel descriptionPanel;

            SetListPanel(out listPanel, mainPanel.Size.Y);
            SetDescriptionPanel(out descriptionPanel, mainPanel.Size.Y);

            mainPanel.AddChild(listPanel);
            mainPanel.AddChild(descriptionPanel);
        }

        private void SetListPanel(out Panel listPanel, float height)
        {
            listPanel = new Panel(new Vector2(300, height), anchor: Anchor.AutoInline);

            foreach (String testFileName in TestManager.Instance.GetTestFiles())
            {
                Button testFileButton = new Button(testFileName);
                testFileButton.OnClick += (Entity entity) =>
                {
                    SetDescriptionPanelData(testFileName);
                };
                listPanel.AddChild(testFileButton);
            }
        }

        private void SetDescriptionPanel(out Panel descriptionPanel, float height)
        {
            descriptionPanel = new Panel(new Vector2(400, height), anchor: Anchor.AutoInline);
        }

        private void SetDescriptionPanelData(String testName)
        {
           Debug.WriteLine(TestManager.Instance.GetTestData(testName).name);
        }

    }
}
using GeonBit.UI;
using GeonBit.UI.Entities;
using GeonBit.UI.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RD_Colonization.Code.Data;
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
        private List<Paragraph> descriptionParagraphs = new List<Paragraph>();
        private TestData selectedTest = null;

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
                if (selectedTest != null)
                {
                    if (selectedTest.mapName != null && selectedTest.mapName != string.Empty)
                    {
                        MapManager.Instance.GenerateMap(selectedTest.mapName, GraphicsDevice);
                    }
                    else
                    {
                        MapManager.Instance.GenerateMap(40);
                    }                                       
                    
                    if (selectedTest.canPlayerPlay == true)
                    {
                        PlayerManager.Instance.SetUpPlayers();
                        TestManager.Instance.InitializeTest(selectedTest);
                        ScreenManager.Instance.SetScreen(gameScreenString);
                    }
                    else
                    {
                        PlayerManager.Instance.SetUpPlayers(false);
                        TestManager.Instance.InitializeTest(selectedTest);
                        PlayerManager.Instance.ProcessTurn();
                    }
                }
            };
        }

        private void SetMainPanel(out Panel mainPanel)
        {
            mainPanel = new Panel(new Vector2(800, 500), PanelSkin.None, Anchor.TopCenter, new Vector2(10, 10));

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
                    selectedTest = TestManager.Instance.GetTestData(testFileName);
                    SetDescriptionPanelData(selectedTest);

                };
                listPanel.AddChild(testFileButton);
            }
        }

        private void SetDescriptionPanel(out Panel descriptionPanel, float height)
        {
            descriptionPanel = new Panel(new Vector2(400, height), anchor: Anchor.AutoInline);
            descriptionPanel.AddChild(new Header("Description"));
            descriptionParagraphs.Add((Paragraph) descriptionPanel.AddChild(new Paragraph("Name: ", scale: 0.6f))); 
            descriptionParagraphs.Add((Paragraph) descriptionPanel.AddChild(new Paragraph("Map: ", scale: 0.6f)));
            descriptionParagraphs.Add((Paragraph) descriptionPanel.AddChild(new Paragraph("Use fixed start point: ", scale: 0.6f)));
            descriptionParagraphs.Add((Paragraph) descriptionPanel.AddChild(new Paragraph("Live player: ", scale: 0.6f)));
            descriptionParagraphs.Add((Paragraph) descriptionPanel.AddChild(new Paragraph("Use Evolution: ", scale: 0.6f)));
            descriptionParagraphs.Add((Paragraph)descriptionPanel.AddChild(new Paragraph("Can use history: ", scale: 0.6f)));
            descriptionParagraphs.Add((Paragraph)descriptionPanel.AddChild(new Paragraph("Can use player AI: ", scale: 0.6f)));
            descriptionParagraphs.Add((Paragraph) descriptionPanel.AddChild(new Paragraph("Evolution frequency: ", scale: 0.6f)));
            descriptionParagraphs.Add((Paragraph) descriptionPanel.AddChild(new Paragraph("Use RF: ", scale: 0.6f)));
            descriptionParagraphs.Add((Paragraph) descriptionPanel.AddChild(new Paragraph("Is maintaining secrecy: ", scale: 0.6f)));
            descriptionParagraphs.Add((Paragraph) descriptionPanel.AddChild(new Paragraph("Can affect player: ", scale: 0.6f)));
            descriptionParagraphs.Add((Paragraph) descriptionPanel.AddChild(new Paragraph("Resource fitting frequency: ", scale: 0.6f)));
            descriptionParagraphs.Add((Paragraph) descriptionPanel.AddChild(new Paragraph("Build city score: ", scale: 0.6f)));
            descriptionParagraphs.Add((Paragraph) descriptionPanel.AddChild(new Paragraph("Build unit score: ", scale: 0.6f)));
            descriptionParagraphs.Add((Paragraph) descriptionPanel.AddChild(new Paragraph("Discover tile score: ", scale: 0.6f)));
            descriptionParagraphs.Add((Paragraph) descriptionPanel.AddChild(new Paragraph("Destroy city score: ", scale: 0.6f)));
            descriptionParagraphs.Add((Paragraph) descriptionPanel.AddChild(new Paragraph("Destroy unit score: ", scale: 0.6f)));
            descriptionParagraphs.Add((Paragraph) descriptionPanel.AddChild(new Paragraph("Number of turns: ", scale: 0.6f)));
        }

        private void SetDescriptionPanelData(TestData test)
        {
            foreach(Paragraph p in descriptionParagraphs)
            {
                int index = p.Text.IndexOf(":");
                p.Text = p.Text.Substring(0, index + 1);
            }

            descriptionParagraphs[0].Text += test.name;
            descriptionParagraphs[1].Text += test.mapName;
            descriptionParagraphs[2].Text += test.useFixedStartPoints;
            descriptionParagraphs[3].Text += test.canPlayerPlay;
            descriptionParagraphs[4].Text += test.useEvolution;
            descriptionParagraphs[5].Text += test.canUseHistory;
            descriptionParagraphs[6].Text += test.evolutionFrequency;
            descriptionParagraphs[7].Text += test.canEvolutionUseAIUserParameter;
            descriptionParagraphs[8].Text += test.useResourceFitting;
            descriptionParagraphs[9].Text += test.isMaintainingSecrecy;
            descriptionParagraphs[10].Text += test.canAffectPlayer;
            descriptionParagraphs[11].Text += test.resourceFittingFrequency;
            descriptionParagraphs[12].Text += test.buildCityScore;
            descriptionParagraphs[13].Text += test.buildUnitScore;
            descriptionParagraphs[14].Text += test.discoverTileScore;
            descriptionParagraphs[15].Text += test.destroyEnemyCityScore;
            descriptionParagraphs[16].Text += test.destroyEnemyUnitScore;
            descriptionParagraphs[17].Text += test.numberOfTurns;
        }

    }
}
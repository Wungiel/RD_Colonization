using GeonBit.UI;
using GeonBit.UI.Entities;
using GeonBit.UI.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RD_Colonization.Code.Managers;
using System;
using System.Collections.Generic;
using static RD_Colonization.Code.StringList;

namespace RD_Colonization.Code.Screens
{
    internal class GameSetUpScreen : DefaultScreen
    {
        private Texture2D portraits;
        private List<Entity> rootEntities = new List<Entity>();
        private List<int> sizes = new List<int> { 30, 40, 50, 60 };
        private Dictionary<string, List<Entity>> pairsOfElements = new Dictionary<string, List<Entity>>();
        private string civilizationKey = "Random";
        private string sizeKey = "Random";
        private RadioButton randomCivilization, randomSize;
        private TabData countryTab, sizeMapTab;

        public GameSetUpScreen(ColonizationGame game) : base(game)
        {
        }

        public override void Initialize()
        {
        }

        public override void LoadContent()
        {
            portraits = Content.Load<Texture2D>("Images\\Portraits");
            CivilizationManager.initialize(portraits);
            PrepareGUI();
            foreach (Entity e in rootEntities)
            {
                e.Visible = false;
            }
        }

        private void PrepareGUI()
        {
            Button backButton, startGameButton;
            setBackStartButtons(out backButton, out startGameButton);

            Panel mainPanel = new Panel(new Vector2(800, 450), PanelSkin.None, Anchor.TopCenter, new Vector2(10, 10));
            PanelTabs tabs = new PanelTabs();
            tabs.BackgroundSkin = PanelSkin.Default;
            tabs.Padding = new Vector2(10, 10);

            setCountryTab(tabs);
            setMapSizeTab(tabs);

            UserInterface.Active.AddEntity(mainPanel);
            mainPanel.AddChild(tabs);
            UserInterface.Active.AddEntity(backButton);
            UserInterface.Active.AddEntity(startGameButton);

            rootEntities.Add(backButton);
            rootEntities.Add(startGameButton);
            rootEntities.Add(mainPanel);
        }

        private void setMapSizeTab(PanelTabs tabs)
        {
            sizeMapTab = tabs.AddTab("Map");
            {
                randomSize = new RadioButton("Random", isChecked: true);
                randomSize.OnClick += (Entity entity) =>
                {
                    sizeKey = "Random";
                };
                sizeMapTab.panel.AddChild(randomSize);
                foreach (int i in sizes)
                {
                    RadioButton r = new RadioButton(String.Format("{0}x{0}", i));
                    r.OnClick += (Entity entity) =>
                    {
                        sizeKey = i.ToString();
                    };
                    sizeMapTab.panel.AddChild(r);
                }
            }
        }

        private void setCountryTab(PanelTabs tabs)
        {
            countryTab = tabs.AddTab("Country");
            {
                Panel listPanel = new Panel(new Vector2(300, tabs.Size.Y), anchor: Anchor.AutoInline);
                Panel descriptionPanel = new Panel(new Vector2(400, tabs.Size.Y), anchor: Anchor.AutoInline);
                countryTab.panel.AddChild(listPanel);
                listPanel.AddChild(new Header("List:"));
                listPanel.AddChild(new HorizontalLine());
                randomCivilization = new RadioButton("Random", isChecked: true);
                randomCivilization.OnClick += (Entity entity) =>
                {
                    foreach (KeyValuePair<string, List<Entity>> item in pairsOfElements)
                        foreach (Entity e in item.Value)
                            e.Visible = false;
                    civilizationKey = "Random";
                };
                listPanel.AddChild(randomCivilization);
                foreach (String s in CivilizationManager.getNames())
                {
                    RadioButton r = new RadioButton(s);
                    r.Identifier = s;
                    r.OnClick += (Entity entity) =>
                    {
                        foreach (KeyValuePair<string, List<Entity>> item in pairsOfElements)
                            foreach (Entity e in item.Value)
                                e.Visible = false;
                        List<Entity> tmp = null;
                        pairsOfElements.TryGetValue(s, out tmp);
                        tmp[0].Visible = true;
                        tmp[1].Visible = true;
                        civilizationKey = r.Identifier;
                    };
                    listPanel.AddChild(r);
                }
                countryTab.panel.AddChild(descriptionPanel);
                descriptionPanel.AddChild(new Header("Description"));
                descriptionPanel.AddChild(new HorizontalLine());
                foreach (String s in CivilizationManager.getNames())
                {
                    Image i = CivilizationManager.getPortrait(s);
                    Paragraph p = new Paragraph(CivilizationManager.getInformations(s));
                    descriptionPanel.AddChild(i);
                    descriptionPanel.AddChild(p);
                    i.Visible = false;
                    p.Visible = false;
                    pairsOfElements.Add(s, new List<Entity> { i, p });
                }
            }
        }

        private void setBackStartButtons(out Button backButton, out Button startGameButton)
        {
            backButton = new Button("Back", ButtonSkin.Default, Anchor.BottomLeft, new Vector2(200, 50));
            backButton.OnClick += (Entity entity) =>
            {
                ScreenManager.setScreen(mainMenuScreenString);
            };
            startGameButton = new Button("Start", ButtonSkin.Default, Anchor.BottomRight, new Vector2(200, 50));
            startGameButton.OnClick += (Entity entity) =>
            {
                MessageBox.ShowMsgBox("Start game", String.Format("Your current settings are: \nCivilization: {0}\nSize: {1}", civilizationKey, sizeKey), new MessageBox.MsgBoxOption[] {
                                new MessageBox.MsgBoxOption("Cancel", () =>
                                {
                                    return true;
                                }),
                                new MessageBox.MsgBoxOption("Confirm", () =>
                                {
                                    if (sizeKey.Equals("Random"))
                                        sizeKey = "30";
                                    MapManager.generateMap(Int32.Parse(sizeKey));
                                    UnitManager.setUpGameStart();
                                    ScreenManager.setScreen(gameScreenString);
                                    return true; })
                                });
            };
        }

        public override void LoadScreen()
        {
            foreach (Entity e in rootEntities)
            {
                e.Visible = true;
            }
            randomCivilization.Checked = true;
            randomSize.Checked = true;

            foreach (KeyValuePair<string, List<Entity>> item in pairsOfElements)
                foreach (Entity e in item.Value)
                    e.Visible = false;

            civilizationKey = "Random";
            sizeKey = "Random";
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
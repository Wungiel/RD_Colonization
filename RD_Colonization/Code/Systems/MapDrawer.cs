using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using RD_Colonization.Code.Data;
using RD_Colonization.Code.Entities;
using RD_Colonization.Code.Managers;
using System;
using System.Collections.Generic;
using static RD_Colonization.Code.StringList;

namespace RD_Colonization.Code
{
    public class MapDrawer
    {
        private Texture2D mapTileset;
        private Texture2D unitTileset;
        private Texture2D singlePixel;
        private Dictionary<String, Rectangle> tileGraphics = new Dictionary<String, Rectangle>();
        private Dictionary<String, Rectangle> unitGraphics = new Dictionary<String, Rectangle>();
        private Dictionary<String, Rectangle> cityGraphics = new Dictionary<String, Rectangle>();
        private SpriteFont font;
        int blink = 0;

        public void SetGraphicData(Texture2D mapTileset, Texture2D unitTileset, Texture2D pixel, SpriteFont font)
        {
            this.mapTileset = mapTileset;
            this.unitTileset = unitTileset;
            this.singlePixel = pixel;
            this.font = font;

            tileGraphics.Add(grassString, new Rectangle(0, 64, 64, 64));
            tileGraphics.Add(mountainString, new Rectangle(192, 64, 64, 64));
            tileGraphics.Add(waterString, new Rectangle(0, 0, 64, 64));

            unitGraphics.Add(civilianString, new Rectangle(0, 0, 64, 64));
            unitGraphics.Add(soldierString, new Rectangle(64, 0, 64, 64));
            unitGraphics.Add(scoutString, new Rectangle(64, 0, 64, 64));
            unitGraphics.Add(shipString, new Rectangle(192, 0, 64, 64));

            cityGraphics.Add(cityString, new Rectangle(128, 0, 64, 64));
        }

        public void Draw(SpriteBatch spriteBatch, Camera2D camera)
        {
            
            Matrix transformMatrix = camera.GetViewMatrix();

            spriteBatch.Begin(transformMatrix: transformMatrix);
            foreach (KeyValuePair<Rectangle, Tile> pair in MapManager.Instance.mapDictionary)
            {
                DrawMap(spriteBatch, pair);
            }

            foreach (KeyValuePair<Rectangle, City> pair in CityManager.Instance.citytDictionary)
            {
                DrawCity(spriteBatch, pair);
            }

            foreach (KeyValuePair<Rectangle, Unit> pair in UnitManager.Instance.unitDictionary)
            {
                DrawUnits(spriteBatch, pair);
                DrawPaths(spriteBatch, pair.Value);
            }

            foreach (KeyValuePair<Rectangle, Tile> pair in MapManager.Instance.mapDictionary)
            {
                DrawSpecialMode(spriteBatch, pair);
            }

            blink++;
            if (blink == 45)
                blink = 0;
            spriteBatch.End();
        }


        private void DrawMap(SpriteBatch spriteBatch, KeyValuePair<Rectangle, Tile> pair)
        {
            Rectangle sourceRectangle;
            tileGraphics.TryGetValue(pair.Value.type.name, out sourceRectangle);
            spriteBatch.Draw(mapTileset, pair.Key, sourceRectangle, Color.White);
        }

        private void DrawSpecialMode(SpriteBatch spriteBatch, KeyValuePair<Rectangle, Tile> pair)
        {
            /*if (pair.Value.discoveredByPlayerIds.Contains(PlayerManager.Instance.currentPlayer.id) == false)
            {
                spriteBatch.Draw(singlePixel, pair.Key, Color.Black);
            }*/

            for (int i = 0; i < 4; i++)
            {
                if (pair.Value.riskValues.ContainsKey(i) == true && pair.Value.riskValues[i] > 0)
                {
                    spriteBatch.Draw(singlePixel, pair.Key, Color.Red * 0.5f);
                }
            }

        }

        private void DrawUnits(SpriteBatch spriteBatch, KeyValuePair<Rectangle, Unit> pair)
        {
            Rectangle sourceRectangle;
            if (pair.Value != UnitManager.Instance.currentUnit)
            {
                unitGraphics.TryGetValue(pair.Value.type.name, out sourceRectangle);
                spriteBatch.Draw(unitTileset, pair.Key, sourceRectangle, Color.White);
            }
            else
            {
                if (blink > 15)
                {
                    unitGraphics.TryGetValue(pair.Value.type.name, out sourceRectangle);
                    spriteBatch.Draw(unitTileset, pair.Key, sourceRectangle, Color.White);
                }
            }

            spriteBatch.DrawRectangle(pair.Key, PlayerManager.Instance.GetPlayerByUnit(pair.Value).playerColor);
        }

        private void DrawPaths(SpriteBatch spriteBatch, Unit unit)
        {
            List<Tile> tiles = UnitManager.Instance.GetPathTiles(unit.currentCommand);
            foreach (Tile t in tiles)
            {
                spriteBatch.DrawCircle(t.CreateCircle(), 12, PlayerManager.Instance.GetPlayerByUnit(unit).playerColor);
                spriteBatch.DrawString(font, (tiles.IndexOf(t) / unit.type.speed).ToString(), t.GetCenter(), PlayerManager.Instance.GetPlayerByUnit(unit).playerColor);
            }
        }

        private void DrawCity(SpriteBatch spriteBatch, KeyValuePair<Rectangle, City> pair)
        {
            Rectangle sourceRectangle;
            cityGraphics.TryGetValue(cityString, out sourceRectangle);
            if (pair.Value != CityManager.Instance.currentCity)
            {
                spriteBatch.Draw(unitTileset, pair.Key, sourceRectangle, Color.White);
            }
            else
            {
                if (blink > 15)
                {
                    spriteBatch.Draw(unitTileset, pair.Key, sourceRectangle, Color.White);
                }
            }
            
            spriteBatch.DrawRectangle(pair.Key, PlayerManager.Instance.GetPlayerByCity(pair.Value).playerColor);
        }
        
    }
}
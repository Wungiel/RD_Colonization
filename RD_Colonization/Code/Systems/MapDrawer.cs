using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using RD_Colonization.Code.Data;
using RD_Colonization.Code.Entities;
using RD_Colonization.Code.Managers;
using System;
using System.Collections.Generic;
using static RD_Colonization.Code.StringList;
using static RD_Colonization.Code.RectangleHelper;

namespace RD_Colonization.Code
{
    public class MapDrawer
    {
        private Texture2D mapTileset;
        private Texture2D unitTileset;
        private Dictionary<String, Rectangle> tileGraphics = new Dictionary<String, Rectangle>();
        private Dictionary<String, Rectangle> unitGraphics = new Dictionary<String, Rectangle>();
        private Dictionary<String, Rectangle> cityGraphics = new Dictionary<String, Rectangle>();
        int blink = 0;

        public void setTileset(Texture2D mapTileset, Texture2D unitTileset)
        {
            this.mapTileset = mapTileset;
            this.unitTileset = unitTileset;

            tileGraphics.Add(grassString, new Rectangle(0, 64, 64, 64));
            tileGraphics.Add(mountainString, new Rectangle(192, 64, 64, 64));
            tileGraphics.Add(waterString, new Rectangle(0, 0, 64, 64));

            unitGraphics.Add(civilianString, new Rectangle(0, 0, 64, 64));
            unitGraphics.Add(soldierString, new Rectangle(64, 0, 64, 64));            
            unitGraphics.Add(shipString, new Rectangle(192, 0, 64, 64));

            cityGraphics.Add(cityString, new Rectangle(128, 0, 64, 64));
        }

        public void Draw(SpriteBatch spriteBatch, Camera2D camera)
        {
            
            Matrix transformMatrix = camera.GetViewMatrix();

            spriteBatch.Begin(transformMatrix: transformMatrix);
            foreach (KeyValuePair<Rectangle, Tile> pair in MapManager.mapDictionary)
            {
                drawMap(spriteBatch, pair);
            }

            foreach (KeyValuePair<Rectangle, Unit> pair in UnitManager.unitDictionary)
            {
                drawUnits(spriteBatch, pair);
                drawPaths(spriteBatch, pair.Value);
            }

            foreach (KeyValuePair<Rectangle, City> pair in CityManager.citytDictionary)
            {
                drawCity(spriteBatch, pair);
            }

            blink++;
            if (blink == 45)
                blink = 0;
            spriteBatch.End();
        }


        private void drawMap(SpriteBatch spriteBatch, KeyValuePair<Rectangle, Tile> pair)
        {
            Rectangle sourceRectangle;
            tileGraphics.TryGetValue(pair.Value.type.name, out sourceRectangle);
            spriteBatch.Draw(mapTileset, pair.Key, sourceRectangle, Color.White);
        }

        private void drawUnits(SpriteBatch spriteBatch, KeyValuePair<Rectangle, Unit> pair)
        {
            Rectangle sourceRectangle;
            if (pair.Value != UnitManager.currentUnit)
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
        }

        private void drawPaths(SpriteBatch spriteBatch, Unit value)
        {
            List<Tile> tiles = UnitManager.getPathTiles(value);
            foreach (Tile t in tiles)
            {
                spriteBatch.DrawRectangle(createRectangle(t), Color.Red);
            }
        }

        private void drawCity(SpriteBatch spriteBatch, KeyValuePair<Rectangle, City> pair)
        {
            Rectangle sourceRectangle;
            cityGraphics.TryGetValue(cityString, out sourceRectangle);
            spriteBatch.Draw(unitTileset, pair.Key, sourceRectangle, Color.White);
        }
    }
}
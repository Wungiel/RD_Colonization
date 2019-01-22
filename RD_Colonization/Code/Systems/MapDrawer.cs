using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using RD_Colonization.Code.Data;
using RD_Colonization.Code.Entities;
using RD_Colonization.Code.Managers;
using static RD_Colonization.Code.StringList;

namespace RD_Colonization.Code
{
    internal class MapDrawer
    {
        Texture2D mapTileset;
        Texture2D unitTileset;
        Dictionary<String, Rectangle> tileGraphics = new Dictionary<String, Rectangle>();
        Dictionary<String, Rectangle> unitGraphics = new Dictionary<String, Rectangle>();

        internal void setTileset(Texture2D mapTileset, Texture2D unitTileset)
        {
            this.mapTileset = mapTileset;
            this.unitTileset = unitTileset;

            tileGraphics.Add(grassString, new Rectangle(0, 64, 64, 64));
            tileGraphics.Add(mountainString, new Rectangle(192, 64, 64, 64));
            tileGraphics.Add(waterString, new Rectangle(0, 0, 64, 64));

            unitGraphics.Add(civilianString, new Rectangle(0, 0, 64, 64));
            unitGraphics.Add(soldierString, new Rectangle(64, 0, 64, 64));
            unitGraphics.Add(cityString, new Rectangle(128, 0, 64, 64));
            unitGraphics.Add(shipString, new Rectangle(192, 0, 64, 64));

        }

        internal void Draw(SpriteBatch spriteBatch, Camera2D camera)
        {
            Matrix transformMatrix = camera.GetViewMatrix();
            Rectangle sourceRectangle; 

            spriteBatch.Begin(transformMatrix: transformMatrix);
            foreach (KeyValuePair<Rectangle,Tile> pair in MapManager.mapDictionary)
            {
                tileGraphics.TryGetValue(pair.Value.type.name, out sourceRectangle);
                spriteBatch.Draw(mapTileset, pair.Key, sourceRectangle, Color.White);
            }

            foreach (KeyValuePair<Rectangle, Unit> pair in UnitManager.unitDictionary)
            {
                tileGraphics.TryGetValue(pair.Value.type.name, out sourceRectangle);
                spriteBatch.Draw(mapTileset, pair.Key, sourceRectangle, Color.White);
            }

            spriteBatch.End();
        }
    }
}
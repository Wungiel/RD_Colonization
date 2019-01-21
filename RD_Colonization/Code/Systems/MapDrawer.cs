using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using RD_Colonization.Code.Data;
using RD_Colonization.Code.Managers;
using static RD_Colonization.Code.StringList;

namespace RD_Colonization.Code
{
    internal class MapDrawer
    {
        Texture2D tileset;
        Dictionary<String, Rectangle> tileGraphics = new Dictionary<String, Rectangle>();

        internal void setTileset(Texture2D tileset)
        {
            this.tileset = tileset;
            tileGraphics.Add(grassString, new Rectangle(0, 64, 64, 64));
            tileGraphics.Add(mountainString, new Rectangle(192, 64, 64, 64));
            tileGraphics.Add(waterString, new Rectangle(0, 0, 64, 64));
        }

        internal void Draw(SpriteBatch spriteBatch, Camera2D camera)
        {
            Matrix transformMatrix = camera.GetViewMatrix();
            Rectangle sourceRectangle; 

            spriteBatch.Begin(transformMatrix: transformMatrix);
            foreach (KeyValuePair<Rectangle,Tile> pair in MapManager.mapDictionary)
            {
                tileGraphics.TryGetValue(pair.Value.type.name, out sourceRectangle);
                spriteBatch.Draw(tileset, pair.Key, sourceRectangle, Color.White);
            }
            spriteBatch.End();
        }
    }
}
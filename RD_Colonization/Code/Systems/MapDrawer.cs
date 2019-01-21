using System;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using RD_Colonization.Code.Data;
using RD_Colonization.Code.Managers;

namespace RD_Colonization.Code
{
    internal class MapDrawer
    {
        Texture2D tileset;

        internal void setTileset(Texture2D tileset)
        {
            this.tileset = tileset;
        }

        internal void Draw(SpriteBatch spriteBatch, Camera2D camera)
        {
            spriteBatch.Begin();
            spriteBatch.End();
        }
    }
}
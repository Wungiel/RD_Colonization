using Microsoft.Xna.Framework;
using RD_Colonization.Code.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RD_Colonization.Code
{
    public static class RectangleHelper
    {

        public static Rectangle createRectangle(Tile tile)
        {
            return new Rectangle((int)tile.position.X * 64, (int)tile.position.Y * 64, 64, 64);
        }

        public static Rectangle createRectangle(Vector2 vector2)
        {
            return new Rectangle(((int)vector2.X / 64) * 64, ((int)vector2.Y / 64) * 64, 64, 64);
        }

        public static Rectangle createRectangle(Point point)
        {
            return new Rectangle((int)point.X * 64, (int)point.Y * 64, 64, 64);
        }
    }
}

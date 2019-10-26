﻿using Microsoft.Xna.Framework;
using MonoGame.Extended;
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
        public static Rectangle CreateRectangle(Tile tile)
        {
            return new Rectangle(tile.GetPosition(), tile.GetSize());
        }

        public static Rectangle CreateRectangle(Vector2 vector2)
        {
            return new Rectangle(((int)vector2.X / 64) * 64, ((int)vector2.Y / 64) * 64, 64, 64);
        }

        public static Rectangle CreateRectangle(Point point)
        {
            return new Rectangle((int)point.X * 64, (int)point.Y * 64, 64, 64);
        }

        public static CircleF CreateCircle(Tile tile)
        {
            return new CircleF(tile.GetCenter(), 32);
        }
    }
}

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
    public static class ExtensionMethods
    {
        public static Rectangle CreateRectangle(this Tile tile)
        {
            return new Rectangle(tile.GetPosition(), tile.GetSize());
        }

        public static Rectangle CreateRectangle(this Vector2 vector2)
        {
            return new Rectangle(((int)vector2.X / 64) * 64, ((int)vector2.Y / 64) * 64, 64, 64);
        }

        public static Rectangle CreateRectangle(this Point point)
        {
            return new Rectangle((int)point.X * 64, (int)point.Y * 64, 64, 64);
        }

        public static CircleF CreateCircle(this Tile tile)
        {
            return new CircleF(tile.GetCenter(), 32);
        }

        public static Tile GetClosestTile(this Tile[] tiles, Tile tile)
        {
            if (tiles.Length == 0)
            {
                return null;
            }

            Tile closestTile = tiles[0];
            double closestDistance = closestTile.position.GetDistance(tile.position);

            foreach(Tile t in tiles)
            {
                if (t.position.GetDistance(tile.position) < closestDistance)
                {
                    closestDistance = t.position.GetDistance(tile.position);
                    closestTile = t;
                }
            }

            return closestTile;
        }

        public static double GetDistance (this Point point, Point anotherPoint)
        {
            double distance = Math.Pow(point.X + anotherPoint.X, 2) + Math.Pow(point.Y + anotherPoint.Y, 2);
            return Math.Sqrt(distance);
        }

        public static T GetRandomFromArray <T> (this T [] array) where T: class
        {
            if (array.Length == 0)
            {
                return null;
            } 
            else
            {
                Random random = new Random();
                return array[random.Next(0, array.Length)];
            }
        }

        public static List<int> GetDigits(this int number)
        {
            List<int> digits = new List<int>();

            do
            {
                digits.Add(number % 10);
                number /= 10;
            } while (number > 0);

            if (digits.Count == 1)
            {
                digits.Add(0);
                digits.Add(0);
            }
            else if (digits.Count == 2)
            {
                digits.Add(0);
            }
            digits.Reverse();

            return digits;
        }
    }
}

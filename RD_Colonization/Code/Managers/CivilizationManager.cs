using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RD_Colonization.Code.Data;
using System;
using System.Collections.Generic;

namespace RD_Colonization.Code.Managers
{
    public static class CivilizationManager
    {
        private static Dictionary<string, CivilizationData> civilizations = new Dictionary<string, CivilizationData>();
        private static Texture2D portraitsTexture;
        public static int cash { get; private set; }

        static CivilizationManager()
        {
            civilizations.Add("Germany", new CivilizationData("Germany", "Johan", 0));
            civilizations.Add("Sumeria", new CivilizationData("Sumeria", "Gilgamesh", 1));
            civilizations.Add("Pars", new CivilizationData("Pars", "Hilmes", 2));
            cash = 0;
        }

        public static void initialize(Texture2D portraits)
        {
            portraitsTexture = portraits;
        }

        public static Image getPortrait(String key)
        {
            Image tmpImage = new Image(portraitsTexture);
            CivilizationData tmp = null;
            civilizations.TryGetValue(key, out tmp);
            tmpImage.SourceRectangle = new Rectangle(new Point(tmp.artNumber * 150, 0), new Point(150, 150));
            tmpImage.Anchor = Anchor.AutoCenter;
            tmpImage.Size = new Vector2(150, 150);
            return tmpImage;
        }

        public static string getInformations(String key)
        {
            CivilizationData tmp = null;
            civilizations.TryGetValue(key, out tmp);
            return String.Format("State: {0} \nLeader: {1}", tmp.civilizationName, tmp.civilizationLeader);
        }

        public static List<String> getNames()
        {
            List<String> tmp = new List<String>();
            foreach (KeyValuePair<string, CivilizationData> item in civilizations)
            {
                tmp.Add(item.Key);
            }
            return tmp;
        }

        public static void addCash(int cashToAdd)
        {
            cash += cashToAdd;
        }

    }
}
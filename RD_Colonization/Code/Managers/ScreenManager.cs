using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RD_Colonization.Code.Managers
{
    internal static class ScreenManager
    {
        private static Dictionary<String, DefaultScreen> ScreenList = new Dictionary<String, DefaultScreen>();
        public static DefaultScreen activeScreen = null;

        public static void registerScreen(String key, DefaultScreen screen)
        {
            ScreenList.Add(key, screen);
        }

        internal static void initialize()
        {
            foreach(KeyValuePair<string,DefaultScreen> item in ScreenList)
            {
                item.Value.Initialize();
            }
        }

        internal static void loadContent()
        {
            foreach (KeyValuePair<string, DefaultScreen> item in ScreenList)
            {
                item.Value.LoadContent();
            }
        }

        public static void setScreen(String key)
        {
            if (activeScreen != null)
                activeScreen.UnloadScreen();
            ScreenList.TryGetValue(key, out activeScreen);
            activeScreen.LoadScreen();
        }
        
    }
}

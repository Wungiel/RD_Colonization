using System;
using System.Collections.Generic;

namespace RD_Colonization.Code.Managers
{
    public class ScreenManager : BaseManager<ScreenManager>
    {
        private Dictionary<String, DefaultScreen> ScreenList = new Dictionary<String, DefaultScreen>();
        public DefaultScreen activeScreen = null;

        public void RegisterScreen(String key, DefaultScreen screen)
        {
            ScreenList.Add(key, screen);
        }

        public void Initialize()
        {
            foreach (KeyValuePair<string, DefaultScreen> item in ScreenList)
            {
                item.Value.Initialize();
            }
        }

        public void LoadContent()
        {
            foreach (KeyValuePair<string, DefaultScreen> item in ScreenList)
            {
                item.Value.LoadContent();
            }
        }

        public void SetScreen(String key)
        {
            if (activeScreen != null)
                activeScreen.UnloadScreen();
            ScreenList.TryGetValue(key, out activeScreen);
            activeScreen.LoadScreen();
        }
    }
}
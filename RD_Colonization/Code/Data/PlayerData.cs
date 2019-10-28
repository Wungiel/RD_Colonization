using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RD_Colonization.Code.Data
{
    public class PlayerData
    {
        private static int currentId = 0;
        public int id = 0;
        public int cash = 0;
        public int food = 0;
        public bool isControlledByAI = true;
        public Color playerColor;

        public PlayerData(Color color)
        {
            playerColor = color;
            id = currentId;
            currentId++;
        }

        public void SetLivingPlayerControl()
        {
            isControlledByAI = false;
        }

        public void ModifyCash(int cashAmount)
        {
            cash += cashAmount;
        }
    }
}

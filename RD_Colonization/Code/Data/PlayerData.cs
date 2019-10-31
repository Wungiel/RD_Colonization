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
        public int attackBonus = 1;
        public int attackDDABonus = 1;
        public int healthBonus = 0;
        public bool isControlledByAI = true;
        public Color playerColor;
        public bool isDefeated = false;
        public PlayerAISettingsData settingsAI = new PlayerAISettingsData();

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

        public float GetDDABonus()
        {
            if (attackDDABonus > 2)
            {
                return 2;
            }
            else if (attackDDABonus < 0.1)
            {
                return 0.1f;
            }
            else
            {
                return attackDDABonus;
            }
        }

        public float GetBoughtBonus()
        {
            if (attackBonus > 2)
            {
                return 2;
            }
            else if (attackBonus < 0.1)
            {
                return 0.1f;
            }
            else
            {
                return attackBonus;
            }
        }
    }
}

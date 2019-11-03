using Microsoft.Xna.Framework;
using RD_Colonization.Code.ArtificialIntelligenceModules;
using RD_Colonization.Code.Entities;
using RD_Colonization.Code.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RD_Colonization.Code.Data
{
    public class PlayerData
    {
        public int id = 0;
        public int cash = 0;
        public int food = 0;
        public int attackBonus = 1;
        public int attackDDABonus = 1;
        public int healthBonus = 0;
        public bool isControlledByAI = true;
        public Color playerColor;
        public bool isDefeated = false;
        public bool isDiscoveredByPlayer = false;
        public PlayerAISettingsData settingsAI = new PlayerAISettingsData();
        public AIModules aiModules;
        public HashSet<Tile> discoveredTiles = new HashSet<Tile>();
        public Tile[] explorationMap;

        public PlayerData(Color color, int id)
        {
            playerColor = color;
            this.id = id;
            aiModules = new AIModules(this);
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

        public void AddToDiscoveredHashset(Tile tile)
        {
            discoveredTiles.Add(tile);
        }

        public Point GetCenterOfArea()
        {
            City[] cities = CityManager.Instance.GetPlayersCities(id);
            Point centerPoint = new Point();
            foreach(City city in cities)
            {
                centerPoint = city.currentTile.position;
            }

            centerPoint.X = centerPoint.X / cities.Length;
            centerPoint.Y = centerPoint.Y / cities.Length;

            return centerPoint;
        }

    }
}

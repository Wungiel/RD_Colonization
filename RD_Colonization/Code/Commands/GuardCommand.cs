using Microsoft.Xna.Framework;
using RD_Colonization.Code.Data;
using RD_Colonization.Code.Entities;
using RD_Colonization.Code.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RD_Colonization.Code.Commands
{
    public class GuardCommand : ICommand
    {
        public Unit defender = null;
        public City cityGoal = null;
        public Unit unitGoal = null;
        public MoveCommand moveToDestinyTileCommand = null;

        public GuardCommand(Unit defender, City goal)
        {
            this.defender = defender;
            this.cityGoal = goal;
        }

        public GuardCommand(Unit defender, Unit goal)
        {
            this.defender = defender;
            this.unitGoal = goal;
        }


        //First get tiles of protected  object, stand close to it, if enemy is close then attack, if object destroyed then true
        public bool Execute()
        {
            HashSet<Tile> neigbhouringTiles = new HashSet<Tile>();
            HashSet<Tile> safetyTiles = new HashSet<Tile>();
            moveToDestinyTileCommand = null;

            if (cityGoal != null)
            {
                if (CityManager.Instance.citytDictionary.ContainsValue(cityGoal) == false)
                {
                    return true;
                }
                neigbhouringTiles = MapManager.Instance.GetNeighbours(cityGoal.currentTile, 3);
                safetyTiles = MapManager.Instance.GetNeighbours(unitGoal.currentTile, 5);
            }
            else if (unitGoal != null)
            {
                if (UnitManager.Instance.unitDictionary.ContainsValue(unitGoal) == false)
                {
                    return true;
                }

                neigbhouringTiles = MapManager.Instance.GetNeighbours(unitGoal.currentTile, 3);
                safetyTiles = MapManager.Instance.GetNeighbours(unitGoal.currentTile, 5);
            }

            if (IsEnemyNearby(safetyTiles) == true)
            {
                foreach (Tile tile in safetyTiles)
                {
                    Rectangle tileRectangle = tile.CreateRectangle();
                    if (UnitManager.Instance.unitDictionary.ContainsKey(tileRectangle))
                    {
                        if (UnitManager.Instance.unitDictionary[tileRectangle].playerId != defender.playerId)
                        {
                            moveToDestinyTileCommand = new MoveCommand(tileRectangle, defender);
                            break;
                        }
                    }
                }
                
            }
            else
            {
                if (neigbhouringTiles.Contains(defender.currentTile) == false)
                {
                    moveToDestinyTileCommand = new MoveCommand(GetPossibleTile(neigbhouringTiles), defender);
                    if (moveToDestinyTileCommand.Execute() == true)
                    {
                        moveToDestinyTileCommand = null;
                    }
                }
            }

            return false;

        }

        private bool IsEnemyNearby(HashSet<Tile> neigbhouringTiles)
        {
            foreach(Tile tile in neigbhouringTiles)
            {
                Rectangle tileRectangle = tile.CreateRectangle();
                if (UnitManager.Instance.unitDictionary.ContainsKey(tileRectangle))
                {
                    if (UnitManager.Instance.unitDictionary[tileRectangle].playerId != defender.playerId)
                        return true;
                }
            }

            return false;
        }

        private Rectangle GetPossibleTile(HashSet<Tile> neigbhouringTiles)
        {
            HashSet<Tile> possibleTiles = new HashSet<Tile>();
            Random random = new Random();

            foreach(Tile tile in possibleTiles)
            {
                if (tile.type.walkable == true)
                    possibleTiles.Add(tile);
            }
            return possibleTiles.ElementAt(random.Next(possibleTiles.Count - 1)).CreateRectangle();
        }
    }
}

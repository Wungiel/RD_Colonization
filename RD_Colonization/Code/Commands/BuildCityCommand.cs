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
    class BuildCityCommand : ICommand
    {
        public Unit unitUnderCommand = null;
        public Tile destinyTile = null;
        public MoveCommand moveToDestinyTileCommand = null;

        public BuildCityCommand(Unit unit)
        {
            unitUnderCommand = unit;
        }

        public BuildCityCommand(Unit unit, Tile tile)
        {
            unitUnderCommand = unit;
            destinyTile = tile;
        }

        public bool Execute()
        {
            if (destinyTile == null)
            {
                destinyTile = GetBestTile(MapManager.Instance.GetDiscoveredTiles(unitUnderCommand.playerId));
            }

            if (unitUnderCommand.currentTile != destinyTile)
            {
                if (moveToDestinyTileCommand == null)
                {
                    moveToDestinyTileCommand = new MoveCommand(destinyTile.CreateRectangle(), unitUnderCommand);
                }

                moveToDestinyTileCommand.Execute();
                return false;
            }

            ActionManager.Instance.BuildCity(unitUnderCommand);
            return true;
        }

        private Tile GetBestTile(Tile[] tiles)
        {
            Tile bestTile = tiles.First();

            foreach (Tile t in tiles)
            {
                if (bestTile.GetValue() < t.GetValue())
                {
                    bestTile = t;
                }
            }

            return bestTile;
        }
    }
}

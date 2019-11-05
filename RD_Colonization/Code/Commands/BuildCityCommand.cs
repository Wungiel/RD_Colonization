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
            if (destinyTile == null || CanBuildCityHere(destinyTile) == false)
            {
                destinyTile = GetBestTile(unitUnderCommand.playerId);
                if (destinyTile == null)
                {
                    return true;
                }
            }

            if (unitUnderCommand.currentTile != destinyTile)
            {
                if (moveToDestinyTileCommand == null)
                {
                    moveToDestinyTileCommand = new MoveCommand(destinyTile, unitUnderCommand);
                }

                moveToDestinyTileCommand.Execute();
                return false;
            }

            ActionManager.Instance.BuildCity(unitUnderCommand);
            return true;
        }

        private Tile GetBestTile(int playerId)
        {
            Random random = new Random();
            List<Tile> tiles = PlayerManager.Instance.GetPlayerById(playerId).discoveredTiles.ToList();
            tiles = GetTilesWithOptionToBuild(tiles);

            if (tiles.Count() == 0)
            {
                return null;
            }

            Tile bestTile = tiles[random.Next(tiles.Count)];

            foreach (Tile t in tiles)
            {
                if (bestTile.GetValue() < t.GetValue())
                {
                    bestTile = t;
                }
            }

            return bestTile;
        }

        private List<Tile> GetTilesWithOptionToBuild(List<Tile> tiles)
        {
            List<Tile> buildableTiles = new List<Tile>();

            foreach(Tile t in tiles)
            {
                if (CanBuildCityHere(t) == true)
                {
                    buildableTiles.Add(t);
                }
            }

            return buildableTiles;
        }

        private bool CanBuildCityHere(Tile tile)
        {
            return CityManager.Instance.CheckCitiesOnTiles(MapManager.Instance.GetNeighbours(tile, 2)) == false;
        }
    }
}

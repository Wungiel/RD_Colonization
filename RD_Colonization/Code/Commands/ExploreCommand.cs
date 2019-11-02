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
    public class ExploreCommand : ICommand
    {
        public Unit exploringUnit;
        public MoveCommand moveToDestinyTileCommand = null;
        public Tile tileToExplore = null;

        public ExploreCommand(Unit exploringUnit)
        {
            this.exploringUnit = exploringUnit;
        }

        public bool Execute()
        {
            if (tileToExplore == null)
            {
                tileToExplore = PlayerManager.Instance.GetPlayerByUnit(exploringUnit).explorationMap.GetRandomFromArray();
                if (tileToExplore == null)
                    return true;
            }

            if (moveToDestinyTileCommand == null)
            {
                moveToDestinyTileCommand = new MoveCommand(tileToExplore.CreateRectangle(), exploringUnit);
            }

            return moveToDestinyTileCommand.Execute();
        }

    }
}

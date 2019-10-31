using RD_Colonization.Code.Data;
using RD_Colonization.Code.Entities;
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

        public ExploreCommand(Unit exploringUnit, Tile[] borderTiles)
        {
            this.exploringUnit = exploringUnit;
        }

        public bool Execute()
        {
            return false;
        }

    }
}

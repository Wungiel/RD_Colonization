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
    public class MoveCommand : ICommand
    {
        public Rectangle destination;
        public Unit unit;
        private List<Tile> path = new List<Tile>();

        public MoveCommand(Rectangle rectangle, Unit unit)
        {
            destination = rectangle;
            this.unit = unit;
            PathfinderManager.Instance.CheckPathfinding(destination, this);
        }

        public void SetPath(List<Tile> newPath)
        {
            path = newPath;
        }

        public List<Tile> GetPath()
        {
            return path;
        }

        public bool Execute()
        {
            if (destination == null)
            {
                return true;
            }

            if (PathfinderManager.Instance.CheckPathfinding(destination, this) == false)
            {
                return true;
            }
            else
            {
                while (unit.remainingEnergy > 0)
                {
                    if (path.Count > 0)
                    {
                        UnitManager.Instance.CheckBattle(unit, path[0]);
                        if (UnitManager.Instance.unitDictionary.ContainsValue(unit))
                        {
                            UnitManager.Instance.ChangeUnitPlace(unit, path[0]);
                            path.RemoveAt(0);
                            UnitManager.Instance.DiscoverMap(unit);
                            unit.remainingEnergy--;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        return true;
                    }

                }
                return false;
            }

        }
    }
}

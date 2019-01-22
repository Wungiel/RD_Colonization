using RD_Colonization.Code.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RD_Colonization.Code.Entities
{
    class Unit
    {
        public UnitData type;
        public Tile position;

        public Unit(UnitData type, Tile position)
        {
            this.type = type;
            this.position = position;
        }
    }
}

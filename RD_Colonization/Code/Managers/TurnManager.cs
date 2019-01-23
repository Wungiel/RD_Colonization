using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RD_Colonization.Code.Managers
{
    public static class TurnManager
    {

        public static int turnNumber { get; private set; }
        public delegate void turnChanged();
        public static turnChanged turnEvent;
        

        public static void increaseTurn()
        {
            turnNumber++;
            if (turnEvent != null)
                turnEvent();
        }
    }
}

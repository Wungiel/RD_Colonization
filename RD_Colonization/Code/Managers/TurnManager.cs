using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RD_Colonization.Code.Managers
{
    public class TurnManager : BaseManager<TurnManager>
    {
        public int TurnNumber { get; private set; }
        public int CurrentPlayer { get; private set; } = 0;
        public delegate void turnChanged();
        public turnChanged turnEvent;

        public void IncreaseTurn()
        {
            TurnNumber++;
            if (turnEvent != null)
                turnEvent();
        }
    }
}

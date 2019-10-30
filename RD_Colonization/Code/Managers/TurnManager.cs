using GeonBit.UI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RD_Colonization.Code.StringList;

namespace RD_Colonization.Code.Managers
{
    public class TurnManager : BaseManager<TurnManager>
    {
        public int TurnNumber { get; private set; }
        public delegate void turnChanged();
        public turnChanged turnEvent;
        public int maxTurnsNumber = 100;

        public void IncreaseTurn()
        {
            if (CheckEndingConditions() == true)
            {
                MessageBox.ShowMsgBox("End", "Game finished ");
                TestManager.Instance.WriteTestResultData();
                ScreenManager.Instance.SetScreen(mainMenuScreenString);
                return;
            }

            PlayerManager manager = PlayerManager.Instance;

            manager.SwitchPlayer();

            if (manager.GetCurrentPlayerIndex() == 0)
            {              
                TurnNumber++;
                if (turnEvent != null)
                    turnEvent();
            }       
            
            if (manager.currentPlayer.isControlledByAI == true)
            {
                manager.ProcessTurn();
            }
        }

        private bool CheckEndingConditions()
        {
            return TurnNumber == maxTurnsNumber;
        }
    }
}

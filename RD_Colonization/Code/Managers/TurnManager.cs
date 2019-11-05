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
        public delegate void turnManagerEvent();
        public turnManagerEvent turnEvent;
        public turnManagerEvent lateTurnEvent;
        public turnManagerEvent changePlayerEvent;
        public int maxTurnsNumber = 100;

        public bool IncreaseTurn()
        {
            if (CheckEndingConditions() == true)
            {
                EndGame();
                return true;
            }

            PlayerManager manager = PlayerManager.Instance;

            changePlayerEvent();
            manager.SwitchPlayer();

            if (manager.GetCurrentPlayerIndex() == 0)
            {              
                TurnNumber++;
                if (turnEvent != null)
                    turnEvent();
                    lateTurnEvent();
            }       
            
            if (manager.currentPlayer.isControlledByAI == true)
            {
                manager.ProcessTurn();
            }

            return false;
        }

        public void EndGame()
        {
            MessageBox.MsgBoxOption acceptEndGame = new MessageBox.MsgBoxOption("Ok", Finish);
            MessageBox.MsgBoxOption[] options = { acceptEndGame };
            MessageBox.ShowMsgBox("End", "Game finished", options);
        }

        private bool Finish()
        {
            TestManager.Instance.WriteTestResultData();
            ScreenManager.Instance.SetScreen(mainMenuScreenString);
            ClearManagers();

            if (HistoryManagr.Instance.savingHistory == true)
            {
                HistoryManagr.Instance.ContinueGenerating();
            }

            return true;
        }

        private void ClearManagers()
        {
            ActionManager.Instance.DestroyInstance();
            CityManager.Instance.DestroyInstance();
            InputManager.Instance.DestroyInstance();
            JsonManager.Instance.DestroyInstance();
            MapManager.Instance.DestroyInstance();
            PathfinderManager.Instance.DestroyInstance();
            PlayerManager.Instance.DestroyInstance();
            ScoreManager.Instance.DestroyInstance();
            TestManager.Instance.DestroyInstance();
            UnitManager.Instance.DestroyInstance();
            TurnManager.Instance.DestroyInstance();
            EventSaverManager.Instance.DestroyInstance();
        }

        private bool CheckEndingConditions()
        {
            return TurnNumber == maxTurnsNumber;
        }
    }
}

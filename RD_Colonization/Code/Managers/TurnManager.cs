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
                EndGame();
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

        public void EndGame()
        {
            MessageBox.ShowMsgBox("End", "Game finished ");
            TestManager.Instance.WriteTestResultData();
            ScreenManager.Instance.SetScreen(mainMenuScreenString);
            ClearManagers();
        }

        private void ClearManagers()
        {
            ActionManager.Instance.DestroyInstance();
            CityManager.Instance.DestroyInstance();
            CivilizationManager.Instance.DestroyInstance();
            DDAEvolutionaryAIManager.Instance.DestroyInstance();
            DDAResourceManager.Instance.DestroyInstance();
            InputManager.Instance.DestroyInstance();
            JsonManager.Instance.DestroyInstance();
            MapManager.Instance.DestroyInstance();
            PathfinderManager.Instance.DestroyInstance();
            PlayerManager.Instance.DestroyInstance();
            ScoreManager.Instance.DestroyInstance();
            TestManager.Instance.DestroyInstance();
            UnitManager.Instance.DestroyInstance();
            UnitManager.Instance.DestroyInstance();
        }

        private bool CheckEndingConditions()
        {
            return TurnNumber == maxTurnsNumber;
        }
    }
}

using RD_Colonization.Code.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RD_Colonization.Code.Managers
{
    class EventSaverManager : BaseManager<EventSaverManager>
    {
        public Dictionary<int, string> eventsByPlayer = new Dictionary<int, string>();

        public void StartRegistering()
        {
            foreach (PlayerData player in PlayerManager.Instance.players)
            {
                eventsByPlayer.Add(player.id, string.Empty);
            }
            TurnManager.Instance.lateTurnEvent += ResetSavedEvents;
        }

        public string GetPlayerEvents(int id)
        {
            return eventsByPlayer[id];
        }

        public void ResetSavedEvents()
        {
            foreach (PlayerData player in PlayerManager.Instance.players)
            {
                eventsByPlayer[player.id] = String.Empty;
            }
        }

        public void SaveBuildCityEvent(int id)
        {
            if (eventsByPlayer.ContainsKey(id))
            {
                eventsByPlayer[id] += "1";
            }            
        }

        public void SaveDestroyedEnemyCityEvent(int id)
        {
            if (eventsByPlayer.ContainsKey(id))
            {
                eventsByPlayer[id] += "2";
            }
        }

        public void SaveDestroyedEnemyUnitEvent(int id)
        {
            if (eventsByPlayer.ContainsKey(id))
            {
                eventsByPlayer[id] += "3";
            }
        }


        public void SaveBuildUnitEvent(int id)
        {
            if (eventsByPlayer.ContainsKey(id))
            {
                eventsByPlayer[id] += "4";
            }
        }

    }
}

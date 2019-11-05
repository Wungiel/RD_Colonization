using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RD_Colonization.Code.StringList;

namespace RD_Colonization.Code.Data
{
    public class PlayerAISettingsData
    {
        private static Random randomizer = new Random((int)System.DateTime.Now.Ticks);
        private int aggresiveness;
        private int expansiveness;
        private int risk;

        public int Aggresiveness
        {
            get
            {
                return aggresiveness;
            }
            set
            {
                if (value > 8)
                    aggresiveness = 8;
                else if (value < 0)
                    aggresiveness = 0;
                else
                    aggresiveness = value;
            }
        }

        public int Expansiveness
        {
            get
            {
                return expansiveness;
            }
            set
            {
                if (value > 8)
                    expansiveness = 8;
                else if (value < 0)
                    expansiveness = 0;
                else
                    expansiveness = value;
            }
        }

        public int Risk
        {
            get
            {
                return risk;
            }
            set
            {
                if (value > 8)
                    risk = 8;
                else if (value < 0)
                    risk = 0;
                else
                    risk = value;
            }
        }

        public  PlayerAISettingsData()
        {
            RandomizeSettings();
        }

        public PlayerAISettingsData(String values)
        {
            string[] settings = values.Split(dot);
            if (settings.Length != 4)
            {
                RandomizeSettings();
            }
            else
            {
                aggresiveness = int.Parse(settings[0]);
                expansiveness = int.Parse(settings[2]);
                risk = int.Parse(settings[3]);

            }
        }

        public String SaveSettingsIntoString()
        {
            StringBuilder newString = new StringBuilder(String.Empty);
            newString.Append(aggresiveness.ToString());
            newString.Append(expansiveness.ToString());
            newString.Append(risk.ToString());
            return newString.ToString();
        }

        public bool GetGuardChance()
        {
            Random random = new Random();
            return (random.Next(-8, 8) > (aggresiveness - risk));
        }


        private void RandomizeSettings()
        {            
            aggresiveness = randomizer.Next(0, 8);
            expansiveness = randomizer.Next(0, 8);
            risk = randomizer.Next(0, 8);
        }


    }
}

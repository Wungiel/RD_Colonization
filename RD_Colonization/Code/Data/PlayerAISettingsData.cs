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
        private int aggresiveness;
        private int landness;
        private int expansiveness;
        private int risk;

        int Aggresiveness
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

        int Landness
        {
            get
            {
                return landness;
            }
            set
            {
                if (value > 8)
                    landness = 8;
                else if (value < 0)
                    landness = 0;
                else
                    landness = value;
            }
        }

        int Expansiveness
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

        int Risk
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
                landness = int.Parse(settings[1]);
                expansiveness = int.Parse(settings[2]);
                risk = int.Parse(settings[3]);

            }
        }

        public String SaveSettingsIntoString()
        {
            StringBuilder newString = new StringBuilder(String.Empty);
            newString.Append(aggresiveness.ToString());
            newString.Append(landness.ToString());
            newString.Append(expansiveness.ToString());
            newString.Append(risk.ToString());
            return newString.ToString();
        }

        private void RandomizeSettings()
        {
            Random randomizer = new Random();
            aggresiveness = randomizer.Next(0, 8);
            landness = randomizer.Next(0, 8);
            expansiveness = randomizer.Next(0, 8);
            risk = randomizer.Next(0, 8);
        }

    }
}

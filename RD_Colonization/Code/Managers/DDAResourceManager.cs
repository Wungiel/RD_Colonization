﻿using RD_Colonization.Code.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RD_Colonization.Code.Managers
{
    public class DDAResourceManager : BaseManager<DDAResourceManager>
    {
        public HashSet<PlayerData> noticedPlayers = new HashSet<PlayerData>();
    }
}

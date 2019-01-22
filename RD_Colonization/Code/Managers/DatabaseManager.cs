using RD_Colonization.Code.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RD_Colonization.Code.Managers
{
    static class DatabaseManager
    {

        private static readonly string connectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=GameData;Integrated Security=True;Pooling=False";
        private static string queryLoadString = 
            "SELECT MapID, MapSize, MapData from dbo.Map "
                + "WHERE MapID = 1;";

        static DatabaseManager()
        {

        }

        public static MapData loadData()
        {
            //using (SqlConnection connection =
            //new SqlConnection(connectionString))

                return null;
        }

        public static void saveData()
        {

        }
    }
}

using Microsoft.Xna.Framework;
using RD_Colonization.Code.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RD_Colonization.Code.StringList;

namespace RD_Colonization.Code.Managers
{
    static class DatabaseManager
    {

        private static readonly string connectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=GameData;Integrated Security=True;Pooling=False";
        private static string queryLoadString =
            "SELECT MapID, MapSize, MapData from GameData.Map;";
        private static string querySaveString =
            "INSERT INTO GameData.Map(MapSize, MapData) "
                + "VALUES  (@MapSize, @MapData)";
        private static string deleteSavesString =
            "DELETE from GameData.Map ";

        static DatabaseManager()
        {

        }

        public static MapData loadData()
        {
            List<MapData> dataList = new List<MapData>();

            using (SqlConnection connection =
            new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryLoadString, connection);
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        dataList.Add(new MapData((int) reader[1], (string) reader[2]));
                    }
                    reader.Close();
                }
            }

            if (dataList.Count == 0)
                return null;
            else
                return dataList[0];
        }

        public static void saveData()
        {            
            StringBuilder mapData = new StringBuilder();
            var tileValues = from tiles in MapManager.mapDictionary
                             select tiles.Value;

            foreach(Tile t in tileValues)
            {
                if (t.type.name == waterString)
                    mapData.Append("0");
                else if (t.type.name == grassString)
                    mapData.Append("1");
                else
                    mapData.Append("2");
            }

            using (SqlConnection connection =
            new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(querySaveString, connection);
                {
                    
                    command.Parameters.Add("@MapSize", SqlDbType.Int).Value = MapManager.mapSize;
                    command.Parameters.Add("@MapData", SqlDbType.Text).Value = mapData.ToString();
                    connection.Open();
                    command.ExecuteNonQuery();
                    
                }

            }

        }


        internal static void removeData()
        {
            using (SqlConnection connection =
            new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(deleteSavesString, connection);
                {
                    connection.Open();
                    command.ExecuteNonQuery();

                }
            }
        }

    }
}

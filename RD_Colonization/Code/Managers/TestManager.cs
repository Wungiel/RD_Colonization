using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RD_Colonization.Code.Data;
using static RD_Colonization.Code.StringList;

namespace RD_Colonization.Code.Managers
{
    public class TestManager : BaseManager<TestManager>
    {
        public List<String> GetTestFiles()
        {
            string testDirectoryPath = System.IO.Directory.GetCurrentDirectory() + slash + testDataFolderString;

            if (Directory.Exists(testDirectoryPath) == false)
            {
                Directory.CreateDirectory(testDirectoryPath);
            }            

            return Directory.EnumerateFiles(testDirectoryPath, "*", SearchOption.AllDirectories).Select(Path.GetFileName).ToList();
        }

        public TestData GetTestData(string testName)
        {
            string filePath = System.IO.Directory.GetCurrentDirectory() + slash + testDataFolderString + slash + testName;

            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                return JsonManager.Instance.ReadJSON<TestData>(json);
            }
            else
            {
                return null;
            }
        }
    }
}

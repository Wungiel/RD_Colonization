using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace RD_Colonization.Code.Managers
{
    public class JsonManager : BaseManager<JsonManager>
    {

        public T ReadJSON <T> (String jsonString) where T: new()
        {
            T newObject;

            try
            {
                newObject = JsonConvert.DeserializeObject<T>(jsonString);
            }
            catch (Exception)
            {
                newObject = new T();
            }
            
            return newObject;
        }

        public String WriteIntoJson <T> (T original)
        {
            return JsonConvert.SerializeObject(original);
        }
    }
}

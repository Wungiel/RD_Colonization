using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RD_Colonization.Code
{
    public abstract class BaseManager<T> where T: BaseManager<T>, new()
    {
        private static T instance;
    
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new T();
                }
                return instance;
            }
            private set
            {
                instance = value;
            }
        }

        public void DestroyInstance()
        {
            Instance = null;
        }
    }
}

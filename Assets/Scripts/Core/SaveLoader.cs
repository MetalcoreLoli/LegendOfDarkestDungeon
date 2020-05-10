using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Core
{
    public class SaveLoader
    {
        public bool IsNeedToLoad { get; set; }
        private static SaveLoader _instance;
        public static SaveLoader Instance()
        {
            if (_instance == null)
                _instance = new SaveLoader();

            return _instance;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Core.Data
{
    interface IData
    {
        Dictionary<string, int> GetData();
        void LoadData(Dictionary<string, int> data);
    }
}

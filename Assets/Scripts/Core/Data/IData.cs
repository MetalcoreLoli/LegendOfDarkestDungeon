using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Core.Data
{
    interface IData<TKey, TValue>
    {
        Dictionary<TKey, TValue> GetData();
        void LoadData(Dictionary<TKey, TValue> data);
    }
}

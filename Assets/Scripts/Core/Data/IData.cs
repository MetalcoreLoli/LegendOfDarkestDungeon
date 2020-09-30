using System.Collections.Generic;

namespace Assets.Scripts.Core.Data
{
    internal interface IData<TKey, TValue>
    {
        Dictionary<TKey, TValue> GetData();

        void LoadData(Dictionary<TKey, TValue> data);
    }
}
using Assets.Scripts.Items;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Core
{
    public class ItemManager : MonoBehaviour
    {
        public GameObject[] Items;

        public void DropAt(Vector3 position, string name)
        {
            GameManager.Instance.board.SpawnObject(position, GetItem(name).gameObject);
        }

        public Item GetItem(string name)
        {
            return Items.FirstOrDefault(item => item.name == name).GetComponent<Item>();
        }
    }
}
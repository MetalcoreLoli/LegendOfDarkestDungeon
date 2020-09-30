using UnityEngine;

namespace Assets.Scripts.Dungeon.Factory
{
    public class DungeonFactoryManager : MonoBehaviour
    {
        private DefaultDungeonFactory dungeonFactory;
        private BlueDungeonFactory dungeonBlueFactory;

        public static DungeonFactoryManager instance;
        public DefaultDungeonFactory DefaultDungeonFactory { get => dungeonFactory; private set => dungeonFactory = value; }
        public BlueDungeonFactory BlueDungeonFactory { get => dungeonBlueFactory; private set => dungeonBlueFactory = value; }

        //public static DungeonFactoryManager Get()
        //{
        //    return GameObject.Find("DungeonManager").GetComponent<DungeonFactoryManager>();
        //}

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else if (instance != this)
                DestroyImmediate(gameObject);

            DontDestroyOnLoad(gameObject);
            dungeonFactory = GetComponent<DefaultDungeonFactory>();
            dungeonFactory.Size = new Vector2(70, 40);
            dungeonBlueFactory = GetComponent<BlueDungeonFactory>();
            dungeonBlueFactory.Size = new Vector2(70, 40);
        }
    }
}
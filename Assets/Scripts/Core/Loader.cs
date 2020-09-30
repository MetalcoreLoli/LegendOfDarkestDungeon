using Assets.Scripts.Dungeon.Factory;
using UnityEngine;

public class Loader : MonoBehaviour
{
    public GameObject gameManager;
    public GameObject dungeonManager;

    // Start is called before the first frame update
    private void Awake()
    {
        if (DungeonFactoryManager.instance == null)
        {
            Instantiate(dungeonManager);
        }

        if (GameManager.Instance == null)
        {
            Instantiate(gameManager);
        }
    }
}
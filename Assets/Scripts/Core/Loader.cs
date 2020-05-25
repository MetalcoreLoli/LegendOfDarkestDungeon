using Assets.Scripts.Dungeon.Factory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour
{
    public GameObject gameManager;
    public GameObject dungeonManager;
    // Start is called before the first frame update
    void Awake()
    {
        if (DungeonFactoryManager.instance == null)
        {
            Instantiate(dungeonManager);
        }

        if (GameManager._instance == null)
        { 
            Instantiate(gameManager);
        }
    }
}

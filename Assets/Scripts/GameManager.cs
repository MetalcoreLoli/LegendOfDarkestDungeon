using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager _instance = null;

    public GameObject Player;

    [HideInInspector]public bool playersTurn; 

    public BoardManager boardManager;
    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else if (_instance != this)
            Destroy(gameObject);


        DontDestroyOnLoad(gameObject);
        boardManager = GetComponent<BoardManager>();
        Init();

      
    }

    private void OnLevelWasLoaded(int level)
    {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;

    }

    public void GameOver()
    {
        enabled = false;
    }

    //[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    //public static void CallBackInit()
    //{
    //    SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    //}

    private static void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        _instance.Init();
    }

    private void Init()
    {
        _instance.playersTurn = true;
        boardManager.SetUpLevel(0);
        var room = boardManager.Rooms.FirstOrDefault();

        var Player = GameObject.FindGameObjectWithTag("Player");
        Player.transform.position = room.GetCenter();


    }
}

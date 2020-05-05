using Assets.Scripts.Dices;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager _instance = null;

    public GameObject Player;


    public List<Enemy> Enemies;
    [HideInInspector]public bool playersTurn; 

    public BoardManager boardManager;

    public int playersHp    = 20;
    public int playersMaxHp = 20;
    public int playersMp    = DiceManager.RollUndSumFromString("2d6") * 55;
    public int playersMaxMp    = DiceManager.RollUndSumFromString("2d6") * 55;

    public int FloorNumber = 1;
    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else if (_instance != this)
            Destroy(gameObject);


        DontDestroyOnLoad(gameObject);
        boardManager = GetComponent<BoardManager>();
        Enemies = new List<Enemy>();
        Init();
      
    }

    //private void OnLevelWasLoaded(int level)
    //{
    //    SceneManager.sceneLoaded += SceneManager_sceneLoaded;

    //}

    public void GameOver()
    {
        enabled = false;
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void CallBackInit()
    {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    private static void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        _instance.FloorNumber += 1;
        _instance.Init();
    }

    private void Init()
    {
        Enemies.Clear();
        _instance.playersTurn = true;
        boardManager.SetUpLevel(FloorNumber);
        var room = boardManager.Rooms.FirstOrDefault();

        var Player = GameObject.FindGameObjectWithTag("Player");
        Player.transform.position = room.GetCenter();

        var player_comp = Player.GetComponent<Player>();
        var players_light = GameObject.FindGameObjectWithTag("PlayersLight").GetComponent<Light>();
        players_light.intensity = player_comp.Hp = playersHp;
        player_comp.MaxHp = _instance.playersMaxHp;
        player_comp.Mana = _instance.playersMaxMp;
        player_comp.MaxMana = _instance.playersMaxMp;

    }

    private void Update()
    {
        //if (playersTurn)
        //    return;
       // StartCoroutine(MoveEnemies());
        
    }

    IEnumerator MoveEnemies()
    {
        Enemies.RemoveAll(i => i == null);
        yield return new WaitForSeconds(.1f);
        foreach (var enemy in Enemies)
        {
            if (enemy != null)
            {
                enemy.MoveEnemy();
                yield return new WaitForSeconds(enemy.moveTime);
            }
        }

        playersTurn = true;
    }
}

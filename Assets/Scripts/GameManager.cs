using Assets.Scripts.Dices;
using Assets.Scripts.Stats;
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

    public ActorCharacteristics playerCharacteristics;

    public int FloorNumber = 1;
    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else if (_instance != this)
            Destroy(gameObject);

       

        DontDestroyOnLoad(gameObject);
        boardManager = GetComponent<BoardManager>();
        playerCharacteristics = new ActorCharacteristics(25, DiceManager.RollUndSumFromString("4d6") * 6);
        Enemies = new List<Enemy>();
        Init();
      
    }

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

        var player_comp             = Player.GetComponent<Player>();
        var players_light           = GameObject.FindGameObjectWithTag("PlayersLight").GetComponent<Light>();
        players_light.intensity     = playerCharacteristics.Hp;
        playerCharacteristics.Mp    = playerCharacteristics.MaxMp;
        player_comp.Characteristics = playerCharacteristics;

    }

    public void RemoveEnemy(Enemy enemy)
    {
       // Enemies.Remove(enemy);
    }

    private void FixedUpdate()
    {
        //if (playersTurn)
        //    return;
        StartCoroutine(MoveEnemies());
       
    }

    IEnumerator MoveEnemies()
    {
        Enemies.RemoveAll(i => i == null);
        yield return new WaitForSeconds(.1f);
        foreach (var enemy in Enemies.Where(e => e.isActiveAndEnabled))
        {
            yield return new WaitForSeconds(enemy.moveTime);
            enemy.MoveEnemy();
        }

        playersTurn = true;
    }
}

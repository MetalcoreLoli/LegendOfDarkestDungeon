#define TEST
using Assets.Scripts.Actors;
using Assets.Scripts.Core;
using Assets.Scripts.Dices;
using Assets.Scripts.Stats;
using Assets.Scripts.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager _instance = null;

    public Player Player;


    public List<Enemy> Enemies;
    [HideInInspector]public bool playersTurn; 

    public BoardManager boardManager;
    public DataManager  dataManager;
    public ShortcutMenu shortcutMenu;
    public InventoryManager inventoryManager;
    public ItemManager itemManager;
    public GameObject potion;
    public ActorCharacteristics playerCharacteristics;

    public int FloorNumber = 1;
    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else if (_instance != this)
            Destroy(gameObject);

       

        DontDestroyOnLoad(gameObject);

        itemManager             = GetComponent<ItemManager>();
        boardManager            = GetComponent<BoardManager>();
        shortcutMenu            = GetComponent<ShortcutMenu>();
        dataManager             = GetComponent<DataManager>();
        inventoryManager        = GetComponent<InventoryManager>();

        Player = GameObject.Find("Player").GetComponent<Player>();
        if (SaveLoader.Instance().IsNeedToLoad)
        {
            dataManager.LoadSavedData();
        }
        else
        {
            playerCharacteristics = new ActorCharacteristics(25, DiceManager.RollUndSumFromString("4d6") * 6);
            
        }

        Enemies = new List<Enemy>();

#if UNITY_EDITOR 
        Init();
#endif
      
    }
    private void Start()
    {
        var ui = GameObject.Find("HUDCanvas").GetComponent<UIController>();
        if (!SaveLoader.Instance().IsNeedToLoad)
        {
            ui.crtMenu.Open();
        }

    }

    public void GameOver()
    {
        enabled = false;
    }
    
    public void UpdatePlayersCharacteristics(ActorCharacteristics actorCharacteristics)
    {
        if (Player == null) return;
        Player.Characteristics = actorCharacteristics;
        playerCharacteristics = actorCharacteristics;
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void CallBackInit()
    {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    private static void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (_instance != null)
        {
            _instance.FloorNumber += 1;
            _instance.Init();
        }
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

        shortcutMenu.Init();

    }

    public void RemoveEnemy(Enemy enemy)
    {
       // Enemies.Remove(enemy);
    }

    private void FixedUpdate()
    {
        //if (playersTurn)
        //    return;
        ///StartCoroutine(MoveEnemies());
       
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
    private void OnDestroy()
    {
        foreach (var item in Enemies)
        {
            Destroy(item);
        }
    }
}

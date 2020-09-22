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
    public GameObject messageBox;
    public Player Player;


    public List<Enemy> Enemies;
    [HideInInspector] public bool playersTurn;

    public Board board;
    public static MessageBox MessageBox;
    public DataManager dataManager;
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
        messageBox.SetActive(false);
        var msg = Instantiate(messageBox);

        MessageBox = msg.GetComponent<MessageBox>();
        DontDestroyOnLoad(MessageBox);

        itemManager         = GetComponent<ItemManager>();
        board               = GetComponent<Board>();
        shortcutMenu        = GetComponent<ShortcutMenu>();
        dataManager         = GetComponent<DataManager>();
        inventoryManager    = GetComponent<InventoryManager>();

        Player = GameObject.Find("Player").GetComponent<Player>();

        if (SaveLoader.Instance().IsNeedToLoad)
        {
            dataManager.LoadSavedData();
        }
        else
        {

            playerCharacteristics = new ActorCharacteristics(50, DiceManager.RollUndSumFromString("4d6") * 7);
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

        var ui = GameObject.Find("HUDCanvas").GetComponent<UIController>();
        if (!ui.crtMenu.IsOpen && MessageBox.DialogResult == DialogResult.None)
        { 
            //MessageBox = Instantiate(messageBox).GetComponent<MessageBox>();
            MessageBox.Show(":CCCCC", "You DEAD");
        }
        if (MessageBox.DialogResult != DialogResult.None)
        {
            StopAllCoroutines();
            Enemies = new List<Enemy>();
            Destroy (GameObject.Find("SoundManager").gameObject);
            Destroy (gameObject);
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }

        //if (dialogResult == DialogResult.OK || dialogResult == DialogResult.Cancel) 
        //{
        //    //SceneManager.LoadScene(0, LoadSceneMode.Single);
        //    Debug.Log(dialogResult+ "on gameover");
        //}
    }

    public void UpdatePlayersCharacteristics(ActorCharacteristics actorCharacteristics)
    {
        if (Player == null) return;
        var actor = Player.GetComponent<Actor>();
        actor.Characteristics = actorCharacteristics;

        var ui = GameObject.Find("HUDCanvas").GetComponent<UIController>();

        ui.HpController.SetMax(actor.Characteristics.MaxHp);
        ui.HpController.SetValue(actor.Characteristics.Hp);

        ui.MpController.SetMax(actor.Characteristics.MaxMp);
        ui.MpController.SetValue(actor.Characteristics.Mp);
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
        if (enabled == false)
            enabled = true;
        Enemies = new List<Enemy>();
        _instance.playersTurn = true;
        board.SetUpLevel(FloorNumber);
        var room = board.Rooms.FirstOrDefault();
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        Player.transform.position = room.GetCenter();

        var players_light = GameObject.FindGameObjectWithTag("PlayersLight").GetComponent<Light>();
        players_light.intensity = 25;
        Player.GetComponent<Actor>().Characteristics = playerCharacteristics;

        shortcutMenu.Init();

    }

    private void Update()
    {
    }


    //IEnumerator MoveEnemies()
    //{
    //    yield return new WaitForSeconds(0.5f);
    //    if (Enemies.Count > 0)
    //    {
    //        foreach (var enemy in Enemies)
    //        {
    //            yield return new WaitForSeconds(enemy.moveTime);
    //            if (enemy != null)
    //            {
    //                //Debug.Log($"{(enemy.gameObject == null ? "null" : "not null")}");
    //                enemy.MoveEnemy();
    //            }
    //        }
    //    }
    //    playersTurn = true;
    //}
    private void OnDestroy()
    {
        foreach (var item in Enemies)
            item.TakeDamage(item.characteristics.MaxHp * 2, false);
    }
}

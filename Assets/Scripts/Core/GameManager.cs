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
    public static GameManager Instance = null;
    public GameObject messageBox;
    public GameObject PlayerObject;
    public Player Player;
    public Actor PlayerActor;

    public List<Enemy> Enemies;
    [HideInInspector] public bool playersTurn;

    public Board board;
    public static MessageBox MessageBox;
    public DataManager dataManager;
    public InventoryManager inventoryManager;
    public ItemManager itemManager;
    public GameObject potion;
    public ActorCharacteristics playerCharacteristics;

    public int FloorNumber = 1;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        PlayerObject = Instantiate(PlayerObject);
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(PlayerObject);

        PlayerObject.GetComponent<Casting>().Caster = PlayerObject.transform;

        messageBox.SetActive(false);
        var msg = Instantiate(messageBox);

        MessageBox = msg.GetComponent<MessageBox>();
        DontDestroyOnLoad(MessageBox);

        itemManager = GetComponent<ItemManager>();
        board = GetComponent<Board>();
        dataManager = GetComponent<DataManager>();
        inventoryManager = GetComponent<InventoryManager>();

        Player = PlayerObject?.GetComponent<Player>();
        PlayerActor = PlayerObject?.GetComponent<Actor>();

        if (SaveLoader.Instance().IsNeedToLoad)
        {
            dataManager.LoadSavedData();
        }
        else
        {
            playerCharacteristics = new ActorCharacteristics(50, DiceManager.RollUndSumFromString("4d6") * 7);
        }

#if UNITY_EDITOR
        Init();
#endif
    }

    private void Start()
    {
        var ui = GameObject.Find("HUDCanvas").GetComponent<UIController>();

        PlayerActor.OnHealthUpdate += (s, e) => ui.HpController.SetValue(e);
        PlayerActor.OnManaUpdate += (s, e) => ui.MpController.SetValue(e);

        if (!SaveLoader.Instance().IsNeedToLoad)
        {
            //ui.crtMenu.Open();
        }
    }

    public void GameOver()
    {
        var ui = GameObject.Find("HUDCanvas").GetComponent<UIController>();
       // if (!ui.crtMenu.IsOpen && MessageBox.DialogResult == DialogResult.None)
       // {
       //     //MessageBox = Instantiate(messageBox).GetComponent<MessageBox>();
       //     MessageBox.Show(":CCCCC", "You DEAD");
       // }
        if (MessageBox.DialogResult != DialogResult.None)
        {
            StopAllCoroutines();
            Enemies = new List<Enemy>();
            Destroy(GameObject.Find("SoundManager").gameObject);
            Destroy(gameObject);
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
        var actor = PlayerActor;
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
        if (Instance != null)
        {
            Instance.FloorNumber += 1;
            Instance.Init();
        }
    }

    private void Init()
    {
        if (enabled == false)
            enabled = true;
        Enemies = new List<Enemy>();
        Instance.playersTurn = true;
        board.SetUpLevel(FloorNumber);
        var room = board.dungeonGenerator.Rooms.FirstOrDefault();
        //PlayerObject = Instantiate(PlayerObject);

        Player.transform.position = room.GetCenter();

        //var players_light = GameObject.FindGameObjectWithTag("PlayersLight").GetComponent<Light>();
        //players_light.intensity = 25;
        Player.GetComponent<Actor>().Characteristics = playerCharacteristics;
    }

    private void Update()
    {
        StartCoroutine(MoveEnemies());
    }

    private IEnumerator MoveEnemies()
    {
        yield return new WaitForSeconds(0.5f);
        if (Enemies.Count > 0)
        {
            foreach (var enemy in Enemies)
            {
                yield return new WaitForSeconds(enemy.moveTime);
                if (enemy != null)
                {
                    //Debug.Log($"{(enemy.gameObject == null ? "null" : "not null")}");
                    enemy.MoveEnemy();
                }
            }
        }
        playersTurn = true;
    }

    private void OnDestroy()
    {
        //foreach (var item in Enemies)
        //    item.TakeDamage(item.characteristics.MaxHp * 2, false);
    }
}
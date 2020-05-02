using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
        else if (_instance != null)
            Destroy(gameObject);

        _instance.playersTurn = true;

        //DontDestroyOnLoad(gameObject);
        boardManager = GetComponent<BoardManager>();
        Init();
    }

    private void GameOver()
    {
        enabled = false;
    }
    private void Init()
    {
        boardManager.SetUpLevel(1);
        var room = boardManager.Rooms.FirstOrDefault();

        var player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = room.GetCenter();
        //if (room != null)
        //    PlayerCam.transform.Translate(PlayerCam.transform.localPosition + room.Location);

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

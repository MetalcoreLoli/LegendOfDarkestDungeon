using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager _instance = null;

    public GameObject Player;

    public BoardManager boardManager;
    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else if (_instance != null)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        boardManager = GetComponent<BoardManager>();
        Init();
    }

    private void Init()
    {
        Debug.Log("Start");
        boardManager.SetUpLevel(1);
        Debug.Log("Start board");
        var room = boardManager.Rooms.FirstOrDefault();

        Instantiate(Player, room.GetCenter(), Quaternion.identity);
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

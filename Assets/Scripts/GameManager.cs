using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager _instance = null;

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
        boardManager.SetUpLevel(1);
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

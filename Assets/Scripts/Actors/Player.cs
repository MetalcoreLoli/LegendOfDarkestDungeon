using Assets.Scripts.Actors;
using Assets.Scripts.Core;
using Assets.Scripts.Core.Data;
using Assets.Scripts.Dices;
using Assets.Scripts.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum LookDir
{
    Up = 0,
    Down = 1,
    Left = 2,
    Right = 3
}

public class Player : MovingObject, IData<string, int>
{
    public float speed = 5f;

    private float restartLevelDelay = 1f;
    private float manaRegenarationTime = 15f;
    private Animator animator;

    private float horizontal = 0;
    private float vertical = 0;

    private int level = 1;

    [SerializeField] private int exp;
    [SerializeField] private int maxExp = 20;

    public Camera Camera;

    private bool isFlipLeftRight = false;
    private bool isFlipUpDown = false;

    [NonSerialized] private Actor actor;
    public static Player Instance = null;

    public int Level { get => level; internal set => level = value; }
    public int Exp { get => exp; internal set => exp = value; }
    public int MaxExp { get => maxExp; internal set => maxExp = value; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }

    protected override void Start()
    {
        animator = GetComponent<Animator>();
        actor = GameManager.Instance.PlayerActor;

        //#if UNITY_EDITOR
        //        actor.Characteristics.Mp = actor.Characteristics.MaxMp = 25000;
        //#endif

        //GameManager.Instance.shortcutMenu.AddToShortcutMenu(GameManager.Instance.potion, 4);

        StartCoroutine(RegenerateMana(manaRegenarationTime));
        base.Start();
    }

    // Update is called once per frame
    private void Update()
    {
        if (GameManager.Instance.playersTurn == false) return;
        var ui = GameObject.Find("HUDCanvas").GetComponent<UIController>();
        if (!ui.crtMenu.IsOpen)
        {
            horizontal = (Input.GetAxisRaw("Horizontal"));
            vertical = (Input.GetAxisRaw("Vertical"));

            if (GameInput.GetKeyDown("Use"))
            {
            }
        }

        if (horizontal != 0)
        {
            if (horizontal > 0)
                lookDir = LookDir.Right;
            else
                lookDir = LookDir.Left;

            if (horizontal > 0 && !isFlipLeftRight)
            {
                ///GameObject.FindGameObjectWithTag("PlayersLight").transform.Rotate(0.0f, -180.0f, 0.0f);
                FlipLeftRight();
            }
            if (horizontal < 0 && isFlipLeftRight)
            {
                FlipLeftRight();
            }
            vertical = 0;
        }
        if (vertical != 0)
        {
            if (vertical > 0)
                lookDir = LookDir.Up;
            else
                lookDir = LookDir.Down;

            if (vertical > 0 && !isFlipUpDown)
            {
                FlipFirePointUpDown();
            }

            if (vertical < 0 && isFlipUpDown)
            {
                FlipFirePointUpDown();
            }

            //FliLeftRight();
            horizontal = 0;
        }

        if (horizontal != 0 || vertical != 0)
        {
            Vector3 inputDir = new Vector3(horizontal, vertical).normalized;
            float inputAngel = (Mathf.Atan2(inputDir.y, inputDir.x)) * Mathf.Rad2Deg;
            Debug.DrawRay(transform.position, inputDir * 3, Color.white);
            GameObject.Find("FirePoint").transform.eulerAngles = new Vector3(0, 0, inputAngel);
        }

        //GameObject.Find("FirePoint").transform.eulerAngles = Vector3.up * inputAngel;
        ////transform.eulerAngles = Vector2.up * -angel;
    }

    private IEnumerator RegenerateMana(float time)
    {
        while (true)
        {
#if UNITY_EDITOR
            time = 10f;
#endif
            yield return new WaitForSeconds(time);
            Debug.Log("Mp was restored " + Time.time);
            int mana = DiceManager.RollDice("1d4");
            GameManager.Instance.PlayerActor.UpdateMana(mana);
        }
    }

    private void FlipLeftRight()
    {
        isFlipLeftRight = !isFlipLeftRight;
        var vec = transform.localScale;
        vec.x *= -1;
        transform.localScale = vec;

        //GameObject.FindGameObjectWithTag("PlayersFirePoint").transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    private void FlipFirePointUpDown()
    {
        isFlipUpDown = !isFlipUpDown;
        var firePoint = GameObject.FindGameObjectWithTag("PlayersFirePointUp");
        var vec = firePoint.transform.localPosition;
        vec.y *= -1;
        firePoint.transform.localPosition = vec;

        if (lookDir == LookDir.Down)
        {
            var veca = firePoint.transform.localEulerAngles;
            veca.z = (transform.localScale.x < 0) ? 90f : -90f;
            firePoint.transform.localEulerAngles = veca;
        }
        if (lookDir == LookDir.Up)
        {
            var veca = firePoint.transform.localEulerAngles;
            veca.z = (transform.localScale.x < 0) ? -90f : 90f;
            firePoint.transform.localEulerAngles = veca;
        }
    }

    private void FixedUpdate()
    {
        // rb2D.MovePosition(rb2D.position + new Vector2(horizontal, vertical) * speed * Time.fixedDeltaTime);

        if (horizontal != 0 || vertical != 0)
        {
            AttemptMove<Enemy>((int)(horizontal), (int)(vertical));
            //if (GameManager.Instance.playersTurn)
            //{
            //    GameManager.Instance.playersTurn = false;
            //}
            //IsMoving = false;
        }
        // GameManager.Instance.playersTurn = false;
    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == ("Exit"))
        {
            Invoke("Restart", restartLevelDelay);
        }
    }

    public void UpdateHealth(int value)
    {
        actor.Characteristics.Hp += value;
        //if (value > 0)
        //    TextPopUp.CreateWithColor(transform.position, "+" + value, DamageDealer.Text.transform, Color.green);

        if (actor.Characteristics.Hp > actor.Characteristics.MaxHp)
        {
            actor.Characteristics.Hp = actor.Characteristics.MaxHp;
        }
        else if (actor.Characteristics.Hp < 0)
        {
            actor.Characteristics.Hp = 0;
#if UNITY_EDITOR == false
            enabled = false;
            GameManager._instance.GameOver();
#endif
        }
    }

    public virtual void UpdateExp(int value)
    {
        exp += value;
        if (exp >= maxExp)
        {
            Level++;
            var lvlMenu = GameObject.Find("HUDCanvas").GetComponent<UIController>().lvlMenu;
            lvlMenu.Points = lvlMenu.MaxPoints += 2;
            maxExp *= 2;
            lvlMenu.Open();
            actor.Characteristics.Hp = actor.Characteristics.MaxHp;
            actor.Characteristics.Mp = actor.Characteristics.MaxMp;
        }
        //TextPopUp.CreateWithColor(transform.position, "+" + value, DamageDealer.Text.transform, Color.yellow);
    }

    public void LoseHp(int damage)
    {
        UpdateHealth(-damage);
        //TextPopUp.CreateWithColor(transform.position, "-" + damage, DamageDealer.Text.transform, Color.red);
        animator.SetTrigger("Hit");
    }

    public bool PlayerCastSpell(int cost)
    {
        if (GameManager.Instance.PlayerActor.Characteristics.Mp - cost >= 0)
        {
            GameManager.Instance.PlayerActor.UpdateMana(-cost);
            // animator.SetTrigger("Casting2");
            return true;
        }
        return false;
    }

    protected override void OnCantMove<T>(T comp)
    {
        Enemy enemy = comp as Enemy;
        //if (DiceManager.TwentyEdges.Roll() > 15)
        //          LoseHp(enemy.Damage);
    }

    public Dictionary<string, int> GetData()
    {
        Dictionary<string, int> temp = new Dictionary<string, int>();

        //temp.Add("Hp",      actor.Characteristics.Hp);
        //temp.Add("MaxHp",   actor.Characteristics.MaxHp);

        //temp.Add("Mp",      actor.Characteristics.Mp);
        //temp.Add("MaxMp",   actor.Characteristics.MaxMp);

        //temp.Add("Lucky",           actor.Characteristics.Lucky);
        //temp.Add("Dexterity",       actor.Characteristics.Dexterity);
        //temp.Add("Strength",        actor.Characteristics.Strength);
        //temp.Add("Charisma",        actor.Characteristics.Charisma);
        //temp.Add("Intelligence",    actor.Characteristics.Intelligence);

        //temp.Add("Exp",    Exp);
        //temp.Add("MaxExp",    MaxExp);

        //temp.Add("Level",    Level);

        return temp;
    }

    public void LoadData(Dictionary<string, int> data)
    {
        //actor.Characteristics.Hp      = data["Hp"];
        //actor.Characteristics.MaxHp   = data["MaxHp"];
        //actor.Characteristics.Mp      = data["Mp"];
        //actor.Characteristics.MaxMp   = data["MaxMp"];

        //actor.Characteristics.Lucky           = data["Lucky"];
        //actor.Characteristics.Dexterity       = data["Dexterity"];
        //actor.Characteristics.Strength        = data["Strength"];
        //actor.Characteristics.Intelligence    = data["Intelligence"];
        //actor.Characteristics.Charisma        = data["Charisma"];

        //Exp     = data["Exp"];
        //MaxExp  = data["MaxExp"];

        //Level   = data["Level"];

        //GameManager.Instance.playeractor.Characteristics = actor.Characteristics;
        //GameManager.Instance.Player = this;
    }
}
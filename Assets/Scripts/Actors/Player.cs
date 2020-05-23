using Assets.Scripts.Core;
using Assets.Scripts.Core.Data;
using Assets.Scripts.Dices;
using Assets.Scripts.Stats;
using Assets.Scripts.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum LookDir
{ 
    Up      = 0,
    Down    = 1,
    Left    = 2,
    Right   = 3
}

public class Player : MovingObject, IData
{
    public float speed = 5f;

    private float restartLevelDelay = 1f;
    private float manaRegenarationTime = 15f;
    private Animator animator;

    private float horizontal  = 0;
    private float vertical    = 0;

    public int MaxHp;
    public int Hp;

    public int Mana;
    public int MaxMana;

    public Camera Camera;
    
    private bool isFlipLeftRight    = false;
    private bool isFlipUpDown       = false;
    [SerializeField] private DamageDealer damageDealer;
    public LookDir lookDir = LookDir.Left;

    public ActorCharacteristics Characteristics;
    public  DamageDealer DamageDealer { get => damageDealer; }
    protected override void Start()
    {
        animator = GetComponent<Animator>();
        Hp      = Characteristics.Hp;
        MaxHp   = Characteristics.MaxHp;
        Mana    = Characteristics.Mp;
        MaxMana = Characteristics.MaxMp;

#if UNITY_EDITOR
        Characteristics.Mp = Characteristics.MaxMp = 25000;
#endif



        GameManager._instance.shortcutMenu.AddToShortcutMenu(GetComponent<Casting>().SpellPrefabs[0], 0);
        GameManager._instance.shortcutMenu.AddToShortcutMenu(GetComponent<Casting>().SpellPrefabs[1], 1);
        GameManager._instance.shortcutMenu.AddToShortcutMenu(GetComponent<Casting>().SpellPrefabs[2], 2);
        //GameManager._instance.shortcutMenu.AddToShortcutMenu(GameManager._instance.potion, 4);

        StartCoroutine(RegenerateMana(manaRegenarationTime));

        base.Start();
    }

    // Update is called once per frame
    void Update()
    {

        if (GameManager._instance.playersTurn == false) return;
        var ui = GameObject.Find("HUDCanvas").GetComponent<UIController>();
        if (!ui.crtMenu.IsOpen)
        { 

            horizontal  = (Input.GetAxisRaw("Horizontal"));
            vertical    = (Input.GetAxisRaw("Vertical"));
            
            if (GameInput.GetKeyDown("Use"))
            {
                GameManager._instance.shortcutMenu.ActivateCell();
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
        while(true)
        {
#if UNITY_EDITOR
            time = 10f;
#endif
            yield return new WaitForSeconds(time);
            Debug.Log("Mp was restored " + Time.time);
            int mana = DiceManager.RollDice("1d4");
            UpdateMana(mana);
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
            //if (GameManager._instance.playersTurn)
            //{
            //    GameManager._instance.playersTurn = false;
            //}
            //IsMoving = false;
        }
       // GameManager._instance.playersTurn = false;
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
        Characteristics.Hp += value;
        if (value > 0)
            TextPopUp.CreateWithColor(transform.position, "+" + value, DamageDealer.Text.transform, Color.green);

        if (Characteristics.Hp > Characteristics.MaxHp)
        {
            Characteristics.Hp = Characteristics.MaxHp;
        }
        else if (Characteristics.Hp < 0)
        {
            Characteristics.Hp = 0;
#if UNITY_EDITOR == false
            enabled = false;
            GameManager._instance.GameOver();
#endif
        }
    }

    public void UpdateMana(int value)
    {
        
        Characteristics.Mp  += value;
        if (value > 0)
            TextPopUp.CreateWithColor(transform.position, "+" + value, DamageDealer.Text.transform, Color.blue);

        if (Characteristics.Mp > Characteristics.MaxMp)
        {
            Characteristics.Mp = Characteristics.MaxMp;
        }
        else if (Characteristics.Mp < 0)
        {
            Characteristics.Mp = 0;
        }
    }

    public void LoseHp(int damage)
    {
        UpdateHealth(-damage);
        TextPopUp.CreateWithColor(transform.position, "-" + damage, DamageDealer.Text.transform, Color.red);
        //GameManager._instance.playerCharacteristics.Hp = Hp;
        animator.SetTrigger("Hit");
    }

    public bool PlayerCastSpell(int cost)
    { 
        if (Characteristics.Mp - cost >= 0)
        {
            UpdateMana(-cost);
            Mana = Characteristics.Mp;
            animator.SetTrigger("Casting2");
            return true;
        }
        return false;

    }
    protected override void OnCantMove<T>(T comp)
    {
        Enemy enemy  = comp as Enemy;
		//if (DiceManager.TwentyEdges.Roll() > 15)
  //          LoseHp(enemy.Damage);
    }

    public Dictionary<string, int> GetData()
    {

        Dictionary<string, int> temp = new Dictionary<string, int>();

        temp.Add("Hp",      Characteristics.Hp);
        temp.Add("MaxHp",   Characteristics.MaxHp);

        temp.Add("Mp",      Characteristics.Mp);
        temp.Add("MaxMp",   Characteristics.MaxMp);

        temp.Add("Lucky",           Characteristics.Lucky);
        temp.Add("Dexterity",       Characteristics.Dexterity);
        temp.Add("Strength",        Characteristics.Strength);
        temp.Add("Charisma",        Characteristics.Charisma);
        temp.Add("Intelligence",    Characteristics.Intelligence);

        return temp;
    }

    public void LoadData(Dictionary<string, int> data)
    {
        Characteristics.Hp      = data["Hp"];
        Characteristics.MaxHp   = data["MaxHp"];
        Characteristics.Mp      = data["Mp"];
        Characteristics.MaxMp   = data["MaxMp"];

        Characteristics.Lucky           = data["Lucky"];
        Characteristics.Dexterity       = data["Dexterity"];
        Characteristics.Strength        = data["Strength"];
        Characteristics.Intelligence    = data["Intelligence"];
        Characteristics.Charisma        = data["Charisma"];

        GameManager._instance.playerCharacteristics = Characteristics;
        //GameManager._instance.Player = this;
    }
}

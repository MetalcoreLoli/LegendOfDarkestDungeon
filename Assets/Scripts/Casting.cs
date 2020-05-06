using Assets.Scripts.Spells;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Casting : MonoBehaviour
{
    public Transform FirePoint;
    public Transform FirePointUp;
    public GameObject SpellPrefab;

    public GameObject[] SpellPrefabs;

    public float spellForce = 10f;

    private Player player;

    private int spellNumber = 0;

    private void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        player = GetComponent<Player>();
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            spellNumber = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            spellNumber = 1;
        }


        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Cast();
        }
    }

    private void Cast()
    {
        var spell = SpellPrefabs[spellNumber].GetComponent<Spell>();
        spell.FirePoint = FirePoint;
        spell.FirePointUp = FirePointUp;
        
        spell.Cast();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Casting : MonoBehaviour
{
    public Transform FirePoint;
    public Transform FirePointUp;
    public GameObject SpellPrefab;

    public float spellForce = 10f;

    private Player player;
    
    // Update is called once per frame
    void Update()
    {
        player = GetComponent<Player>();
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            GetComponent<Player>().PlayerCastSpell();
            Cast();
        }
    }

    private void Cast()
    {
        GameObject spell = null;
        var firePointPos = FirePoint.position;
        if (player.lookDir == LookDir.Left || player.lookDir == LookDir.Right)
            spell = Instantiate(SpellPrefab, FirePoint.position, FirePoint.rotation);
        else
        { 
            spell = Instantiate(SpellPrefab, FirePointUp.position, FirePointUp.rotation);
            firePointPos = FirePointUp.position;
        }


        Rigidbody2D rb = spell.GetComponent<Rigidbody2D>();

        rb.AddForce(firePointPos * spellForce, ForceMode2D.Impulse);


    }
}

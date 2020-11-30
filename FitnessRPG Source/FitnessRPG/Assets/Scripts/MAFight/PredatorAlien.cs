using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredatorAlien : Enemy
{
    public int UltHitCounter { get; set; }
    public bool ulting;

    new void Start()
    {
        base.Start();
        UltHitCounter = 3;
        ulting = false;
    }

    new void Update()
    {
        base.Update();

        if (!manager.Paused && !manager.End && ulting)
        {
            if (xScale < 1)
            {
                xScale += (Time.deltaTime / manager.enemyCopy.EnemyStats["AttackSpeed"]);
                ultBarCD.localScale = new Vector3(xScale, 1.0f, 1.0f);
                attackDelay -= Time.deltaTime;
                ultCDText.text = "Spezialattacke" + " in:\t" + attackDelay.ToString("F2") + " Sek.";
            }
            else
            {
                done = false;
                ultBarCD.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                ultCDText.text = "Spezialattacke" + " in:\t" + "0 Sek.";
                xScale = 0;
                ultBarCD.localScale = new Vector3(xScale, 1.0f, 1.0f);
                ultCDText.text = "Spezialattacke" + " in:\t" + attackDelay.ToString("F2") + " Sek.";
                attackDelay = manager.enemyCopy.EnemyStats["AttackSpeed"];
                UltimateCatAttack();
            }
        }
    }

    override protected void Attack()
    {
        anim.SetBool("Attack", true);
        UltHitCounter--;                        //so far the hit counter reduces independent of a hit, which is intended
        if (UltHitCounter > 0)
        {
            done = true;
        }
        else
        {
            attackCD.gameObject.SetActive(false);
            attackCDText.gameObject.SetActive(false);
            ultBar.SetActive(true);
            xScale = 0;             //redundant, but for clarity
            ultBarCD.localScale = new Vector3(xScale, 1.0f, 1.0f);
            ultCDText.text = "Spezialattacke";
            ulting = true;
        }
    }
    //called when counter hits zero, some special attack
    void UltimateCatAttack()
    {
        anim.SetBool("Attack", true);
        attackCD.gameObject.SetActive(true);
        attackCDText.gameObject.SetActive(true);
        xScale = 0;             //redundant, but for clarity
        ultBarCD.localScale = new Vector3(xScale, 1.0f, 1.0f);
        ultBar.SetActive(false);
        ulting = false;
        UltHitCounter = 3;
        done = true;
    }
}

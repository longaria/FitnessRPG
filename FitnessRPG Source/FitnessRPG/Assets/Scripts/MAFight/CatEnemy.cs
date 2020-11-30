using System.Collections;
using UnityEngine;

public class CatEnemy : Enemy
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
                StartCoroutine(UltimateCatAttack());
            }
        }
        else
        {
            base.Update();
        }
    }

    override protected void Attack()
    {
        anim.SetBool("Attack", true);
        UltHitCounter--;                        //so far the hit counter reduces independent of a hit, which is intended
        if(UltHitCounter > 0)
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
    IEnumerator UltimateCatAttack()
    {
        GameObject ult = Resources.Load<GameObject>("Prefabs/MAFights/ultimateBomb");
        Vector3 playerPos = manager.player.transform.position;
        for(int i = 0; i < 10; i++)
        {
            float x = Random.Range(-1.5f, 1.5f);
            GameObject inst = Instantiate(ult);
            inst.transform.position= new Vector3(playerPos.x + x, playerPos.y + 10, playerPos.z);
            yield return new WaitForSeconds(0.1f);
        }

        float min = manager.enemyCopy.EnemyStats["DamageM"];
        manager.enemyCopy.EnemyStats["DamageM"] = 1.5f;
        int dmg = CalcDmg();
        manager.player.GetComponent<Player>().UpdateHealth(dmg);
        manager.enemyCopy.EnemyStats["DamageM"] = min;

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

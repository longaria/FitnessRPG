using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public MAFightManager manager;
    public Animator anim;
    public float curHealth, xScale;
    public float attackDelay;
    public bool dodged, crit, done; // true if PLAYER dodged

    protected GameObject damageCombatText, healthbar, ultBar;
    protected RectTransform attackCD, ultBarCD;
    protected Text attackCDText, ultCDText;
    protected Transform CanvasTransform;

    protected void Awake()
    {
        manager = GameObject.FindGameObjectWithTag("myManager").GetComponent<MAFightManager>();
        damageCombatText = manager.fctPrefab;
        healthbar = GameObject.FindGameObjectWithTag("enemyHB");
        attackCD = GameObject.FindGameObjectWithTag("enemyDel").GetComponent<RectTransform>();
        attackCDText = GameObject.FindGameObjectWithTag("enemyDelText").GetComponent<Text>();
        ultBar = manager.enemyUlt;
        ultBar.SetActive(true);
        ultBarCD = GameObject.FindGameObjectWithTag("enemyUltDel").GetComponent<RectTransform>();
        ultCDText = GameObject.FindGameObjectWithTag("enemyUltText").GetComponent<Text>();
        ultBar.SetActive(false);
        anim = GetComponent<Animator>();
    }

    // Use this for initialization
    protected void Start()
    {
        dodged = false;
        crit = false;
        done = true;
        curHealth = manager.enemyCopy.EnemyStats["Health"] * manager.enemyCopy.EnemyStats["HealthM"];
        attackDelay = manager.enemyCopy.EnemyStats["AttackSpeed"];
        CanvasTransform = GameObject.FindGameObjectWithTag("myCanvas").transform;
        xScale = 0;
        attackCD.localScale = new Vector3(xScale, 1.0f, 1.0f);
        attackCDText.text = "Angriff in:\t" + attackDelay.ToString("F2") + " Sek.";
    }

    protected void Update()
    {
        if (!manager.Paused && done && !manager.End)        //Paused: timescale set to 0, done: attack execution, End: fight over
        {
            if (xScale < 1)
            {
                xScale += (Time.deltaTime / manager.enemyCopy.EnemyStats["AttackSpeed"]);
                attackCD.localScale = new Vector3(xScale, 1.0f, 1.0f);
                attackDelay -= Time.deltaTime;
                attackCDText.text = "Angriff in:\t" + attackDelay.ToString("F2") + " Sek.";
            }
            else
            {
                done = false;
                attackCD.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                attackCDText.text = "Angriff in:\t" + "0 Sek.";
                xScale = 0;
                attackCD.localScale = new Vector3(xScale, 1.0f, 1.0f);
                attackCDText.text = "Angriff in:\t" + attackDelay.ToString("F2") + " Sek.";
                attackDelay = manager.enemyCopy.EnemyStats["AttackSpeed"];
                Attack();
            }
        }
    }
    //returns enemys largest mitigated damage
    public int CalcDmg()
    {
        Dictionary<string, float> p = manager.playerStats;
        Dictionary<string, float> e = manager.enemyCopy.EnemyStats;

        float rawPhysicalDamage = Mathf.RoundToInt(e["DamageM"] * Random.Range(e["MinDmg"], e["MaxDmg"] + 1));
        float rawMagicDamage = Mathf.RoundToInt(e["DamageM"] * Random.Range(e["MinMDmg"], e["MaxMDmg"] + 1));
        float mitigatedPhysicalDamage = Mathf.Max(0, rawPhysicalDamage - (p["PhysicalDefenseM"] * p["PhysicalDefense"]));
        float mitigatedMagicDamage = Mathf.Max(0, rawMagicDamage - (p["MagicDefenseM"] * p["MagicDefense"]));

        bool dodgeRoll = Random.Range(1, 101) <= p["DodgeChance"] ? true : false;
        bool critRoll = Random.Range(1, 101) <= e["CritChance"] ? true : false;

        if (dodgeRoll)
        {
            manager.player.GetComponent<Player>().DisplayFCTMsg("Ausgewichen!");
            return 0;
        }
        else
        {
            if (critRoll)
            {
                int result = Mathf.RoundToInt(1.5f * Mathf.Max(mitigatedMagicDamage, mitigatedPhysicalDamage));
                if (result == 0)
                {
                    manager.player.GetComponent<Player>().DisplayFCTMsg("Geblockt!");
                    return 0;
                }
                else
                {
                    manager.player.GetComponent<Player>().crit = true;
                    return result;
                }
            }
            else
            {
                int result = Mathf.RoundToInt(Mathf.Max(mitigatedMagicDamage, mitigatedPhysicalDamage));
                if (result == 0)
                {
                    manager.player.GetComponent<Player>().DisplayFCTMsg("Geblockt!");
                    return 0;
                }
                else return result;
            }
        }
    }

    virtual protected void Attack()
    {
        Debug.Log("Enemy Attack() should never be called");
    }

    //updates health and health text
    public void UpdateHealth(int damage)
    {
        float maxHealth = manager.enemyCopy.EnemyStats["Health"] * manager.enemyCopy.EnemyStats["HealthM"];
        float oldScale = curHealth / maxHealth;
        curHealth = (damage >= 0) ? Mathf.Max(curHealth - damage, 0) : Mathf.Min(curHealth - damage, maxHealth);
        float newScale = curHealth / maxHealth;
        DisplayFCTDamage(damage);

        if (GameObject.FindGameObjectWithTag("ultimateProjectile") == null)
        {
            manager.UpdateUltimateBar();
        }

        if (curHealth == 0)
        {
            StartCoroutine(manager.UpdateBar(oldScale, 0, healthbar));
        }
        else
        {
            StartCoroutine(manager.UpdateBar(oldScale, newScale, healthbar));
            if(damage >= 0)
            {
                //anim.SetBool("Hit", true);
            }
            else
            {
                //play drink animation or smth
            }
        }
    }
    //display floating combat text above enemys head
    void DisplayFCTDamage(int damage)
    {
        GameObject a = Instantiate(damageCombatText, CanvasTransform);
        a.GetComponent<RectTransform>().localRotation = Quaternion.identity;
        a.GetComponent<RectTransform>().localScale = Vector3.one;
        a.transform.position = new Vector3(transform.position.x, transform.position.y + 10, transform.position.z);

        if (crit)
        {
            crit = false;
            a.GetComponent<RectTransform>().localScale = new Vector3(1.5f, 1.5f, 1);
            a.GetComponent<Text>().text = damage.ToString() + " !!!";
            a.GetComponent<Text>().color = Color.red;
        }
        else if (damage < 0)
        {
            damage *= -1;
            a.GetComponent<Text>().color = Color.green;
            a.GetComponent<Text>().text = damage.ToString();
        }
        else
        {
            a.GetComponent<Text>().text = damage.ToString();
            a.GetComponent<Text>().color = Color.red;
        }
        a.GetComponent<Animator>().Play("FCT");
        Destroy(a, 2);
    }

    public void DisplayFCTMsg(string message)
    {
        //supposed to display extra stats/conditions etc.
        GameObject a = Instantiate(damageCombatText, CanvasTransform);
        a.GetComponent<RectTransform>().localRotation = Quaternion.identity;
        a.GetComponent<RectTransform>().localScale = Vector3.one;
        a.GetComponent<RectTransform>().localPosition = new Vector3(475, 2, 0);

        a.GetComponent<Text>().text = message;
        a.GetComponent<Text>().color = Color.yellow;

        if (message == "Ausgewichen!" || message == "Geblockt!") a.GetComponent<Text>().color = Color.cyan;

        a.GetComponent<Animator>().Play("FCT");
        Destroy(a, 2);
    }
}

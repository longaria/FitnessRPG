using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private GameObject damageCombatText, healthbar;
    public MAFightManager manager;
    public Animator anim;
    private bool jumping;
    private Button button;
    private RectTransform attackCD, recoveryRect;
    private Text attackCDText, recoveryText;
    public float curHealth, xScale, recScale;
    public float attackDelay, endurance, jumpCost, tempDodge;
    private int hitCounter, crosshairTrigger;
    public bool dodged, crit; //true if ENEMY dodged, see how updatedHealth/calcDmg interact..

    private Transform CanvasTransform; 

    void Awake()
    {
        manager = GameObject.FindGameObjectWithTag("myManager").GetComponent<MAFightManager>();
        damageCombatText = manager.fctPrefab;
        healthbar = GameObject.FindGameObjectWithTag("playerHB");
        anim = GetComponent<Animator>();
        CanvasTransform = GameObject.FindGameObjectWithTag("myCanvas").transform;
        attackCD = GameObject.FindGameObjectWithTag("playerDel").GetComponent<RectTransform>();
        attackCDText = GameObject.FindGameObjectWithTag("playerDelText").GetComponent<Text>();
        recoveryRect = GameObject.FindGameObjectWithTag("playerRec").GetComponent<RectTransform>();
        recoveryText = GameObject.FindGameObjectWithTag("playerRecText").GetComponent<Text>();
    }

    // Use this for initialization
    void Start()
    {
        hitCounter = 0;
        //number of hits required for CrossHair: 20 dex == 1 hit reduction
        crosshairTrigger = Mathf.RoundToInt(10 - (manager.playerStats["Dexterity"] *0.05f));
        jumping = false;
        dodged = false;
        crit = false;
        xScale = 0;
        recScale = 1;
        jumpCost = 33;
        curHealth = manager.playerStats["Health"] * manager.playerStats["HealthM"];
        attackDelay = manager.playerStats["AttackSpeed"];
        endurance = manager.playerStats["Endurance"];
        attackCD.localScale = new Vector3(xScale, 1.0f, 1.0f);
        attackCDText.text = "Angriff in:\t" + attackDelay.ToString("F2") + " Sek.";
    }

    void Update()
    {
        if (!manager.Paused && !manager.End)
        {
            if (xScale < 1)
            {
                xScale += (Time.deltaTime / manager.playerStats["AttackSpeed"]);
                attackCD.localScale = new Vector3(xScale, 1.0f, 1.0f);
                attackDelay -= Time.deltaTime;
                attackCDText.text = "Angriff in:\t" + attackDelay.ToString("F2") + " Sek.";
            }
            else
            {
                attackCD.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                attackCDText.text = "Angriff in:\t" + "0 Sek.";
                anim.SetBool("Attack", true);
                Vector3 enemy = manager.enemy.transform.position;
                int dmg = CalcDmg();
                if (dmg > 0)
                {
                    IncreaseHitCounter();
                    manager.enemy.GetComponent<Enemy>().UpdateHealth(dmg);
                    manager.SpawnParticle(new Vector3(enemy.x, enemy.y + 3, enemy.z), "Blood");
                }
                else
                {
                    manager.SpawnParticle(new Vector3(enemy.x, enemy.y + 3, enemy.z), "Poof");
                }
                xScale = 0;
                attackCD.localScale = new Vector3(xScale, 1.0f, 1.0f);
                attackCDText.text = "Angriff in:\t" + attackDelay.ToString("F2") + " Sek.";
                attackDelay = manager.playerStats["AttackSpeed"];
            }

            if(recScale < 1)
            {
                recScale += Time.deltaTime * manager.playerStats["Recovery"] / manager.playerStats["Endurance"];
                recoveryRect.localScale = new Vector3(recScale, 1.0f, 1.0f);
                endurance += Time.deltaTime * manager.playerStats["Recovery"];
                recoveryText.text = "Ausdauer: " + ((int)endurance).ToString();
            }
            else
            {
                recScale = 1;
                recoveryRect.localScale = new Vector3(recScale, 1.0f, 1.0f);
                endurance = manager.playerStats["Endurance"];
                recoveryText.text = "Ausdauer: " + ((int)endurance).ToString();
            }
        }
    }

    // returns players largest mitigated damage
    public int CalcDmg()
    {
        Dictionary<string, float> p = manager.playerStats;
        Dictionary<string, float> e = manager.enemyCopy.EnemyStats;

        float rawPhysicalDamage = Mathf.RoundToInt(p["DamageM"] * Random.Range(p["MinDmg"], p["MaxDmg"] + 1));
        float rawMagicDamage = Mathf.RoundToInt(p["DamageM"] * Random.Range(p["MinMDmg"], p["MaxMDmg"] + 1));
        float mitigatedPhysicalDamage = Mathf.Max(0, rawPhysicalDamage - (e["PhysicalDefenseM"] * e["PhysicalDefense"]));
        float mitigatedMagicDamage = Mathf.Max(0, rawMagicDamage - (e["MagicDefenseM"] * e["MagicDefense"]));

        bool dodgeRoll = Random.Range(1, 101) <= e["DodgeChance"] ? true : false;
        bool critRoll = Random.Range(1, 101) <= p["CritChance"] ? true : false;

        if (dodgeRoll)
        {
            manager.enemy.GetComponent<Enemy>().DisplayFCTMsg("Ausgewichen!");
            return 0;
        }
        else
        {
            if (critRoll)
            {
                int result = Mathf.RoundToInt(1.5f * Mathf.Max(mitigatedMagicDamage, mitigatedPhysicalDamage));
                if (result == 0)
                {
                    manager.enemy.GetComponent<Enemy>().DisplayFCTMsg("Geblockt!");
                    return 0;
                }
                else
                {
                    manager.enemy.GetComponent<Enemy>().crit = true;
                    return result;
                }
            }
            else
            {
                int result = Mathf.RoundToInt(Mathf.Max(mitigatedMagicDamage, mitigatedPhysicalDamage));

                if (result == 0)
                {
                    manager.enemy.GetComponent<Enemy>().DisplayFCTMsg("Geblockt!");
                    return 0;
                }
                else return result;
            }
        }
    }

    //updates health and health text, called usually by the oppenent
    public void UpdateHealth(int damage)
    {
        float maxHealth = manager.playerStats["Health"] * manager.playerStats["HealthM"];
        float oldScale = curHealth / maxHealth;
        curHealth = (damage >= 0) ? Mathf.Max(curHealth - damage, 0) : Mathf.Min(curHealth - damage, maxHealth);
        float newScale = curHealth / maxHealth;

        DisplayFCTDamage(damage);

        if (curHealth == 0)
        {
            StartCoroutine(manager.UpdateBar(oldScale, 0, healthbar));
        }
        else
        {
            StartCoroutine(manager.UpdateBar(oldScale, newScale, healthbar));
            if (damage >= 0)
            {
                anim.SetBool("Hit", true);
            }
            else
            {
                //maybe drink animation?
            }
        }
    }

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
        a.GetComponent<RectTransform>().localPosition = new Vector3(-430, 2, 0);

        a.GetComponent<Text>().text = message;
        a.GetComponent<Text>().color = Color.yellow;

        if (message == "Ausgewichen!" || message == "Geblockt!") a.GetComponent<Text>().color = Color.cyan;

        a.GetComponent<Animator>().Play("FCT");
        Destroy(a, 2);
    }

    public void IncreaseHitCounter()
    {
        hitCounter++;
        if (hitCounter == crosshairTrigger && !manager.End)
        {
            hitCounter = 0;
            manager.ActivateCrosshair();
        }
    }

    public void Jump()
    {
        if(jumpCost < endurance)
        {
            jumping = true;
            recScale = Mathf.Max(0, recScale - jumpCost / endurance);
            endurance -= jumpCost;
            tempDodge = manager.playerStats["DodgeChance"];
            manager.playerStats["DodgeChance"] = 100;
            GetComponent<Rigidbody2D>().AddForce(Vector2.up * 50);
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (jumping && (collision.collider.gameObject.tag == "block"))
        {
            manager.playerStats["DodgeChance"] = tempDodge;
            jumping = false;
        }
    }

}

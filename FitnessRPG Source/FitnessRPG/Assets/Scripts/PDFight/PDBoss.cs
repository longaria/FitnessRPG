using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PDBoss : MonoBehaviour
{
    private GameObject damageCombatText;
    private PDManager manager;
    private Animator anim;
    private int health, curhealth;

    public Vector3 MyPosition { get; set; } //in world space
    public float AttackSpd { get; set; }
    public float UpdateHP { get; set; }
    public int BossDmgPool { get; set; }

    void Awake()
    {
        damageCombatText = Resources.Load<GameObject>("PDFCTParent");
        anim = GetComponent<Animator>();
        manager = GameObject.FindGameObjectWithTag("myManager").GetComponent<PDManager>();
    }

    // Use this for initialization
    void Start()
    {
        BossDmgPool = 0;
        UpdateHP = 1;
        AttackSpd = manager.bossData.EnemyStats["AttackSpeed"];
    }

    void Update()
    {
        if (!manager.PlayersDead && !manager.BossDead)
        {
            if (AttackSpd > 0)
            {
                AttackSpd -= Time.deltaTime;
            }
            else
            {
                anim.SetBool("Attack", true);

                foreach(KeyValuePair<string, GameObject> player in manager.players)
                {
                    int dmg = PDcalcDmg(player.Value.GetComponent<PDPlayer>().Profile);
                    player.Value.GetComponent<PDPlayer>().UpdateHealth(dmg);
                }
                if(manager.players.Count != manager.playersCopy.Count)
                {
                    manager.players = new Dictionary<string, GameObject>(manager.playersCopy);
                }
                AttackSpd = manager.bossData.EnemyStats["AttackSpeed"];
            }
            if(UpdateHP > 0)
            {
                UpdateHP -= Time.deltaTime;
            }
            else
            {
                if (BossDmgPool > 0)
                {
                    UpdateHealth(BossDmgPool);
                    BossDmgPool = 0;
                }
                UpdateHP = 1;
            }
        }
    }

    // returns bosses largest mitigated damage against given player
    public int PDcalcDmg(Profile.PlayerData player)
    {
        Dictionary<string, float> e = player.stats;

        float playerMitigation = (manager.bossData.Name == "Auge des Horrors") ? e["PhysicalDefense"] * e["PhysicalDefenseM"] :
            e["MagicDefense"] * e["MagicDefenseM"];

        float bossDmg = Random.Range(manager.avgBossHit / 2, manager.avgBossHit * 2) / manager.players.Count;
        float damage = bossDmg - playerMitigation;
        bool dodgeRoll = Random.Range(1, 101) <= e["DodgeChance"] ? true : false;
        bool critRoll = Random.Range(1, 101) <= 5 ? true : false;

        if (dodgeRoll)
        {
            return 0;
        }
        else
        {
            if (critRoll)
            {
                return Mathf.RoundToInt(1.5f * damage);
            }
            else
            {
                return Mathf.RoundToInt(damage);
            }
        }
    }

    //updates health, given net damage
    public void UpdateHealth(int damage)
    {
        if (damage > 0)
        {
            curhealth = Mathf.Max(curhealth - damage, 0);
            if (curhealth == 0)
            {
                anim.SetBool("Dead", true);
                manager.BossDead = true;
                manager.StartCoroutine(manager.PlayFireWorks());
            }
            float scale = (float) curhealth / health;
            manager.BossBar.GetComponent<RectTransform>().localScale = new Vector3(scale, 1, 1);
            manager.bossPercText.GetComponent<Text>().text = ((int)(scale * 100)).ToString() + " %";
        }
        displayFCTDamage(damage);
    }

    void displayFCTDamage(int damage)
    {
        Transform parent = GameObject.FindGameObjectWithTag("myCanvas").transform;
        GameObject a = Instantiate(damageCombatText, MyPosition, Quaternion.identity, parent);
        a.GetComponent<RectTransform>().localRotation = Quaternion.identity;
        a.GetComponent<RectTransform>().localScale = Vector3.one;

        a.name = name + " FCT";

        if (damage < 0)
        {
            damage *= -1;
            a.GetComponentInChildren<Text>().color = Color.green;
        }
        else
        {
            a.GetComponentInChildren<Text>().color = Color.red;
        }
        a.GetComponentInChildren<Text>().text = damage.ToString();
        a.GetComponentInChildren<Animator>().Play("PDFCT");
        Destroy(a, 2);
    }

    public void setHealth(int h)
    {
        health = h;
        curhealth = h;
    }
}

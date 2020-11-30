using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PDPlayer : MonoBehaviour
{
    private GameObject damageCombatText;
    private PDManager manager;
    private Animator anim;

    private bool death;
    private int curhealth;

    public Vector3 MyPosition { get; set; } //in world space
    public float Dmg { get; set; }
    public float AttackSpd { get; set; }
    public Profile.PlayerData Profile { get; set; }
    public string Name { get; set; }

    private void Awake()
    {
        damageCombatText = Resources.Load<GameObject>("PDFCTParent");
        anim = GetComponent<Animator>();
        manager = GameObject.FindGameObjectWithTag("myManager").GetComponent<PDManager>();
    }

    // Use this for initialization
    void Start()
    {
        death = false;
        AttackSpd = Profile.stats["AttackSpeed"];
    }

    void Update()
    {
        if (!death && !manager.PlayersDead && !manager.BossDead)
        {
            if (AttackSpd > 0)
            {
                AttackSpd -= Time.deltaTime;
            }
            else
            {
                anim.SetBool("Attack", true);
                int dmg = PDcalcDmg();
                manager.playersDmgDict[Name] += dmg;
                manager.boss.GetComponent<PDBoss>().BossDmgPool += dmg;
                AttackSpd = Profile.stats["AttackSpeed"];
            }
        }
    }

    // returns players largest mitigated damage against boss
    public int PDcalcDmg()
    {
        Dictionary<string, float> p = Profile.stats;
        Dictionary<string, float> e = manager.bossData.EnemyStats;

        float rawPhysicalDamage = Mathf.RoundToInt(p["DamageM"] * Random.Range(p["MinDmg"], p["MaxDmg"] + 1));
        float rawMagicDamage = Mathf.RoundToInt(p["DamageM"] * Random.Range(p["MinMDmg"], p["MaxMDmg"] + 1));
        float mitigatedPhysicalDamage = Mathf.Max(0, rawPhysicalDamage - (e["PhysicalDefenseM"] * e["PhysicalDefense"]));
        float mitigatedMagicDamage = Mathf.Max(0, rawMagicDamage - (e["MagicDefenseM"] * e["MagicDefense"]));

        bool dodgeRoll = Random.Range(1, 101) <= e["DodgeChance"] ? true : false;
        bool critRoll = Random.Range(1, 101) <= p["CritChance"] ? true : false;

        if (dodgeRoll)
        {
            return 0;
        }
        else
        {
            if (critRoll)
            {
                return Mathf.RoundToInt(1.5f * Mathf.Max(mitigatedMagicDamage, mitigatedPhysicalDamage));
            }
            else
            {
                return Mathf.RoundToInt(Mathf.Max(mitigatedMagicDamage, mitigatedPhysicalDamage));
            }
        }
    }

    //updates health, given net damage
    public void UpdateHealth(int damage)
    {
        if (damage >= 0)
        {
            curhealth = Mathf.Max(curhealth - damage, 0);
            manager.curPlayerHealth = Mathf.Max(manager.curPlayerHealth - damage, 0);
            if (curhealth == 0)
            {
                death = true;
                anim.SetBool("Dead", true);
                manager.SpawnParticle(transform.position, "DeathSkull");
                Destroy(gameObject, 1f);
                manager.playersCopy.Remove(Profile.AvatarName);
                if (manager.playersCopy.Count == 0) 
                {
                        manager.PlayersDead = true;
                        manager.EvaluateEndOfFight();
                }
                GameObject o = GameObject.Find(gameObject.name + " Text");
                if (o) o.SetActive(false);
            }
            float scale = manager.curPlayerHealth / manager.totalPlayerHealth;
            manager.playerBar.GetComponent<RectTransform>().localScale = new Vector3(scale, 1, 1);
            manager.playerPercText.GetComponent<Text>().text = ((int)(scale * 100)).ToString() + " %";
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
        curhealth = h;
    }
}

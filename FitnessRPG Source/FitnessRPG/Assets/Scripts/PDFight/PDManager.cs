using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PDManager : MonoBehaviour {

    public GameObject playerBar, boss, BossBar, playerPercText, fightScreen, bossPercText, WinScreen, LossScreen, DamageScreen, rankCell;
    public Text playerName;
    public Canvas myCanvas;
    public Dictionary<string, GameObject> players, playersCopy;
    public Dictionary<string, float> playersDmgDict { get; set; }
    private List<KeyValuePair<string, float>> playersDmg;
    public Profile.GameData.Enemy bossData;
    public float curPlayerHealth, totalPlayerHealth, avgBossHit;
    public bool PlayersDead { get; set; }
    public bool BossDead { get; set; }
    private const int AVATARSLOTS = 12;         //how many avatars can be spawned on the field at once
    private const int BOSSHITS = 6;
    private const int PLAYERHITS = 7;          //how many hits the boss should need to kill the group - used to scale boss average hit
    private enum Attacker { Players, Boss};
    public PDProfile profile;
    public Profile p;

    class Damage : IComparer<KeyValuePair<string, float>>
    {
        public int Compare(KeyValuePair<string, float> x, KeyValuePair<string, float> y)
        {
            if (x.Key == "" || y.Key == "")
            {
                return 0;
            }

            // CompareTo() method 
            return x.Value.CompareTo(y.Value);
        }
    }

    //calculate who won the fight and display end message, loot etc accordingly
    public void EvaluateEndOfFight()
    {
        //contains both are dead - in that case, be generous and give out boss win loot
        if (BossDead)
        {
            //normally calculate the loot list first, then put into WinScreen
            foreach (KeyValuePair<string, Profile.PlayerData> participant in profile.participationlist)
            {
                participant.Value.CurExperiencePoints += bossData.XP;
                //check if level up occured and grant stat points accordingly
                int nextLvlXP = 0;
                for (int i = 1; i <= participant.Value.Level; i++)
                {
                    nextLvlXP += i * 75;
                }
                //account that multiple level ups could occur
                while (participant.Value.CurExperiencePoints > 0 &&
                    (participant.Value.CurExperiencePoints) >= nextLvlXP)
                {
                    participant.Value.Level++;
                    nextLvlXP += participant.Value.Level * 75;
                    participant.Value.FreeStatPointsLeft += 5;

                }
                participant.Value.Gold += bossData.MaxGold;
                for(int i = 0; i <bossData.loot.Length; i++)
                {
                    Profile.GameData.Item item = p.gameData.allItems.Find(x => x.ID.Equals(bossData.loot[i].itemID));
                    if(item != null) participant.Value.AddToPlayerInventory(item);
                }
                participant.Value.ActivityPointsGained += 50;
                //Save(participant.Value);
            }
            WinScreen.SetActive(true);
            Text[] texts = WinScreen.GetComponentsInChildren<Text>();
            Text loot = texts[0].gameObject.name == "Loot" ? texts[0] : texts[1];
            loot.text = "Jeder gewinnt:\n\n" + bossData.XP + " Erfahrung\n" + bossData.MaxGold + " Gold\n" + "50 Aktivitätspunkte\n";

            for (int i = 0; i < bossData.loot.Length; i++)
            {
                Profile.GameData.Item item = p.gameData.allItems.Find(x => x.ID.Equals(bossData.loot[i].itemID));
                loot.text += item.Name + "\n";
            }
        }
        else
        {
            //normally calculate the loot list first, then put into LossScreen
            foreach (KeyValuePair<string, Profile.PlayerData> participant in profile.participationlist)
            {
                participant.Value.CurExperiencePoints += bossData.XP / 2;
                participant.Value.Gold += bossData.MinGold;
                participant.Value.ActivityPointsGained += 50;
                //Save(participant.Value);
            }
            LossScreen.SetActive(true);
            Text[] texts = LossScreen.GetComponentsInChildren<Text>();
            Text loot = texts[0].gameObject.name == "Loot" ? texts[0] : texts[1];
            loot.text = "Jeder gewinnt:\n\n" + (bossData.XP / 2) + " Erfahrung\n" + bossData.MinGold + " Gold\n" + "50 Aktivitätspunkte\n";

            for (int i = 0; i < bossData.loot.Length; i++)
            {
                Profile.GameData.Item item = p.gameData.allItems.Find(x => x.ID.Equals(bossData.loot[i].itemID));
                if(item != null) loot.text += item.Name + "\n";
            }
        }

        foreach(KeyValuePair<string,float> k in playersDmgDict)
        {
            playersDmg.Add(k);
        }

        Damage d = new Damage();
        playersDmg.Sort(d);
        playersDmg.Reverse();
        Invoke("LoadDamage", 3f);
    }

    public void SpawnParticle(Vector3 pos, string particleName)
    {
        GameObject pref = Resources.Load<GameObject>("Prefabs/FX/" + particleName);
        GameObject particles = Instantiate(pref);
        particles.transform.position = pos;
        if (particleName == "DarkMagicAura") particles.transform.localScale = new Vector3(3.5f, 3.5f, 1);
        ParticleSystem ps = particles.GetComponent<ParticleSystem>();

        if (ps != null)
        {
            ParticleSystem[] parts = particles.GetComponentsInChildren<ParticleSystem>();
            foreach (ParticleSystem p in parts)
            {
                ps.GetComponent<Renderer>().sortingLayerName = "myLayer";
            }
            particles.SetActive(true);
            var main = ps.main;
            if (main.loop)
            {
                ps.gameObject.AddComponent<CFX_AutoStopLoopedEffect>();
                ps.gameObject.AddComponent<CFX_AutoDestructShuriken>();
            }
        }
    }

    public IEnumerator PlayFireWorks()
    {
        for (int i = 0; i < 20; i++)
        {
            int k = Random.Range(1, 3);
            string str = k == 1 ? "Fireworks" : "Fireworks2";
            float r = Random.Range(-4.5f, 4.5f);                //world space view range x [-10,16], y[-6,9]
            float s = Random.Range(-2.5f, 2.5f);
            SpawnParticle(new Vector3(r, s, 1), str);
            yield return new WaitForSeconds(.2f);
        }
        EvaluateEndOfFight();
    }

    void LoadDamage()
    {
        fightScreen.SetActive(false);
        WinScreen.SetActive(false);
        LossScreen.SetActive(false);
        DamageScreen.SetActive(true);
        Transform parent = GameObject.Find("List").GetComponent<Transform>();
        int i = 1;
        float maxScore = -1;

        foreach(KeyValuePair<string,float> k in playersDmg)
        {
            GameObject o = Instantiate(rankCell, parent);
            Text[] ts = o.GetComponentsInChildren<Text>();
            Text score = null;
            RectTransform bar = null;
            if (maxScore == -1) maxScore = k.Value;             //assuming the first time this is set to highest score

            foreach (Text t in ts)
            {
                if (t.name == "Rank") t.text = i.ToString();
                else if (t.name == "Playername") t.text = k.Key;
                else score = t;
            }

            Image[] imgs = o.GetComponentsInChildren<Image>();

            foreach(Image im in imgs)
            {
                if (im.name == "AvatarImage") im.sprite = Resources.Load<GameObject>("Avatars/" + profile.participationlist[k.Key].AvatarID).GetComponent<SpriteRenderer>().sprite;
                if (im.name == "Bar")
                {
                    bar = im.rectTransform;
                    byte r = (byte)Random.Range(0, 256);
                    byte g = (byte)Random.Range(0, 256);
                    byte b = (byte)Random.Range(0, 256);
                    im.color = new Color32(r,g,b, 255);
                }
            }

            float scale = k.Value / maxScore;
            StartCoroutine(LoadCell(score, bar, 5, scale, k.Value));
            i++;
        }
        Invoke("LoadRankings", 10f);        //bars fill in 5 seconds, so give 5 additional seconds to view, then switch scenes
    }

    IEnumerator LoadCell(Text score, RectTransform bar, float duration, float scale, float scoreNumber)
    {
        if(score == null || bar == null) yield break;
        float start = 0;
        float startNum = 0;

        while (start < scale)
        {
            start += Time.deltaTime / duration;
            startNum += Time.deltaTime * scoreNumber / duration;
            score.text = ((int)startNum).ToString();
            bar.localScale = new Vector3(start, 1, 1);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        bar.localScale = new Vector3(scale, 1, 1);
        score.text = scoreNumber.ToString();
    }

    //saves playerInfo to data base
    public IEnumerator Save(Profile.PlayerData playerData)
    {
        WWWForm form = new WWWForm();
        //playerdetails
        form.AddField("username", playerData.AvatarName);
        form.AddField("Avatarname", playerData.AvatarName);
        form.AddField("AvatarID", playerData.AvatarID);
        form.AddField("FirstLogin", playerData.FirstLogin.ToString());
        form.AddField("Badge1", playerData.Badge1.ToString());
        form.AddField("Badge2", playerData.Badge2.ToString());
        form.AddField("Badge3", playerData.Badge3.ToString());
        form.AddField("ActivityStreak", playerData.ActivityStreak.ToString());
        form.AddField("ActivityPoints", playerData.ActivityPoints.ToString());
        form.AddField("ActivityPointsGained", playerData.ActivityPointsGained.ToString());
        form.AddField("Level", playerData.Level.ToString());
        form.AddField("XP", playerData.CurExperiencePoints.ToString());
        form.AddField("FreeStatPoints", playerData.FreeStatPointsLeft.ToString());
        form.AddField("Gold", playerData.Gold.ToString());
        form.AddField("BestStageBeaten", playerData.bestStageBeaten.ToString());
        string s = "";
        for (int i = 0; i < 14; i++)
        {
            s += playerData.BeatenStages[i] + ",";
        }
        form.AddField("Stages", s);
        s = "";
        for (int i = 0; i < 20; i++)
        {
            if (playerData.inventory[i] == null)
            {
                s += "NULL" + ",";
            }
            else
            {
                s += playerData.inventory[i].Name + ",";
            }
        }
        form.AddField("Inventory", s);
        s = "";
        for (int i = 0; i < 4; i++)
        {
            if (playerData.equipment[i] == null)
            {
                s += "NULL" + ",";
            }
            else
            {
                s += playerData.equipment[i].Name + ",";
            }

        }
        form.AddField("Equipment", s);

        //playerstats values
        form.AddField("Stats", p.Dict2String(playerData.stats));

        UnityWebRequest www = UnityWebRequest.Post(p.save, form);
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError || www.downloadHandler.text != "0")
        {
            Debug.Log(www.error);
            Debug.Log("Could not save! " + www.downloadHandler.text);
        }
        else
        {
            Debug.Log("PlayerPD save complete!");
        }
    }

    public void LoadRankings()
    {
        SceneManager.LoadScene("PDStats");
    }
    //Initializes Boss stats
    private void InitBossStats()
    {
        float avgPlayerDmg = 0;
        float avgPlayerDefense = 0;
        foreach(KeyValuePair<string, GameObject> player in players)
        {
            Profile.PlayerData m_stats = player.Value.GetComponent<PDPlayer>().Profile;

            float Def = Mathf.Max(m_stats.stats["PhysicalDefense"] * m_stats.stats["PhysicalDefenseM"],
                m_stats.stats["MagicDefense"] * m_stats.stats["MagicDefenseM"]);
            float physDmg = (bossData.EnemyStats["PhysicalDefense"] * bossData.EnemyStats["PhysicalDefenseM"]) - 
                ((m_stats.stats["MinDmg"] + m_stats.stats["MaxDmg"]) / 2) * m_stats.stats["DamageM"] * 
                ((1 + m_stats.stats["CritChance"]) / 100) * m_stats.ActivityStreak;
            float magicDmg = (bossData.EnemyStats["MagicDefense"] * bossData.EnemyStats["MagicDefenseM"]) - 
                ((m_stats.stats["MinMDmg"] + m_stats.stats["MaxMDmg"]) / 2) * m_stats.stats["DamageM"] *
                ((1 + m_stats.stats["CritChance"]) / 100) * m_stats.ActivityStreak;
            float dmg = Mathf.Max(physDmg, magicDmg) / m_stats.stats["AttackSpeed"];

            avgPlayerDmg += dmg;
            avgPlayerDefense += Def;
        }
        avgBossHit = (totalPlayerHealth / PLAYERHITS) + (avgPlayerDefense / players.Count);
        boss.GetComponent<PDBoss>().setHealth((int) (avgPlayerDmg * BOSSHITS));
    }

    void SpawnPlayers()
    {
        //get playerlist, then depending on the number of players (in this course) spawn them in pos 1-12
        //list is filled alphabetically, same order as folder
        int i = 1;
        foreach(KeyValuePair<string, Profile.PlayerData> k in profile.participationlist)
        {
            if (i > AVATARSLOTS) break;
            GameObject original = Resources.Load<GameObject>("Avatars/" + k.Value.AvatarID);
            Transform tilePosition =  GameObject.Find("Pos" + i).GetComponent<Transform>();

            GameObject instance = Instantiate(original, tilePosition.position, Quaternion.identity);
            instance.AddComponent<PDPlayer>();
            instance.GetComponent<PDPlayer>().Profile = k.Value;
            instance.name = k.Value.AvatarName;
            //set player name for playerdmg sorting later
            instance.GetComponent<PDPlayer>().Name = k.Value.AvatarName;
            //init health of players and team health
            float h = k.Value.stats["Health"] * k.Value.stats["HealthM"] * k.Value.ActivityStreak;
            totalPlayerHealth += (int) h;
            instance.GetComponent<PDPlayer>().setHealth((int) h);
            //if the avatar has a rigidbody2d set it to static (because removing/destroying component didn't quite work
            if (instance.GetComponent<Rigidbody2D>())
            {
                instance.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            }
            //find position where to place, adjust slightly in y-scale, 
            int OrderInLayer = (i % 3) == 1 ? 3 : (i % 3) == 2 ? 0 : 6;
            //instance.GetComponent<SpriteRenderer>().sortingLayerName = "Players";
            instance.GetComponent<SpriteRenderer>().sortingOrder = OrderInLayer;
            //scale the prefab and adjust by activity streak
            instance.GetComponent<Transform>().localScale = Vector3.Scale(original.GetComponent<Transform>().localScale, new Vector3((0.33f), (0.33f), 1f));
            instance.GetComponent<Transform>().localScale = Vector3.Scale(instance.GetComponent<Transform>().localScale, 
                new Vector3(k.Value.ActivityStreak, k.Value.ActivityStreak, 1f));
            instance.GetComponent<Transform>().position = tilePosition.position;

            //FCT position: above avatar head -> Avatar.Pos (center pivot) + 1 yields head position
            instance.GetComponent<PDPlayer>().MyPosition = new Vector3(instance.GetComponent<Transform>().position.x,
            instance.GetComponent<Transform>().position.y + 1.5f, instance.GetComponent<Transform>().position.z);

            //display usernames under each player - not sure, if it's the best position
            Text t = Instantiate(playerName, tilePosition.position, Quaternion.identity, fightScreen.transform);
            t.name = k.Value.AvatarName + " Text";
            t.text = k.Value.AvatarName;
            //spawn a "spawn" particle underneath each player
            SpawnParticle(tilePosition.position, "Spawn");
            //add player gameobject to players
            players.Add(k.Value.AvatarName, instance);
            playersDmgDict.Add(k.Value.AvatarName, 0);
            i++;
        }
        playersCopy = new Dictionary<string, GameObject>(players);
        curPlayerHealth = totalPlayerHealth;
    }
    //spawns one of two FlyingEyes
    void SpawnBoss()
    {
        int i = Random.Range(1, 3);
        //GameObject original = Resources.Load<GameObject>("Boss/FlyingEye" + i.ToString() + "PD");
        GameObject original = Resources.Load<GameObject>("Boss/FlyingEye1PD");
        Transform tilePosition = GameObject.Find("BossPosition").GetComponent<Transform>();

        boss = Instantiate(original, tilePosition.position, Quaternion.identity);
        boss.name = original.name + " Instance";
        //scaling stuff
        boss.GetComponent<Transform>().position = new Vector3(tilePosition.position.x, tilePosition.position.y - 2.5f, tilePosition.position.z);
        boss.GetComponent<Transform>().localScale = new Vector3(2.5f, 2.5f, 1);
        //FCT pos: assuming 6U, +3 (pivot center)
        boss.GetComponent<PDBoss>().MyPosition = new Vector3(boss.GetComponent<Transform>().position.x,
        boss.GetComponent<Transform>().position.y + 3, boss.GetComponent<Transform>().position.z);

        //display boss name as text
        Text t = Instantiate(playerName, tilePosition.position, Quaternion.identity, fightScreen.transform);
        t.name = original.name + " Text";

        bossData = i == 1 ? p.gameData.allEnemies.Find(x => x.ID.Equals("Boss/FlyingEye1")) :
        p.gameData.allEnemies.Find(x => x.ID.Equals("Boss/FlyingEye2"));

        t.text = bossData.Name;
        GameObject.Find("BossTeamText").GetComponent<Text>().text = bossData.Name;
        boss.GetComponent<SpriteRenderer>().sortingLayerName = "Players";
        boss.GetComponent<SpriteRenderer>().sortingOrder = 0;
        SpawnParticle(tilePosition.position, "DarkMagicAura");
        InitBossStats();
    }

    void Awake()
    {
        WinScreen.SetActive(false);
        LossScreen.SetActive(false);
        profile = GameObject.Find("PDProfile").GetComponent<PDProfile>();
        p = GameObject.Find("PDProfile").GetComponent<Profile>();
    }
    // Use this for initialization
    void Start ()
    {
        players = new Dictionary<string, GameObject>();
        playersDmgDict = new Dictionary<string, float>();
        playersDmg = new List<KeyValuePair<string, float>>();
        totalPlayerHealth = 0;
        curPlayerHealth = 0;
        SpawnPlayers();
        SpawnBoss();
    }
}

//#define PD
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;
using DigitalRubyShared;
using UnityEngine.Networking;
using System.Collections;

public class Profile : MonoBehaviour
{
    public static Profile MyProfile;
    public int MyHash { get; set; }
    public string Logname { get; set; }
    public string Password { get; set; }
    public bool FirstSessionLog { get; set; }
    public bool LoopPrompt { get; set; }
    public GameData.Enemy EnemyToLoad { get; set; }
    public PlayerData playerData;
    public GameData gameData;
    public string reg, log, save, load, checkGym, items, enemies;

    public class PlayerData
    {
        public string AvatarName;
        public string AvatarID;

        public bool FirstLogin;
        public bool Badge1;
        public bool Badge2;
        public bool Badge3;

        public float ActivityStreak;

        public int ActivityPoints;
        public int ActivityPointsGained;
        public int Level;
        public int CurExperiencePoints;
        public int FreeStatPointsLeft;
        public int Gold;
        public int bestStageBeaten;

        public int[] BeatenStages;
        public GameData.Item[] inventory;
        public GameData.Item[] equipment;
        public Dictionary<string, float> stats = new Dictionary<string, float>();

        public void AddToPlayerInventory(GameData.Item item)
        {
            for (int i = 0; i < inventory.Length; i++)
            {
                if (inventory[i] == null)
                {
                    inventory[i] = item;
                    break;
                }
            }
        }
    }

    public class GameData
    {
        public enum InventorySlots { Active, Passive, Weapon, Usable, None }

        public class Item
        {
            public string ID; // <---search term and by which resource is found
            public string Name; //<---ingame full name / description
            public string Description;
            public InventorySlots Type;

            public int Cost, Charges;
            public float Duration;
            public Dictionary<string, float> stats;

            public Item()
            {
                ID = Name = Description = "";
                Type = InventorySlots.None;
                Cost = 0;
                Duration = 0;
                Charges = 0;
                stats = new Dictionary<string, float>();
            }
        }

        public class Enemy
        {
            public struct ItemAndChance
            {
                public float LootDropRate;
                public string itemID;
                public int amount;
            }

            public string ID, Name, Description;
            public int XP, MinGold, MaxGold, stage;
            public float spawnHeight;
            public ItemAndChance[] loot;
            public Dictionary<string, float> EnemyStats;

            public Enemy()
            {
                ID = Name = "";
                XP = MinGold = MaxGold = stage = 0;
                EnemyStats = new Dictionary<string, float>();
                spawnHeight = 0;
            }
        }

        //can't serialize dictionaries, so use two Lists each instead
        public List<Item> allItems = new List<Item>();
        public List<Enemy> allEnemies= new List<Enemy>();
    }

    public IEnumerator LoadGameData()
    {
        //loading all items from data base
        using (UnityWebRequest www = UnityWebRequest.Get(items))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                Debug.Log(www.downloadHandler.text);
            }
            else
            {
                Debug.Log("LoadGameData() items complete");
                string result = www.downloadHandler.text;

                string[] elementsZ = result.Split('\n');        //split first across new line

                for(int run = 0; run < elementsZ.Length - 1; run++)
                {
                    string[] elements = elementsZ[run].Split('\t');     //...then along tabs
                    for (int i = 0; i < elements.Length; i += 8)     //one item row has 8 columns
                    {
                        GameData.Item item = new GameData.Item();
                        item.ID = elements[i];
                        item.Name = elements[i + 1];
                        item.Description = elements[i + 2];
                        item.Type = (GameData.InventorySlots)Enum.Parse(typeof(GameData.InventorySlots), elements[i + 3]);
                        item.Cost = int.Parse(elements[i + 4]);
                        //last part is a string with all stats
                        string[] statsArr = elements[i + 5].Split(',');
                        //ONLY ADDS ITEMS THAT HAVE SOME STATS ATTACHED TO THEM
                        if (statsArr.Length > 1)
                        {
                            for (int j = 0; j < statsArr.Length; j += 2)
                            {
                                item.stats.Add(statsArr[j], float.Parse(statsArr[j + 1]));
                            }
                            gameData.allItems.Add(item);
                        }
                        item.Duration = float.Parse(elements[i + 6]);
                        item.Charges = int.Parse(elements[i + 7]);
                    }
                }
            }
        }
        //loading all enemies from data base
        using (UnityWebRequest www = UnityWebRequest.Get(enemies))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                Debug.Log(www.downloadHandler.text);
            }
            else
            {
                Debug.Log("LoadGameData() enemies complete");
                string result = www.downloadHandler.text;
                string[] elementsZ = result.Split('\n');
                int run = 0;
                while(run < elementsZ.Length - 1)
                {
                    string[] elements = elementsZ[run].Split('\t');
                    for (int i = 0; i < elements.Length; i += 9)        //one item row has 9 columns
                    {
                        GameData.Enemy enemy = new GameData.Enemy();
                        enemy.ID = elements[i];
                        enemy.Name = elements[i + 1];
                        enemy.Description = elements[i + 2];
                        enemy.XP = int.Parse(elements[i + 3]);
                        enemy.MinGold = int.Parse(elements[i + 4]);
                        enemy.MaxGold = int.Parse(elements[i + 5]);
                        enemy.stage = int.Parse(elements[i + 6]);
                        //process loot string
                        string[] lootArr = elements[i + 7].Split(',');      //one struct has in order: id, amount, chance
                        if (lootArr.Length > 1)
                        {
                            int size = lootArr.Length / 3;
                            enemy.loot = new GameData.Enemy.ItemAndChance[size];
                            for (int k = 0; k < lootArr.Length; k += 3)
                            {
                                GameData.Enemy.ItemAndChance ic;
                                ic.itemID = lootArr[k];
                                ic.amount = int.Parse(lootArr[k + 1]);
                                ic.LootDropRate = float.Parse(lootArr[k + 2]);
                                enemy.loot[k / 3] = ic;
                            }
                        }
                        //process stats string
                        string[] statsArr = elements[i + 8].Split(',');     //basically a pair of key, value
                        if (statsArr.Length > 1)
                        {
                            for (int j = 0; j < statsArr.Length; j += 2)
                            {
                                enemy.EnemyStats.Add(statsArr[j], float.Parse(statsArr[j + 1]));
                            }
                            gameData.allEnemies.Add(enemy);
                        }
                    }
                    run++;
                }
            }

#if PD
            //Public Display-VERSION
            PDProfile s = GetComponent<PDProfile>();
            s.InitURLs();
            s.StartCoroutine(s.LoadPlayers());
#else
            //Mobile App-VERSION
            StartCoroutine(Load());
#endif
        }
    }

    //saves playerInfo to database
    public IEnumerator Save(PlayerData playerData)
    {
        WWWForm form = new WWWForm();
        //playerdetails
        form.AddField("username", Logname);
        form.AddField("Avatarname", Logname);
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
            if(playerData.inventory[i] == null)
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
            if(playerData.equipment[i] == null)
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
        form.AddField("Stats", Dict2String(playerData.stats));

        UnityWebRequest www = UnityWebRequest.Post(save, form);
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError || www.downloadHandler.text != "0")
        {
            Debug.Log(www.error);
            Debug.Log("Could not save! " + www.downloadHandler.text);
        }
        else
        {
            Debug.Log("Player save complete!");
        }
    }
    //loads playerInfo from database
    public IEnumerator Load()
    {
        if (Logname == "") yield break;
        WWWForm form = new WWWForm();
        form.AddField("username", Logname);

        using (UnityWebRequest www = UnityWebRequest.Post(load, form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                Debug.Log(www.downloadHandler.text);
            }
            else
            {
                Debug.Log("Load() complete");
                string result = www.downloadHandler.text;
                string[] elements = result.Split('\t');

                playerData.AvatarID = elements[0];
                playerData.AvatarName = Logname;
                playerData.FirstLogin = int.Parse(elements[1]) == 1 ? true : false;

                playerData.Badge1 = int.Parse(elements[2]) == 1 ? true : false;
                playerData.Badge2 = int.Parse(elements[3]) == 1 ? true : false;
                playerData.Badge3 = int.Parse(elements[4]) == 1 ? true : false;

                playerData.ActivityStreak = float.Parse(elements[5]);
                playerData.ActivityPoints = int.Parse(elements[6]);
                playerData.ActivityPointsGained = int.Parse(elements[7]);

                playerData.Level = int.Parse(elements[8]);
                playerData.CurExperiencePoints = int.Parse(elements[9]);

                playerData.FreeStatPointsLeft = int.Parse(elements[10]);
                playerData.Gold = int.Parse(elements[11]);
                playerData.bestStageBeaten = int.Parse(elements[12]);

                //stage array process
                string a = elements[13];
                playerData.BeatenStages = new int[14];
                if ( a == "")
                {
                    for (int i = 0; i < playerData.BeatenStages.Length; i++)
                    {
                        playerData.BeatenStages[i] = 0;
                    }
                }
                else
                {
                    string[] stageResult = a.Split(',');
                    for (int i = 0; i < playerData.BeatenStages.Length; i++)
                    {
                        playerData.BeatenStages[i] = int.Parse(stageResult[i]);
                    }
                }
                
                //inventory array process
                string b = elements[14];
                playerData.inventory = new GameData.Item[20];

                if(b == "")
                {
                    for (int i = 0; i < playerData.inventory.Length; i++)
                    {
                        playerData.inventory[i] = null;
                    }
                }
                else
                {
                    string[] invResult = b.Split(',');
                    for (int i = 0; i < playerData.inventory.Length; i++)
                    {
                        if (invResult[i] != "NULL")
                        {
                            GameData.Item j = gameData.allItems.Find(x => x.Name.Equals(invResult[i]));
                            playerData.inventory[i] = j;
                        }
                        else
                        {
                            playerData.inventory[i] = null;
                        }
                    }
                }

                //equipment
                string c = elements[15];
                playerData.equipment = new GameData.Item[4];

                if(c == "")
                {
                    for (int i = 0; i < playerData.equipment.Length; i++)
                    {
                        playerData.equipment[i] = null;
                    }
                }
                else
                {
                    string[] eqResult = c.Split(',');
                    for (int i = 0; i < playerData.equipment.Length; i++)
                    {
                        if (eqResult[i] != "NULL")
                        {
                            GameData.Item j = gameData.allItems.Find(x => x.Name.Equals(eqResult[i]));
                            playerData.equipment[i] = j;
                        }
                        else
                        {
                            playerData.equipment[i] = null;
                        }
                    }
                }

                //stats
                string dictionary = elements[16];
                string[] res = dictionary.Split(',');

                for(int i = 0; i < res.Length - 1; i+=2)
                {
                    playerData.stats.Add(res[i], float.Parse(res[i + 1]));
                }
            }
        }
    }

    public string Dict2String(Dictionary<string,float> dict)
    {
        string res = "";
        foreach(KeyValuePair<string,float> p in dict)
        {
            res += p.Key + ",";
            res += p.Value.ToString() + ",";
        }
        return res;
    }

    public void InitURLS()
    {
        string path = Application.dataPath + "/config.txt";
        StreamReader reader = new StreamReader(path);
#if PD
        reader = new StreamReader(Application.dataPath + "/config.txt");
#else
        //reader = new StreamReader(Application.dataPath + "/config.txt");
#endif

        string allText = reader.ReadToEnd();
        string[] all = allText.Split('\n');
        string[] res = all[0].Split('\t');
        reg = res[1].Substring(0,res[1].Length - 1);             //to get rid of the \r at the end...s

        res = all[1].Split('\t');
        log = res[1].Substring(0, res[1].Length - 1);

        res = all[2].Split('\t');
        checkGym = res[1].Substring(0, res[1].Length - 1); 

        res = all[3].Split('\t');
        save = res[1].Substring(0, res[1].Length - 1);

        res = all[4].Split('\t');
        load = res[1].Substring(0, res[1].Length - 1);

        res = all[5].Split('\t');
        items = res[1].Substring(0, res[1].Length - 1);

        res = all[6].Split('\t');
        enemies = res[1].Substring(0, res[1].Length - 1);
    }

    void Awake()
    {
        if(MyProfile == null)
        {
            DontDestroyOnLoad(gameObject);
            MyProfile = this;
        }
        else if(MyProfile != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        playerData = new PlayerData();
        gameData = new GameData();
        FirstSessionLog = true;
        LoopPrompt = false;
        InitURLS();
#if PD
        StartCoroutine(LoadGameData());
#endif
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
    }

    //does not work on windows phone 8.1+ and iOS - ONLY SAVES PROFILE CHANGES, NOT GAMEDATA (for now?)
    public void OnApplicationQuit()
    {
        //Debug.Log("calling save");
#if PD
        return;
#else
        StartCoroutine(Save(playerData));
#endif
    }
}

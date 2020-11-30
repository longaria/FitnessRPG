using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DigitalRubyShared;
using System.Collections.Generic;

// Fight view screen
public class MAFightManager : MonoBehaviour
{
	public GameObject chest, crosshair, centerText, lootWindow, ButtonsPanel, enemyUlt, ultimate, UltBar, ItemCell, player, enemy, 
        fctPrefab, TutorialPanel1, TutorialPanel2;
	public Profile p;
    public bool CrosshairActive { get; set;}
    public bool Paused { get; set; }
    public bool End { get; set; }
    public bool TutorialOn { get; set; }
    public Dictionary<string, float> playerStats;
    public Profile.GameData.Enemy enemyCopy;

    enum HitObjects { Player, Enemy, EnemyProjectile };
    GameObject startObject, destinationObject;          //these are needed for drag and drop touch/ultimate
    int maxUlt, curUlt, tutorialPhase;
    bool ActiveOn, WeaponOut, UseCD;
    const float DEFAULTCOOLDOWN = 3; 

    #region Touchvariables
    TapGestureRecognizer tapRec;
	SwipeGestureRecognizer swipeRec;
    LongPressGestureRecognizer longPressGesture;
    #endregion
    // Use this for initialization
    void Awake()
    {
        p = GameObject.FindGameObjectWithTag("myProfile").GetComponent<Profile>();
    }

    void Start ()
    {
        //init gestures
        CreateTapGesture();
        CreateSwipeGesture();
        CreateLongPressGesture();  
        
        maxUlt = 100;
        curUlt = 0;

        Paused = false;
		CrosshairActive = false;
        End = false;
        TutorialOn = false;
        ActiveOn = UseCD = WeaponOut = false;
        tutorialPhase = 0;
        if (p.playerData.bestStageBeaten == 0)
        {
            TutorialOn = true;
            Tutorial();
        }
        else
        {
            Init();
        }
    }

    void Update()
    {
        Time.timeScale = Paused ? 0 : 1;
	}
    #region Touchfunctions
    void ClearTouchedObjects()
    {
        startObject = null;
        destinationObject = null;
    }

    void LongPressGestureCallback(GestureRecognizer gesture)
    {
        if (gesture.State == GestureRecognizerState.Began)
        {
            Vector2 v = new Vector2(gesture.StartFocusX, gesture.StartFocusY);
            startObject = CheckForObjectHit(v, HitObjects.Player);
        }

        else if (gesture.State == GestureRecognizerState.Ended)
        {
            Vector2 end = new Vector2(gesture.DistanceX + gesture.StartFocusX, gesture.DistanceY + gesture.StartFocusY);
            destinationObject = CheckForObjectHit(end, HitObjects.Enemy);

            if (startObject != null && destinationObject != null)
            {
                if (startObject.tag == "myPlayer" && destinationObject.tag == "myEnemy")
                {
                    if (UltBar.GetComponent<RectTransform>().localScale.x == 1.0f)
                    {
                        UnleashUltimate();
                        ClearTouchedObjects();
                    }
                }
            }
        }
    }

    void CreateLongPressGesture()
    {
        longPressGesture = new LongPressGestureRecognizer();
        longPressGesture.MaximumNumberOfTouchesToTrack = 1;
        longPressGesture.MinimumDurationSeconds = 0.2f;
        longPressGesture.StateUpdated += LongPressGestureCallback;
        FingersScript.Instance.AddGesture(longPressGesture);
    }

    //the starting point for the ray is simply the touch point itself, and the ray direction is unnecessary/zero
    //checks if specific objects are hit
    GameObject CheckForObjectHit(Vector2 pos, HitObjects h)
    {
        Vector2 start = Camera.main.ScreenToWorldPoint(pos);
        GameObject result = null;
        switch (h)
        {
            case HitObjects.EnemyProjectile:
                {
                    RaycastHit2D[] hits = Physics2D.CircleCastAll(start, 1.5f, Vector2.zero);

                    foreach (RaycastHit2D hit in hits)
                    {
                        if (hit.collider != null)
                        {
                            if (hit.collider.gameObject.tag == "projectile")
                            {
                                Projectile script = hit.collider.gameObject.GetComponent<Projectile>();
                                if (script.GetThrower() == 2) return hit.collider.gameObject;
                            }
                        }
                    }
                    return result;
                }
            case HitObjects.Player:
                {
                    RaycastHit2D[] hits = Physics2D.CircleCastAll(start, 1.0f, Vector2.zero);

                    foreach (RaycastHit2D hit in hits)
                    {
                        if (hit.collider != null)
                        {
                            if (hit.collider.gameObject.tag == "myPlayer") result = hit.collider.gameObject;
                        }
                    }
                    return result;
                }
            case HitObjects.Enemy:
                {
                    RaycastHit2D[] hits = Physics2D.CircleCastAll(start, 1.0f, Vector2.zero);

                    foreach (RaycastHit2D hit in hits)
                    {
                        if (hit.collider.gameObject.tag == "myEnemy") result = hit.collider.gameObject;
                    }
                    return result;
                }
            default:
                return result;
        }
    }

    //checks, if projectile is touched and swiped
    void SwipeRecCallback(GestureRecognizer gesture)
    {
        if (gesture.State == GestureRecognizerState.Ended)
        {
            Vector2 v = new Vector2(gesture.StartFocusX, gesture.StartFocusY);
            GameObject hitter = CheckForObjectHit(v, HitObjects.EnemyProjectile);
            if (hitter != null)
            {
                if (hitter.tag == "projectile" && hitter.gameObject.GetComponent<Projectile>().GetThrower() == 2)
                {
                    hitter.GetComponent<Projectile>().SetThrower(1);
                }
            }
        }  
    }

    void CreateSwipeGesture()
    {
        swipeRec = new SwipeGestureRecognizer();
        swipeRec.Direction = SwipeGestureRecognizerDirection.Right;
        swipeRec.StateUpdated += SwipeRecCallback;
        swipeRec.DirectionThreshold = 1.5f;
        FingersScript.Instance.AddGesture(swipeRec);
    }
    //tap checks tap on player, makes him jump, if positive
    void TapRecCallback(GestureRecognizer gesture)
    {
        if (gesture.State == GestureRecognizerState.Ended)
        {
            GameObject hitter = CheckForObjectHit(new Vector2(gesture.StartFocusX, gesture.StartFocusY), HitObjects.Player);
            if (hitter != null && hitter.tag == "myPlayer")
            {
                hitter.GetComponent<Player>().Jump();
            }
        }
    }

    void CreateTapGesture()
    {
        tapRec = new TapGestureRecognizer();
        tapRec.StateUpdated += TapRecCallback;
        FingersScript.Instance.AddGesture(tapRec);
    }
    #endregion

    public void Tutorial()
    {
        tutorialPhase++;
        switch (tutorialPhase)
        {
            case 1:
                TutorialPanel1.SetActive(true);
                break;
            case 2:
                TutorialPanel1.SetActive(false);
                TutorialPanel2.SetActive(true);
                break;
            default:
                TutorialOn = false;
                TutorialPanel2.SetActive(false);
                Init();
                break;
        }
        
    }

    public void CloseTutorial()
    {
        TutorialPanel1.SetActive(false);
        Init();
    }

    void Init()
    {
        InitActorStats();
        SetUI();
        SpawnPlayer();
        SpawnEnemy();
    }

    void UnleashUltimate()
    {
        UltBar.GetComponent<RectTransform>().localScale = new Vector3(0, 1.0f, 1.0f);
        curUlt = 0;
        GameObject inst = Instantiate(ultimate, new Vector3(12, 14, 0), Quaternion.Euler(0, 0, 0));
        inst.transform.localScale = new Vector3(8, 8, 1);
        float enemyDodge = enemyCopy.EnemyStats["DodgeChance"];
        enemyCopy.EnemyStats["DodgeChance"] = 0;
        float dmg = player.GetComponent<Player>().CalcDmg() * 1.5f;
        enemy.GetComponent<Enemy>().UpdateHealth((int)dmg);
        enemyCopy.EnemyStats["DodgeChance"] = enemyDodge;
    }

    public void DestroyAllProjectiles()
    {
        GameObject[] list = GameObject.FindGameObjectsWithTag("projectile");
        foreach (GameObject o in list)
        {
            Destroy(o);
        }
    }

    public void SpawnParticle(Vector3 pos, string particleName)
    {
        GameObject pref = Resources.Load<GameObject>("Prefabs/FX/" + particleName);
        GameObject particles = Instantiate(pref);
        particles.transform.position = pos;

        ParticleSystem ps = particles.GetComponent<ParticleSystem>();

        if (ps != null)
        {
            ParticleSystem[] parts = particles.GetComponentsInChildren<ParticleSystem>();
            foreach( ParticleSystem p in parts)
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

    public void SpawnPlayer()
    {
        GameObject AvatarToLoad = Resources.Load<GameObject>("Avatars/" + p.playerData.AvatarID);
        player = Instantiate(AvatarToLoad, new Vector3(-3.0f, -2.75f, 0), Quaternion.Euler(0, 0, 0));
        player.tag = "myPlayer";
        player.AddComponent<Player>();
        player.AddComponent<Rigidbody2D>();
        player.GetComponent<Rigidbody2D>().mass = 0.1f;
        player.GetComponent<Rigidbody2D>().angularDrag = 0;
        player.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
        player.GetComponent<SpriteRenderer>().sortingOrder = -1;
        player.GetComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Avatars/" + p.playerData.AvatarID);
        SpawnParticle(player.transform.position, "Spawn");
    }

    public void SetUI()
    {
        //player UI
        GameObject.FindGameObjectWithTag("playerName").GetComponent<Text>().text = p.playerData.AvatarName;
        GameObject.FindGameObjectWithTag("playerLevel").GetComponent<Text>().text = "Level " + p.playerData.Level.ToString();
        //enemy UI
        GameObject.FindGameObjectWithTag("enemyName").GetComponent<Text>().text = p.EnemyToLoad.Name;
        GameObject.FindGameObjectWithTag("enemyLevel").GetComponent<Text>().text = "Level " + p.EnemyToLoad.stage.ToString();
        //clear buttons of images and set charge text
        for (int i = 0; i < p.playerData.equipment.Length; i++)
        {
            Image b = GameObject.Find("ItemButtonImage" + (i + 1).ToString()).GetComponent<Image>();
            if(p.playerData.equipment[i] == null)
            {
                b.sprite = null;
                b.color = new Color(1, 1, 1, 0);
            }
            else
            {
                b.sprite = Resources.Load<Sprite>(p.playerData.equipment[i].ID);
                b.color = new Color(1, 1, 1, 1);
            }
        }
        GameObject.Find("ActiveChargeText").GetComponent<Text>().text = p.playerData.equipment[0] != null ? 
            p.playerData.equipment[0].Charges.ToString() : "";
        GameObject.Find("PassiveChargeText").GetComponent<Text>().text = p.playerData.equipment[1] != null ?
            p.playerData.equipment[1].Charges.ToString() : "";
        GameObject.Find("WeaponChargeText").GetComponent<Text>().text = p.playerData.equipment[2] != null ?
            p.playerData.equipment[2].Charges.ToString() : "";
        GameObject.Find("UsableChargeText").GetComponent<Text>().text = p.playerData.equipment[3] != null ?
            p.playerData.equipment[3].Charges.ToString() : "";
    }

    //deep copies of player and enemy, so buffs can be applied, not messing up profile data; TODO: need to update enemy tint and loot
    public void InitActorStats()
    {
        //player first -deep copy
        playerStats = new Dictionary<string, float>(p.playerData.stats);
        foreach (KeyValuePair<string, float> pair in p.playerData.stats)
        {
            playerStats[pair.Key] = playerStats[pair.Key] * p.playerData.ActivityStreak;
        }
        playerStats["AttackSpeed"] /= Mathf.Pow(p.playerData.ActivityStreak, 2);
        //activate passive skill
        Profile.GameData.Item item = p.playerData.equipment[1];
        StartCoroutine(UpdateStats(item, true, 0, 1));

        //enemy deep copy
        enemyCopy = new Profile.GameData.Enemy();
        float factor = 1 + (p.EnemyToLoad.stage/100);
        enemyCopy.EnemyStats = new Dictionary<string, float>(p.EnemyToLoad.EnemyStats);

        foreach (KeyValuePair<string, float> e in p.EnemyToLoad.EnemyStats)
        {
            enemyCopy.EnemyStats[e.Key] = e.Value * factor;
        }
        enemyCopy.EnemyStats["AttackSpeed"] /= Mathf.Pow(1.01f, factor) * factor;       //need to multiply once by factor because of loop before
        enemyCopy.EnemyStats["CritChance"] += (0.5f * (factor - 1)) / factor;
        enemyCopy.EnemyStats["DodgeChance"] += (0.5f * (factor - 1)) / factor;

        enemyCopy.XP = (int)(p.EnemyToLoad.XP * (factor - 1) * p.playerData.ActivityStreak);
        enemyCopy.MinGold = (int)(p.EnemyToLoad.MinGold * factor * p.playerData.ActivityStreak);
        enemyCopy.MaxGold = (int)(p.EnemyToLoad.MaxGold * factor * p.playerData.ActivityStreak);
    }

    public void SpawnEnemy()
    {
        GameObject EnemyToLoad = Resources.Load<GameObject>(p.EnemyToLoad.ID);
        enemy = Instantiate(EnemyToLoad, new Vector3(10.0f, p.EnemyToLoad.spawnHeight, 0), Quaternion.Euler(0, 180, 0));
        enemy.tag = "myEnemy";
        enemy.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
        enemy.GetComponent<SpriteRenderer>().sortingOrder = -1;
        enemy.GetComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(p.EnemyToLoad.ID);
        if (enemy.name.Contains("FlyingEye"))
        {
            enemy.transform.localScale = new Vector3(1.5f, 1.5f, 1);
            enemy.transform.localRotation = Quaternion.identity;
        }
        SpawnParticle(enemy.transform.position, "Spawn");
	}

	public void SpawnChest()
    {
		GameObject b =  Instantiate(chest, new Vector3(10,-2,0), Quaternion.identity);
        b.GetComponent<Animator>().SetBool("Open", true);
	}

    //shows Ultimate ready message
    public void ShowUltimateText(){

		Transform parent = GameObject.FindGameObjectWithTag ("myCanvas").transform;

		GameObject a = Instantiate (centerText, parent);
		a.GetComponent<RectTransform> ().localPosition = new Vector3 (0, 165, 0);
		a.GetComponent<RectTransform> ().localRotation = Quaternion.identity;
		a.GetComponent<RectTransform> ().localScale = Vector3.one;
		a.GetComponent<Text>().color = new Color(1.0f, 0.49f, 0.31f, 1.0f);
		a.GetComponent<Text> ().text = "Spezialattacke bereit!";
		a.GetComponent<Animator> ().Play ("lvlUp");
		Destroy (a, 2);
	}

	//fill ultimate bar, when full send notification and reset the bar
	public void UpdateUltimateBar(){

		//if the bar is full, do nothing and exit
		if (curUlt == maxUlt) return;
		//old scale x value (0-1)
		float scaleBefore = UltBar.GetComponent<RectTransform>().localScale.x;

		//update current bar value, if max value reached, show notification
		curUlt = Mathf.Min(curUlt + 20, maxUlt);
		if (curUlt == maxUlt) {
			ShowUltimateText ();
		}

		//new scale value (0-1)
		float scaleAfter = (float) curUlt / maxUlt;

		StartCoroutine(UpdateBar(scaleBefore, scaleAfter, UltBar));
	}

	//increases or decrease any bar smoothly
	public IEnumerator UpdateBar(float scaleBefore, float scaleEnd, GameObject bar)
    {

		float x = scaleBefore;

        if(scaleBefore >= scaleEnd)
        {
            //decrease
            while (x > scaleEnd)
            {
                bar.GetComponent<RectTransform>().localScale = new Vector3(x, 1.0f, 1.0f);
                x -= Time.deltaTime;
                UpdatePercentageText(bar);
                yield return new WaitForSeconds(Time.deltaTime);
            }

            x = scaleEnd;
            bar.GetComponent<RectTransform>().localScale = new Vector3(x, 1.0f, 1.0f);
            UpdatePercentageText(bar);
            yield return new WaitForSeconds(Time.deltaTime);
            if(x <= 0)
            {
                End = true;
                if (bar.tag == "playerHB")
                {
                    player.GetComponent<Animator>().SetBool("Dead", true);
                }
                if (bar.tag == "enemyHB")
                {
                    enemy.GetComponent<Animator>().SetBool("Dead", true);
                }
            }
        }
        else
        {
            //increase
            while (x < scaleEnd)
            {
                bar.GetComponent<RectTransform>().localScale = new Vector3(x, 1.0f, 1.0f);
                x += Time.deltaTime;
                UpdatePercentageText(bar);
                yield return new WaitForSeconds(Time.deltaTime);
            }

            x = scaleEnd;
            bar.GetComponent<RectTransform>().localScale = new Vector3(x, 1.0f, 1.0f);
            UpdatePercentageText(bar);
            yield return new WaitForSeconds(Time.deltaTime);
        }
	}

	void UpdatePercentageText(GameObject bar)
    {

		int percentage = Mathf.RoundToInt(100 * bar.GetComponent<RectTransform>().localScale.x); 

		if (bar.tag == "playerHB")
        {
			GameObject.FindGameObjectWithTag ("playerHBTxT").GetComponent<Text> ().text = percentage.ToString () + "%";
            return;
		}
        if(bar.tag == "enemyHB")
        {
			GameObject.FindGameObjectWithTag ("enemyHBTxT").GetComponent<Text> ().text = percentage.ToString () + "%";
            return;
		}
	}

    public IEnumerator UpdateStats(Profile.GameData.Item item, bool isPlayer, float duration, int call)
    {
        if (item == null) yield break;

        if (isPlayer)
        {
            foreach (KeyValuePair<string, float> kp in item.stats)
            {
                float value;
                if (playerStats.TryGetValue(kp.Key, out value))
                {
                    if (kp.Key == "DamageM" || kp.Key == "PhysicalDefenseM" || kp.Key == "MagicDefenseM" || kp.Key == "HealthM")
                    {
                        playerStats[kp.Key] *= value;
                    }
                    else if (kp.Key == "AttackSpeed")
                    {
                        playerStats[kp.Key] /= (1.01f * value);
                    }
                    else if (kp.Key == "Health")
                    {
                        player.GetComponent<Player>().UpdateHealth(-1 * (int)value);
                    }
                    else playerStats[kp.Key] += value;
                }
                else
                {
                    //some special key, that is not part of the normal stats - no idea yet how to handle
                }
            }
        }
        else
        {
            foreach (KeyValuePair<string, float> kp in item.stats)
            {
                float value;
                if (enemyCopy.EnemyStats.TryGetValue(kp.Key, out value))
                {
                    if (kp.Key == "DamageM" || kp.Key == "PhysicalDefenseM" || kp.Key == "MagicDefenseM" || kp.Key == "HealthM")
                    {
                        enemyCopy.EnemyStats[kp.Key] *= value;
                    }
                    else if (kp.Key == "AttackSpeed")
                    {
                        enemyCopy.EnemyStats[kp.Key] /= (1.01f * value);
                    }
                    else if (kp.Key == "Health")
                    {
                        enemyCopy.EnemyStats[kp.Key] += value;
                        enemy.GetComponent<Enemy>().UpdateHealth(-1 * (int)value);
                    }
                    else enemyCopy.EnemyStats[kp.Key] += value;
                }
                else
                {
                    //some special key, that is not part of the normal stats - no idea yet how to handle
                }
            }
        }
        if(duration > 0)
        {
            yield return new WaitForSeconds(duration);
            RevertStats(item, isPlayer, call);
        }
    }

    void RevertStats(Profile.GameData.Item item, bool isPlayer, int call)
    {
        if (item == null) return;

        if (isPlayer)
        {
            foreach (KeyValuePair<string, float> kp in item.stats)
            {
                float value;
                if (playerStats.TryGetValue(kp.Key, out value))
                {
                    if (kp.Key == "DamageM" || kp.Key == "PhysicalDefenseM" || kp.Key == "MagicDefenseM" || kp.Key == "HealthM")
                    {
                        playerStats[kp.Key] /= value;
                    }
                    else if (kp.Key == "AttackSpeed")
                    {
                        playerStats[kp.Key] *= (1.01f * value);
                    }
                    else if (kp.Key == "Health")
                    {
                        playerStats[kp.Key] -= value;
                        player.GetComponent<Player>().UpdateHealth(-1 * (int)value);
                    }
                    else playerStats[kp.Key] -= value;
                }
                else
                {
                    //some special key, that is not part of the normal stats - no idea yet how to handle
                }
            }
        }
        else
        {
            foreach (KeyValuePair<string, float> kp in item.stats)
            {
                float value;
                if (enemyCopy.EnemyStats.TryGetValue(kp.Key, out value))
                {
                    if (kp.Key == "DamageM" || kp.Key == "PhysicalDefenseM" || kp.Key == "MagicDefenseM" || kp.Key == "HealthM")
                    {
                        enemyCopy.EnemyStats[kp.Key] /= value;
                    }
                    else if (kp.Key == "AttackSpeed")
                    {
                        enemyCopy.EnemyStats[kp.Key] *= (1.01f * value);
                    }
                    else if (kp.Key == "Health")
                    {
                        enemyCopy.EnemyStats[kp.Key] -= value;
                        enemy.GetComponent<Enemy>().UpdateHealth((int)value);
                    }
                    else enemyCopy.EnemyStats[kp.Key] -= value;
                }
                else
                {
                    //some special key, that is not part of the normal stats - no idea yet how to handle
                }
            }
        }
    }

    //called when skip fight is clicked
    public void CalculateFightOutcome()
    {
        Paused = true;
        //calculate the bars
        Player playerScript = player.GetComponent<Player>();
        Enemy enemyScript = enemy.GetComponent<Enemy>();

        float playerHP = playerScript.curHealth;
        float enemyHP = enemyScript.curHealth;
        float playerDelay = playerScript.attackDelay;
        float enemyDelay = enemyScript.attackDelay;
        // starts with rest hp and delay from actors
        while (true)
        {
            if(playerDelay >= enemyDelay)
            {
                playerHP -= enemyScript.CalcDmg();
                if (playerHP <= 0)
                {
                    Paused = false;
                    StartCoroutine(EvaluateFight(false));
                    return;
                }
                //subtract enemy attack delay from player delay, which is larger
                playerDelay -= enemyDelay;
                enemyDelay = enemyCopy.EnemyStats["AttackSpeed"];
            }
            else
            {
                enemyHP -= playerScript.CalcDmg();
                if(enemyHP <= 0)
                {
                    Paused = false;
                    StartCoroutine(EvaluateFight(true));
                    return;
                }
                enemyDelay -= playerDelay;
                playerDelay = playerStats["AttackSpeed"];
            }
        }
    }
    //called when someone dies, or after fight is skipped
    public IEnumerator EvaluateFight(bool PlayerWon)
    {
        if (PlayerWon)
        {
            for(int i = 0; i < 20; i++)
            {
                int k = Random.Range(1, 3);
                string str = k == 1 ? "Fireworks" : "Fireworks2";
                float r = Random.Range(-3.5f, 9.5f);                //world space view range x [-10,16], y[-6,9]
                float s = Random.Range(-3, 6);
                SpawnParticle(new Vector3(r, s, 1), str);
                yield return new WaitForSeconds(.2f);
            }
        }
        else
        {
            yield return new WaitForSeconds(1.2f);
        }
        Paused = true;      //<---no redundancy because this function can be called independent of calculateFightOutcome
        ButtonsPanel.SetActive(false);
        lootWindow.SetActive(true);

        //set loot window title
        GameObject.Find("Titeltext").GetComponent<Text>().text = PlayerWon ? "Sieg!" : "Verloren!";

        //setup Gold Item and add to profile
        int goldWon = PlayerWon ? Mathf.RoundToInt(Random.Range(p.EnemyToLoad.MinGold, p.EnemyToLoad.MaxGold + 1) * p.playerData.ActivityStreak) : 
            Mathf.RoundToInt((p.EnemyToLoad.MinGold / 2) * p.playerData.ActivityStreak);
        GameObject first = Instantiate(ItemCell, GameObject.Find("Loot").transform);
        first.name = "first";
        Text[] texts = first.GetComponentsInChildren<Text>();
        if(texts[0].text == "0")
        {
            texts[0].text = goldWon.ToString();
            texts[1].text = "Gold";
        }
        else
        {
            texts[0].text = "Gold"; 
            texts[1].text = goldWon.ToString();
        }
        p.playerData.Gold += goldWon;

        //setup XP Item and add to profile
        GameObject second = Instantiate(ItemCell, GameObject.Find("Loot").transform);
        second.name = "second";
        second.GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
        int xpWon = PlayerWon ? Mathf.RoundToInt(p.EnemyToLoad.XP * p.playerData.ActivityStreak) : 
            Mathf.RoundToInt((p.EnemyToLoad.XP / 2) * p.playerData.ActivityStreak);
        texts = second.GetComponentsInChildren<Text>();
        if (texts[0].text == "0")
        {
            texts[0].text = xpWon.ToString();
            texts[1].text = "Erfahrung";
        }
        else
        {
            texts[0].text = "Erfahrung";
            texts[1].text = xpWon.ToString();
        }
        p.playerData.CurExperiencePoints += xpWon;

        int nextLvlXP = 0;
        for (int i = 1; i <=p.playerData.Level; i++)
        {
            nextLvlXP += i * 75;
        }
        while(p.playerData.CurExperiencePoints > 0 && 
            (p.playerData.CurExperiencePoints) >= nextLvlXP)
        {
            p.playerData.Level++;
            nextLvlXP += p.playerData.Level * 75;
            p.playerData.FreeStatPointsLeft += 5;
        }

        if (PlayerWon)
        {
            //check players HP and set the right button in array
            float playerHPScale = GameObject.FindGameObjectWithTag("playerHB").GetComponent<RectTransform>().localScale.x;
            int x = playerHPScale >= 0.66f ? 3 : playerHPScale >= 0.33f ? 2 : 1;
            //stage is actual stage: begins at 1, so needs to be set minus 1, mod 14
            int arrayStage = (p.EnemyToLoad.stage - 1) % 14 ;
            p.playerData.BeatenStages[arrayStage] = Mathf.Max(p.playerData.BeatenStages[arrayStage], x);
            //if the beaten stage is larger than the current best beaten stage, change
            if (p.playerData.bestStageBeaten < p.EnemyToLoad.stage) p.playerData.bestStageBeaten = p.EnemyToLoad.stage;

            //setup all item-Items
            int j = 1;
            foreach (Profile.GameData.Enemy.ItemAndChance i in p.EnemyToLoad.loot)
            {
                float roll = Random.Range(0, 101) / 100.0f;
                //if roll successfull
                if (i.LootDropRate <= roll)
                {
                    GameObject third = Instantiate(ItemCell, GameObject.Find("Loot").transform);
                    third.name = "third" + j.ToString();
                    j++;
                    Profile.GameData.Item k = p.gameData.allItems.Find(y => y.ID.Equals(i.itemID));
                    third.GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>(i.itemID);
                    texts = third.GetComponentsInChildren<Text>();
                    if (texts[0].text == "0")
                    {
                        texts[0].text = "1";
                        texts[1].text = k.Name;
                    }
                    else
                    {
                        texts[0].text = k.Name;
                        texts[1].text = "1";
                    }
                    p.playerData.AddToPlayerInventory(k);
                }
            }
            if (p.EnemyToLoad.stage % 14 == 0) p.LoopPrompt = true;
        }
    }

    public void OnClickItem(int slot)
    {
        if (TutorialOn) return;
        GameObject Button = GameObject.Find("ItemButtonBG" + (slot+1).ToString());
        SpawnParticle(Button.transform.position, "Click");
        Text t;
        int i;
        if (p.playerData.equipment[slot] == null) return;
        switch (slot)
        {
            case 0:
                //active slot
                t = GameObject.Find("ActiveChargeText").GetComponent<Text>();                
                if (!ActiveOn && int.Parse(t.text) > 0)
                {
                    ActiveOn = true;
                    //reduce charge by 1
                    i = int.Parse(t.text) - 1;
                    t.text = i.ToString();
                    //set a timer for effect duration in seconds
                    ActivateButton(slot);
                }
                break;

            case 2:
                //throw slot
                t = GameObject.Find("WeaponChargeText").GetComponent<Text>();                
                if (!WeaponOut && int.Parse(t.text) > 0)
                {
                    WeaponOut = true;
                    i = int.Parse(t.text) - 1;
                    t.text = i.ToString();
                    ActivateButton(slot);
                }
                break;
            case 3:
                //potion
                t = GameObject.Find("UsableChargeText").GetComponent<Text>();                
                if (!UseCD && int.Parse(t.text) > 0)
                {
                    UseCD = true;
                    i = int.Parse(t.text) - 1;
                    t.text = i.ToString();
                    ActivateButton(slot);
                }
                break;
            default:
                return;
        }
	}
    //handles buttons clicked
    void ActivateButton(int call)
    {
        Profile.GameData.Item item = p.playerData.equipment[call];
        //get CD text
        switch (call)
        {
            case 0:
                player.GetComponent<Player>().DisplayFCTMsg(item.Name + "!");
                StartCoroutine(UpdateStats(item, true, item.Duration, 0));
                break;

            case 2:
                GameObject prefab = Resources.Load<GameObject>("Prefabs/MAFights/projectile");
                GameObject projectile = Instantiate(prefab, new Vector3(0, 1, 0), Quaternion.Euler(0, 0, 90));
                projectile.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(item.ID);
                projectile.AddComponent<BoxCollider2D>();
                projectile.GetComponent<BoxCollider2D>().isTrigger = true;
                projectile.GetComponent<Rigidbody2D>().AddForce(new Vector2(1, 1) * 50);
                Destroy(projectile, 3);
                break;

            case 3:
                player.GetComponent<Player>().DisplayFCTMsg(item.Name + "!");
                StartCoroutine(UpdateStats(item, true, item.Duration, 3));
                break;
            default:
                return;
        }
        if (item.Duration > 0)
        {
            StartCoroutine(ItemDuration(item.Duration, call));
        }
        else StartCoroutine(ItemDuration(DEFAULTCOOLDOWN, call));             //if no duration on item, the default cooldown is used
    }
    //shows cooldown/duration text underneath the pressed ui button
    public IEnumerator ItemDuration(float Duration, int call)
    {
        Text t = null;
        t = call == 0 ? GameObject.Find("ActiveCDText").GetComponent<Text>() : call == 2 ? GameObject.Find("WeaponCDText").GetComponent<Text>() :
            GameObject.Find("UsableCDText").GetComponent<Text>();

        float f = Duration;
        t.text = f.ToString("F1") + " sek";
        //count the cd down
        while (f > 0)
        {
            f -= Time.deltaTime;
            t.text = f.ToString("F1") + " sek";
            yield return new WaitForSeconds(Time.deltaTime);
        }
        t.text = "";
        //unlocks buttons again
        if (call == 0) ActiveOn = false;
        else if (call == 2) WeaponOut = false;
        else UseCD = false;
    }

    public void ActivateCrosshair()
    {
		Transform parent = GameObject.FindGameObjectWithTag ("myCanvas").transform;

		GameObject a = Instantiate (crosshair, parent);
		a.GetComponent<RectTransform> ().localPosition = new Vector3 (530, -127, 0);
		a.GetComponent<RectTransform> ().localRotation = Quaternion.identity;
		a.GetComponent<RectTransform> ().localScale = Vector3.one;
		CrosshairActive = true;
	}

    //called by result panel button
	public void LoadMAMap()
    {
        Paused = false;
		SceneManager.LoadScene ("MAWorld");
	}
}

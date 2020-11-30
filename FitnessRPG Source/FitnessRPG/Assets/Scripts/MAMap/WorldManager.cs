using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DigitalRubyShared;
using System;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class WorldManager : MonoBehaviour
{
    public Text clockText;
    public GameObject shopPanel, inventoryPanel, stagePreviewPanel, charPanel, APGainPanel, selectedItem, payButton, equipButton, avatar, stageNumber,
        tut1, tut2, tut3, tut4, tut4a, tut5, tut5a, tut6, tut7, tut8, tut9, tut10, tut11, avatarConfirm, shopInspectionCell, shopSellingCell, StatText, StatTextNum, 
        Str, Dex, Int, Vit, loopPanel;
    public Canvas canv;
    public Camera c;

    Profile p;
    PanGestureRecognizer panRec;
    bool panning, tutorialOn, uiElementOn;
    int tutorialPhase, mapStage, actualStage;
    string avatarPrefab;
    GameObject AvatarToLoad, EnemyToLoad;

    void Awake()
    {
        p = GameObject.FindGameObjectWithTag("myProfile").GetComponent<Profile>();        
    }

    // Use this for initialization
    void Start ()
    {
        CreatePanGesture();

        panning = false;
        uiElementOn = false;

        if (p.playerData.FirstLogin)
        {
            tutorialPhase = 0;
            tutorialOn = true;
            p.playerData.FreeStatPointsLeft = 5;
            StartTutorial();
        }
        else
        {
            AvatarToLoad = Resources.Load<GameObject>("Avatars/" + p.playerData.AvatarID);
            DrawStats();
        }
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == UnityEngine.TouchPhase.Ended || Input.GetMouseButtonUp(0))
        {
            if (!panning && !uiElementOn)
            {
                Vector2 pos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                CheckWhatIsHit(pos);
            }
        }
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            SceneManager.LoadScene("PDFight");
        }
    }
    #region Gestures
    private void PanGestureCallback(GestureRecognizer gesture)
    {
        if (gesture.State == GestureRecognizerState.Executing)
        {
            panning = true;
            float deltaY = panRec.DeltaY / 25.0f;
            Vector3 pos = c.transform.position;
            pos.y -= deltaY;
            if (pos.y <= -5.4f) pos.y = -5.4f;
            if (pos.y >= 25.92f) pos.y = 25.92f;
            c.transform.position = pos;
        }
        if (gesture.State == GestureRecognizerState.Ended)
        {
            panning = false;
        }
    }

    private void CreatePanGesture()
    {
        panRec = new PanGestureRecognizer();
        panRec.MinimumNumberOfTouchesToTrack = 1;
        panRec.StateUpdated += PanGestureCallback;
        FingersScript.Instance.AddGesture(panRec);
    }
    #endregion
    #region Tutorial functions
    public void StartTutorial()
    {
        uiElementOn = true;
        tutorialPhase++;
        switch (tutorialPhase)
        {
            case 1://welcome screen
                tut1.SetActive(true);
                break;
            case 2://world map intro
                tut1.SetActive(false);
                tut2.SetActive(true);
                break;
            case 3://avatar intro
                tut2.SetActive(false);
                tut3.SetActive(true);
                GameObject.Find("SubCanvasAvatar").GetComponent<Canvas>().overrideSorting = true;
                GameObject.Find("SubCanvasAvatar").GetComponent<Canvas>().sortingLayerName = "myLayer";
                break;
            case 4://character view intro
                GameObject.Find("SubCanvasAvatar").GetComponent<Canvas>().sortingLayerName = "BackgroundSortingLayer";
                GameObject.Find("SubCanvasAvatar").GetComponent<Canvas>().overrideSorting = false;
                tut3.SetActive(false);
                tut4.SetActive(true);
                break;
            case 5://character stat intro
                tut4.SetActive(false);
                tut4a.SetActive(true);
                break;
            case 6://gear intro
                tut4a.SetActive(false);
                closeCharPanel();
                tut5.SetActive(true);
                GameObject.Find("SubCanvasButtons").GetComponent<Canvas>().overrideSorting = true;
                GameObject.Find("SubCanvasButtons").GetComponent<Canvas>().sortingLayerName = "myLayer";
                break;
            case 7://gear view
                tut5.SetActive(false);
                GameObject.Find("SubCanvasButtons").GetComponent<Canvas>().sortingLayerName = "BackgroundSortingLayer";
                GameObject.Find("SubCanvasButtons").GetComponent<Canvas>().overrideSorting = false;
                tut6.SetActive(true);
                break;
            case 8://shop intro
                tut6.SetActive(false);
                closeGearPanel();
                tut5a.SetActive(true);
                GameObject.Find("SubCanvasButtons1").GetComponent<Canvas>().overrideSorting = true;
                GameObject.Find("SubCanvasButtons1").GetComponent<Canvas>().sortingLayerName = "myLayer";
                break;
            case 9://shop view
                tut5a.SetActive(false);
                GameObject.Find("SubCanvasButtons1").GetComponent<Canvas>().sortingLayerName = "BackgroundSortingLayer";
                GameObject.Find("SubCanvasButtons1").GetComponent<Canvas>().overrideSorting = false;
                openShopPanel();
                tut7.SetActive(true);
                break;
            case 10://fight stage and AP intro
                tut7.SetActive(false);
                closeShopPanel();
                tut8.SetActive(true);
                GameObject[] list = GameObject.FindGameObjectsWithTag("MALevelButton");
                foreach( GameObject a in list)
                {
                    a.GetComponent<SpriteRenderer>().sortingLayerName = "myLayer";
                }
                GameObject.Find("SubCanvasAPBar").GetComponent<Canvas>().overrideSorting = true;
                GameObject.Find("SubCanvasAPBar").GetComponent<Canvas>().sortingLayerName = "myLayer";
                break;
            case 11://fight preview view
                tut8.SetActive(false);
                list = GameObject.FindGameObjectsWithTag("MALevelButton");
                foreach (GameObject a in list)
                {
                    a.GetComponent<SpriteRenderer>().sortingLayerName = "BackgroundSortingLayer";
                }
                GameObject.Find("SubCanvasAPBar").GetComponent<Canvas>().overrideSorting = false;
                stagePreviewPanel.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.86f);
                tut9.SetActive(true);
                break;
            case 12://tutorial
                tut9.SetActive(false);
                stagePreviewPanel.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                closeStagePreview();
                tut10.SetActive(true);
                break;
            case 13://avatar choice
                tut10.SetActive(false);
                tut11.SetActive(true);
                break;
            default:
                tut11.SetActive(false);
                p.playerData.FirstLogin = false;
                DrawStats();
                return;
        }
    }

    public void SelectAvatarImage(GameObject clicked)
    {
        avatarConfirm.SetActive(true);
        GameObject ImageChild = avatarConfirm.transform.GetChild(3).gameObject;
        ImageChild.GetComponent<Image>().sprite = clicked.GetComponent<Image>().sprite;
        ImageChild.GetComponent<Image>().preserveAspect = true;
        avatarPrefab = clicked.name;
    }

    public void DeselectAvatarImage()
    {
        GameObject ImageChild = avatarConfirm.transform.GetChild(3).gameObject;
        ImageChild.GetComponent<Image>().sprite = null;
        avatarConfirm.SetActive(false);
    }
    //sets the avatarID in profile
    public void ConfirmAvatar()
    {
        p.playerData.AvatarID = avatarPrefab;
        AvatarToLoad = Resources.Load<GameObject>("Avatars/" + avatarPrefab);
        StartTutorial();
    }
    #endregion

    //needs to update all visuals to match progress from player profile
    public void DrawStats()
    {
        //set playername
        Text playername = GameObject.Find("Playername").GetComponent<Text>();
        playername.text = p.playerData.AvatarName;
        
        //set playerLevel
        Text playerlevel = GameObject.Find("Playerlevel").GetComponent<Text>();
        playerlevel.text = "Lvl " + p.playerData.Level;
        
        //set XP text and bar
        int totalXP = 0;
        for(int i = 1; i <= p.playerData.Level; i++)
        {
            totalXP += i * 75;
        }
        int curXP = p.playerData.CurExperiencePoints;
        float scale = (float) curXP / totalXP;
        Text playerXP = GameObject.Find("XPPoints").GetComponent<Text>();
        playerXP.text = curXP + "/" + totalXP;
        GameObject.Find("XPFG").GetComponent<Transform>().localScale = new Vector3(scale, 1, 1);

        //set AP text and bar
        int totalAP = 150;
        int curAP = p.playerData.ActivityPoints;
        scale = (float)curAP / totalAP;
        Text playerAP = GameObject.Find("APPoints").GetComponent<Text>();
        playerAP.text = curAP + "/" + totalAP;
        GameObject.Find("APFG").GetComponent<Transform>().localScale = new Vector3(scale, 1, 1);

        //set Avatar image and animator
        GameObject av = Resources.Load<GameObject>("Avatars/" + p.playerData.AvatarID);
        avatar.GetComponent<Image>().sprite = av.GetComponent<SpriteRenderer>().sprite;                 //doesnt work if sprite name identical??
        RuntimeAnimatorController anim = Resources.Load("Avatars/" + p.playerData.AvatarID + "UI") as RuntimeAnimatorController;
        avatar.GetComponent<Animator>().runtimeAnimatorController = anim;

        //draw stage buttons
        updateStages();

        initInventory();
        InitShop();
        initCharPanel();
        initStagePanel();

        //finally open APGain Panel, only on first session log (not on scene switch/reload)
        if (p.FirstSessionLog)
        {
            p.FirstSessionLog = false;
            //dont open at all, if no AP was gained
            if (p.playerData.ActivityPointsGained == 0) return;

            APGainPanel.SetActive(true);

            if (tutorialOn)
            {
                //play a different version of AP Gain panel
                GameObject.Find("MiddleText").GetComponent<Text>().text = "Als Dankeschön für dein\n erstes Login bekommst du:\n\n";
            }
            else
            {
                GameObject.Find("MiddleText").GetComponent<Text>().text = "Gewonnene Aktivitätspunkte\nseit letztem Login:\n\n";
            }
            GameObject.Find("APTextAnimation").GetComponent<Text>().text = p.playerData.ActivityPointsGained.ToString() + " Aktivitätspunkte (AP)";
        }

        if (p.LoopPrompt)
        {
            loopPanel.SetActive(true);
        }
    }
    //called when the "weiter" button is clicked on the APGain panel
    public void PlayAPAnim()
    {        
        GameObject.Find("APBody").SetActive(false);
        GameObject.Find("APTextAnimation").GetComponent<Animator>().Play("APAnim");
        //set playerAP in data
        p.playerData.ActivityPoints += p.playerData.ActivityPointsGained;
        p.playerData.ActivityPointsGained = 0;
        //set APtext and bar
        int totalAP = 150;
        int curAP = p.playerData.ActivityPoints;
        float scale = (float)curAP / totalAP;
        Text playerAP = GameObject.Find("APPoints").GetComponent<Text>();
        playerAP.text = curAP + "/" + totalAP;
        Vector3 v = GameObject.Find("APFG").GetComponent<Transform>().localScale;
        GameObject.Find("APFG").GetComponent<Transform>().localScale = new Vector3(scale, v.y, v.z);
        
        tutorialOn = false;
        uiElementOn = false;
        Destroy(APGainPanel, 1.1f);
    }
    //checks what is hit with a touch or mouse click
    void CheckWhatIsHit(Vector2 pos)
    {
        Vector2 start = Camera.main.ScreenToWorldPoint(pos);
        RaycastHit2D hit = Physics2D.Raycast(start, Vector2.zero);
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.tag == "MALevelButton" && !panning)
            {
                // is 1-14
                mapStage = Convert.ToInt32(hit.collider.gameObject.name);
                if (tutorialOn)
                {
                    if(tutorialPhase == 10)
                    {
                        openStagePreview(0);
                        StartTutorial();
                    }
                }
                else
                {
                    openStagePreview(mapStage);
                }
            }
        }
    }
    #region Stages
    //clears out the filled out text
    private void initStagePanel()
    {
        stagePreviewPanel.SetActive(true);
        Destroy(GameObject.Find("Weakness"));
        Destroy(GameObject.Find("Strengths"));
        Destroy(GameObject.Find("1S"));
        Destroy(GameObject.Find("2S"));
        Destroy(GameObject.Find("1W"));
        Destroy(GameObject.Find("2W"));
        Destroy(GameObject.Find("Reward1"));
        Destroy(GameObject.Find("Reward2"));
        Destroy(GameObject.Find("Reward3"));
        Destroy(GameObject.Find("Reward4"));
        stagePreviewPanel.SetActive(false);
    }

    //called every time stages need to be updated
    public void updateStages()
    {
        int curStage = p.playerData.bestStageBeaten;
        //check edge case: best beaten stage is multiple of 14
        if (curStage % 14 == 0)
        {
            //infer from stageArray, if looping was performed: check if any non-zero position has a non-zero number
            if (p.playerData.BeatenStages[13] == 0)
            {
                //looping was performed so stage numbers begin at:
                curStage++;
            }
            else
            {
                //no looping was performed, so stage numbers begin at:
                curStage -= 13;
            }
        }
        else
        {
            curStage = curStage - (curStage % 14) + 1;
        }
        //TODO: so far works only for stage 1-99, no third decimals
        for (int i = 0; i < p.playerData.BeatenStages.Length; i++)
        {
            //set button images k = [1-4]
            int k = 1 + p.playerData.BeatenStages[i];
            GameObject.Find((i+1).ToString()).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Buttons/button0" + k.ToString());

            int firstDecimal = curStage / 10;
            int secondDecimal = curStage % 10;

            //update map button numbers
            string appendage = (p.playerData.BeatenStages[i] == 0) ? "-gray" : "";
            GameObject parent = GameObject.Find((i + 1).ToString());
            //only one decimal
            if (firstDecimal == 0)
            {
                GameObject stage = Instantiate(stageNumber, parent.transform);
                stage.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Buttons/num" + secondDecimal.ToString() + appendage);
                Transform t = stage.GetComponent<Transform>();
                t.localPosition = new Vector3(-0.13f, -0.0362f, 0);
            }
            else
            {
                GameObject stage = Instantiate(stageNumber, parent.transform);
                stage.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Buttons/num" + secondDecimal.ToString() + appendage);
                Transform t = stage.GetComponent<Transform>();
                t.localPosition = new Vector3(-0.06f, -0.0362f, 0);

                stage = Instantiate(stageNumber, parent.transform);
                stage.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Buttons/num" + firstDecimal.ToString() + appendage);
                t = stage.GetComponent<Transform>();
                t.localPosition = new Vector3(-0.25f, -0.0362f, 0);
            }
            curStage++;
        } 
    }

    //called when stage looping was confirmed (make prompt for it)
    public void loopStages()
    {
        //update profile array all to zero
        for (int i = 0; i < p.playerData.BeatenStages.Length; i++)
        {
            p.playerData.BeatenStages[i] = 0;
        }
        updateStages();

        //position main camera to bottom again
        Camera.main.GetComponent<Transform>().position = new Vector3(0, -5.4f, -10);
    }

    public void openStagePreview(int stage)
    {
        //highest playable stage
        int max = p.playerData.bestStageBeaten + 1;
        //only open stagePreview of beaten or current stage
        if (stage <= (max % 14))
        {
            //real gamestage number = 1-14 + ...anyways..works
            actualStage = stage + p.playerData.bestStageBeaten - (p.playerData.bestStageBeaten % 14);
            stagePreviewPanel.SetActive(true);
            uiElementOn = true;

            GameObject enemy = GameObject.Find("EnemyImage");
            string name = "";
            if (tutorialOn) stage = 15;
            switch (stage - 1)
            {
                case 0:
                    name = "Boss/Cat";
                    p.EnemyToLoad = p.gameData.allEnemies.Find(x => x.ID.Equals(name));
                    enemy.GetComponent<Image>().sprite = Resources.Load<GameObject>(name).GetComponent<SpriteRenderer>().sprite;
                    enemy.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 180, 0);
                    p.EnemyToLoad.spawnHeight = -3f;
                    enemy.GetComponent<Animator>().runtimeAnimatorController = Resources.Load("Boss/CatUI") as RuntimeAnimatorController;
                    break;
                case 1:
                    name = "Boss/Dog";
                    p.EnemyToLoad = p.gameData.allEnemies.Find(x => x.ID.Equals(name));
                    enemy.GetComponent<Image>().sprite = Resources.Load<GameObject>(name).GetComponent<SpriteRenderer>().sprite;
                    enemy.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 180, 0);
                    p.EnemyToLoad.spawnHeight = -3f;
                    enemy.GetComponent<Animator>().runtimeAnimatorController = Resources.Load("Boss/DogUI") as RuntimeAnimatorController;
                    break;
                case 2:
                    name = "Boss/Orc1";
                    p.EnemyToLoad = p.gameData.allEnemies.Find(x => x.ID.Equals(name));
                    enemy.GetComponent<Image>().sprite = Resources.Load<GameObject>(name).GetComponent<SpriteRenderer>().sprite;
                    enemy.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 180, 0);
                    p.EnemyToLoad.spawnHeight = -3f;
                    enemy.GetComponent<Animator>().runtimeAnimatorController = Resources.Load("Boss/Orc1UI") as RuntimeAnimatorController;
                    break;
                case 3:
                    name = "Boss/Orc2";
                    p.EnemyToLoad = p.gameData.allEnemies.Find(x => x.ID.Equals(name));
                    enemy.GetComponent<Image>().sprite = Resources.Load<GameObject>(name).GetComponent<SpriteRenderer>().sprite;
                    enemy.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 180, 0);
                    p.EnemyToLoad.spawnHeight = -3f;
                    enemy.GetComponent<Animator>().runtimeAnimatorController = Resources.Load("Boss/Orc2UI") as RuntimeAnimatorController;
                    break;
                case 4:
                    name = "Boss/Orc3";
                    p.EnemyToLoad = p.gameData.allEnemies.Find(x => x.ID.Equals(name));
                    enemy.GetComponent<Image>().sprite = Resources.Load<GameObject>(name).GetComponent<SpriteRenderer>().sprite;
                    enemy.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 180, 0);
                    p.EnemyToLoad.spawnHeight = -3f;
                    enemy.GetComponent<Animator>().runtimeAnimatorController = Resources.Load("Boss/Orc3UI") as RuntimeAnimatorController;
                    break;
                case 5:
                    name = "Boss/ZombieFemale";
                    p.EnemyToLoad = p.gameData.allEnemies.Find(x => x.ID.Equals(name));
                    enemy.GetComponent<Image>().sprite = Resources.Load<GameObject>(name).GetComponent<SpriteRenderer>().sprite;
                    enemy.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 180, 0);
                    p.EnemyToLoad.spawnHeight = 0.85f;
                    enemy.GetComponent<Animator>().runtimeAnimatorController = Resources.Load("Boss/ZombieFemaleUI") as RuntimeAnimatorController;
                    break;
                case 6:
                    name = "Boss/ZombieMale";
                    p.EnemyToLoad = p.gameData.allEnemies.Find(x => x.ID.Equals(name));
                    enemy.GetComponent<Image>().sprite = Resources.Load<GameObject>(name).GetComponent<SpriteRenderer>().sprite;
                    enemy.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 180, 0);
                    p.EnemyToLoad.spawnHeight = 0.38f;
                    enemy.GetComponent<Animator>().runtimeAnimatorController = Resources.Load("Boss/ZombieMaleUI") as RuntimeAnimatorController;
                    break;
                case 7:
                    name = "Boss/Pumpkin";
                    p.EnemyToLoad = p.gameData.allEnemies.Find(x => x.ID.Equals(name));
                    enemy.GetComponent<Image>().sprite = Resources.Load<GameObject>(name).GetComponent<SpriteRenderer>().sprite;
                    enemy.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 180, 0);
                    p.EnemyToLoad.spawnHeight = 0.5f;
                    enemy.GetComponent<Animator>().runtimeAnimatorController = Resources.Load("Boss/PumpkinUI") as RuntimeAnimatorController;
                    break;
                case 8:
                    name = "Boss/Dino";
                    p.EnemyToLoad = p.gameData.allEnemies.Find(x => x.ID.Equals(name));
                    enemy.GetComponent<Image>().sprite = Resources.Load<GameObject>(name).GetComponent<SpriteRenderer>().sprite;
                    enemy.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 180, 0);
                    p.EnemyToLoad.spawnHeight = 0.8f;
                    enemy.GetComponent<Animator>().runtimeAnimatorController = Resources.Load("Boss/DinoUI") as RuntimeAnimatorController;
                    break;
                case 9:
                    name = "Boss/AlienGreen";
                    p.EnemyToLoad = p.gameData.allEnemies.Find(x => x.ID.Equals(name));
                    enemy.GetComponent<Image>().sprite = Resources.Load<GameObject>(name).GetComponent<SpriteRenderer>().sprite;
                    enemy.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 180, 0);
                    p.EnemyToLoad.spawnHeight = -3f;
                    enemy.GetComponent<Animator>().runtimeAnimatorController = Resources.Load("Boss/AlienGreenUI") as RuntimeAnimatorController;
                    break;
                case 10:
                    name = "Boss/Troll1";
                    p.EnemyToLoad = p.gameData.allEnemies.Find(x => x.ID.Equals(name));
                    enemy.GetComponent<Image>().sprite = Resources.Load<GameObject>(name).GetComponent<SpriteRenderer>().sprite;
                    enemy.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 180, 0);
                    p.EnemyToLoad.spawnHeight = -3f;
                    enemy.GetComponent<Animator>().runtimeAnimatorController = Resources.Load("Boss/Troll1UI") as RuntimeAnimatorController;
                    break;
                case 11:
                    name = "Boss/Troll2";
                    p.EnemyToLoad = p.gameData.allEnemies.Find(x => x.ID.Equals(name));
                    enemy.GetComponent<Image>().sprite = Resources.Load<GameObject>(name).GetComponent<SpriteRenderer>().sprite;
                    enemy.GetComponent<RectTransform>().rotation = Quaternion.identity;
                    p.EnemyToLoad.spawnHeight = -3f;
                    enemy.GetComponent<Animator>().runtimeAnimatorController = Resources.Load("Boss/Troll2UI") as RuntimeAnimatorController;
                    break;
                case 12:
                    name = "Boss/Troll3";
                    p.EnemyToLoad = p.gameData.allEnemies.Find(x => x.ID.Equals(name));
                    enemy.GetComponent<Image>().sprite = Resources.Load<GameObject>(name).GetComponent<SpriteRenderer>().sprite;
                    enemy.GetComponent<RectTransform>().rotation = Quaternion.identity;
                    p.EnemyToLoad.spawnHeight = -3f;
                    enemy.GetComponent<Animator>().runtimeAnimatorController = Resources.Load("Boss/Troll3UI") as RuntimeAnimatorController;
                    break;
                case 13:
                    name = "Boss/PredatorAlien";
                    p.EnemyToLoad = p.gameData.allEnemies.Find(x => x.ID.Equals(name));
                    enemy.GetComponent<Image>().sprite = Resources.Load<GameObject>(name).GetComponent<SpriteRenderer>().sprite;
                    enemy.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 180, 0);
                    p.EnemyToLoad.spawnHeight = -3f;
                    enemy.GetComponent<Animator>().runtimeAnimatorController = Resources.Load("Boss/PredatorAlienUI") as RuntimeAnimatorController;
                    break;
                default: //only called by tutorial
                    if (tutorialPhase == 8) StartTutorial();
                    enemy.GetComponent<Image>().sprite = Resources.Load<GameObject>("Boss/Orc1").GetComponent<SpriteRenderer>().sprite;
                    enemy.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 180, 0);
                    enemy.GetComponent<Animator>().runtimeAnimatorController = Resources.Load("Boss/Orc1UI") as RuntimeAnimatorController;
                    return;
            }
            //set middle text elements
            GameObject.Find("StageName").GetComponent<Text>().text = p.EnemyToLoad.Name + "\nLvL " + actualStage;
            string[] full = p.EnemyToLoad.Description.Split(',');
            for (int i = 0; i < full.Length; i++)
            {
                GameObject m = Resources.Load<GameObject>("Prefabs/MAMap/Description");
                Transform p = GameObject.Find("MiddleText").GetComponent<Transform>();
                GameObject middle = Instantiate(m, p);
                middle.GetComponent<Text>().text = full[i];
            }
            //set reward elements
            //Reward 1 always Gold
            GameObject R1 = Resources.Load<GameObject>("Prefabs/MAMap/RewardCell");
            Transform p1 = GameObject.Find("RewardList").GetComponent<Transform>();
            GameObject gold = Instantiate(R1, p1);                                           //default image is already gold symbol
            gold.GetComponentInChildren<Text>().text = p.EnemyToLoad.MaxGold.ToString();

            //Reward 2 XP
            GameObject xp = Instantiate(R1, p1);
            GameObject imageChild = xp.GetComponentInChildren<Image>().gameObject;      //get child gameobject that contains the image component
            GameObject textChild = xp.GetComponentInChildren<Text>().gameObject;
            Vector3 oldPos = imageChild.transform.position;
            Destroy(imageChild);                                  //destroy that image component...
            GameObject t = Instantiate(textChild, xp.transform);
            t.transform.position = oldPos;
            Text xpText = textChild.GetComponent<Text>();                             //..and add a text component for XP text
            xpText.text = "XP";
            xpText.fontSize = 100;
            xpText.color = new Color(255, 255, 0, 255);     //yellow
            //Item Rewards
            for (int i = 0; i < p.EnemyToLoad.loot.Length; i++)
            {
                Profile.GameData.Enemy.ItemAndChance ic = p.EnemyToLoad.loot[i];
                GameObject loot = Instantiate(R1, p1);
                loot.GetComponentInChildren<Text>().text = ic.amount.ToString();
                loot.GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>(ic.itemID);
            }
        }
    }

    public void closeStagePreview()
    {
        uiElementOn = false;
        //destroy all cells again in middletext and rewardslist
        Transform p = GameObject.Find("MiddleText").GetComponent<Transform>();
        Transform[] trans = p.gameObject.GetComponentsInChildren<Transform>();
        for(int i = 0; i < trans.Length; i++)
        {
            if (p != trans[i]) Destroy(trans[i].gameObject);
        }
        Transform p1 = GameObject.Find("RewardList").GetComponent<Transform>();
        trans = p1.gameObject.GetComponentsInChildren<Transform>();
        for (int i = 0; i < trans.Length; i++)
        {
            if (p1 != trans[i]) Destroy(trans[i].gameObject);
        }
        stagePreviewPanel.SetActive(false);
    }
    //called when clicking away from loop prompt panel
    public void CloseLoopPanel()
    {
        p.LoopPrompt = false;
        loopPanel.SetActive(false);
    }
    //called when the herausfordern button on stage preview panel is pressed
    public void loadFight()
    {
        if (!tutorialOn)
        {
            //currently all fights cost 5 AP
            if (p.playerData.ActivityPoints >= 5)
            {
                p.playerData.ActivityPoints -= 5;
                p.EnemyToLoad.stage = actualStage;
                SceneManager.LoadScene("MAFight");
            }
            else
            {
                //some message prompt "not enough AP, go do some sports"..
            }
        }
    }
    #endregion
    #region Inventory
    //called by the pressed item slot in gear panel
    public void setSelectedItem()
    {
        equipButton.SetActive(true);
        selectedItem = EventSystem.current.currentSelectedGameObject;
    }
    //equip the selected item if possible, otherwise discard/deselect
    public void confirmEquip()
    {
        if (selectedItem == null) return;
        //make sure selectedItem is an inventorySlot type
        if (selectedItem.name.Contains("Slot"))
        {
            //get slot number, its inventory item, check if it's equipable, if yes equip and switch the equiped item/sprite with that slot, deselect if not
            string s = selectedItem.name.Substring(4, selectedItem.name.Length - 4); //slotXX expected name
            int slotNumber = Convert.ToInt32(s) - 1;
            Profile.GameData.Item item = p.playerData.inventory[slotNumber];
            if (item == null) return; //inventory slot is empty and so *should* be the ui slot

            //0 active, 1 passive, 2 weapon, 3 usable, 4 none 
            if ((int)item.Type > 3)
            {
                //deselect
                selectedItem = null;
                return;
            }
            else
            {
                Image equipmentSlot = GameObject.Find(item.Type.ToString()).GetComponent<Image>();
                Image[] bla = selectedItem.GetComponentsInChildren<Image>();
                Image inventorySlot = bla[0].gameObject.name == "Item" ? bla[0] : bla[1];
                if(inventorySlot.sprite == null)
                {
                    //allow de-equip later
                    selectedItem = null;
                    return;
                }
                else
                {
                    Sprite inventorySprite = inventorySlot.sprite;
                    Sprite equipSprite = equipmentSlot.sprite;
                    if (equipSprite == null)
                    {
                        equipmentSlot.color = new Color(1, 1, 1, 1);
                        inventorySlot.color = new Color(1, 1, 1, 0);
                        selectedItem.GetComponentInChildren<Text>().text = "";
                    }
                    equipmentSlot.sprite = inventorySprite;
                    inventorySlot.sprite = equipSprite;

                    Profile.GameData.Item temp = p.playerData.equipment[(int)item.Type];
                    p.playerData.equipment[(int)item.Type] = item;
                    p.playerData.inventory[slotNumber] = temp;
                }
            }
        }
    }
    //sets all equipment and inventory of user once at Login
    private void initInventory()
    {
        inventoryPanel.SetActive(true);
        //deactivate tut avatar and set real one
        GameObject.Find("AvatarImageTut").SetActive(false);
        GameObject avatar = GameObject.Find("AvatarImage");
        avatar.AddComponent<Image>();
        avatar.GetComponent<Image>().sprite = AvatarToLoad.GetComponent<SpriteRenderer>().sprite;
        avatar.GetComponent<Image>().preserveAspect = true;
        //set name and level text
        Text[] texts = GameObject.Find("GearPanelTitel").GetComponentsInChildren<Text>();
        if (texts[0].name == "Playername")
        {
            texts[0].text = p.playerData.AvatarName;
            texts[1].text = "Lvl " + p.playerData.Level.ToString();
        }
        else
        {
            texts[1].text = p.playerData.AvatarName;
            texts[0].text = "Lvl " + p.playerData.Level.ToString();
        }
        //set inventory slots
        for (int i = 0; i < p.playerData.inventory.Length; i++)
        {
            if (p.playerData.inventory[i] != null)
            {
                Sprite s = Resources.Load<Sprite>(p.playerData.inventory[i].ID);
                GameObject slot = GameObject.Find("Slot" + (i + 1));

                Text t = slot.GetComponentInChildren<Text>();
                if (t)
                {
                    switch (p.playerData.inventory[i].Type)
                    {
                        case Profile.GameData.InventorySlots.Active:
                            t.text = "A";
                            break;
                        case Profile.GameData.InventorySlots.Passive:
                            t.text = "P";
                            break;
                        case Profile.GameData.InventorySlots.Usable:
                            t.text = "N";
                            break;
                        case Profile.GameData.InventorySlots.Weapon:
                            t.text = "W";
                            break;
                        default:
                            t.text = "";
                            break;
                    }
                }

                Image[] bla = slot.GetComponentsInChildren<Image>();
                Image meh = bla[0].gameObject.name == "Item" ? bla[0] : bla[1];
                meh.sprite = s;
                meh.color = new Color(1, 1, 1, 1);
            }
            else
            {
                GameObject slot = GameObject.Find("Slot" + (i + 1));
                Image[] bla = slot.GetComponentsInChildren<Image>();
                Image meh = bla[0].gameObject.name == "Item" ? bla[0] : bla[1];
                meh.sprite = null;
                meh.color = new Color(1, 1, 1, 0);
            }
        }
        //set equipment slots
        for(int i = 0; i < p.playerData.equipment.Length; i++)
        {
            Profile.GameData.Item item = p.playerData.equipment[i];
            Profile.GameData.InventorySlots slot = (Profile.GameData.InventorySlots)i;
            Image derp = GameObject.Find(slot.ToString()).GetComponent<Image>();
            if (item != null)
            {

                derp.sprite = Resources.Load<Sprite>(item.ID);
                derp.color = new Color(1, 1, 1, 1);
            }
            else
            {
                derp.sprite = null;
                derp.color = new Color(1, 1, 1, 0);
            } 
        }
        inventoryPanel.SetActive(false);
    }

    public void openGearPanel()
    {
        
        if (!tutorialOn)
        {
            uiElementOn = true;
            inventoryPanel.gameObject.SetActive(true);
        }
        else
        {
            if(tutorialPhase == 6)
            {
                uiElementOn = true;
                inventoryPanel.gameObject.SetActive(true);
                StartTutorial();
            }
        }
    }

    public void closeGearPanel()
    {
        uiElementOn = false;
        equipButton.SetActive(false);
        inventoryPanel.gameObject.SetActive(false);
    }
    #endregion
    #region Shop
    //called when a shop item is pressed
    public void ShopInspect()
    {
        //take the selected GameObject and take it's center text child-text
        selectedItem = EventSystem.current.currentSelectedGameObject;
        Text[] texts = selectedItem.GetComponentsInChildren<Text>();
        Text nameText = texts[0].gameObject.name == "Name" ? texts[0] : texts[1];
        //grab the item from GameData that fits the name
        Profile.GameData.Item result = p.gameData.allItems.Find(x => x.Name.Equals(nameText.text));
        
        //fill every info of that item into the inspection panel
        string stats = result.Name + "\n\n" + result.Description;
        string m_slot = "";
        switch (result.Type)
        {
            case Profile.GameData.InventorySlots.Active:
                m_slot = "Aktiv";
                break;
            case Profile.GameData.InventorySlots.Passive:
                m_slot = "Passiv";
                break;
            case Profile.GameData.InventorySlots.Usable:
                m_slot = "Nutzbar";
                break;
            case Profile.GameData.InventorySlots.Weapon:
                m_slot = "Waffe";
                break;
            default:
                break;
        }

        GameObject.Find("ItemSelectCost").GetComponent<Text>().text = m_slot;
        GameObject.Find("ItemSelectStats").GetComponent<Text>().text = stats;
        GameObject.Find("ItemSelectImage").GetComponent<Image>().sprite = Resources.Load<Sprite>(result.ID);
        GameObject.Find("ItemSelectImage").GetComponent<Image>().color = new Color(1, 1, 1, 1);
        //activate pay button
        payButton.SetActive(true);
    }
    //called when kaufen is clicked, checks if you have enough gold, if yes, puts the item into inventory(if enough space) and deducts the gold
    public void TryBuyItem()
    {
        Text[] texts = selectedItem.GetComponentsInChildren<Text>();
        int price;
        string title;

        if(texts[0].name == "Cost")
        {
            price = Convert.ToInt32(texts[0].text);
            title = texts[1].text;
        }
        else
        {
            price = Convert.ToInt32(texts[1].text);
            title = texts[0].text;
        }
        //if we can afford the item, find first free inventory slot in playerdata, deposit the item there and update the gear panel accordingly
        if (price <= p.playerData.Gold)
        {
            for(int i = 0; i < 40; i++)
            {
                if (p.playerData.inventory[i] == null)
                {
                    //adjust playerdata and gold, gold panel
                    p.playerData.Gold -= price;
                    Profile.GameData.Item item = p.gameData.allItems.Find(x => x.Name.Equals(title));
                    p.playerData.inventory[i] = item;
                    GameObject.Find("GoldPanelAmount").GetComponent<Text>().text = p.playerData.Gold.ToString();
                    //update inventoryUI
                    inventoryPanel.SetActive(true);
                    GameObject found = GameObject.Find("Slot" + (i + 1));
                    Image[] images = found.GetComponentsInChildren<Image>();
                    Image image = images[0].gameObject.name == "Item" ? images[0] : images[1];
                    image.sprite = Resources.Load<Sprite>(item.ID);
                    image.color = new Color(1, 1, 1, 1);
                    inventoryPanel.SetActive(false);
                    break;
                }
            }
        }
    }
    //called once at login
    void InitShop()
    {
        shopPanel.SetActive(true);
        //set players gold
        GameObject.Find("GoldPanelAmount").GetComponent<Text>().text = p.playerData.Gold.ToString();
        //clear the inspection cell
        GameObject.Find("ItemSelectStats").GetComponent<Text>().text = "";
        GameObject.Find("ItemSelectCost").GetComponent<Text>().text = "";
        Image item = GameObject.Find("ItemSelectImage").GetComponent<Image>();
        item.sprite = null;
        item.color = new Color(1, 1, 1, 0);
        //fill the 11 cells with items
        GameObject[] shopItemCells = GameObject.FindGameObjectsWithTag("shopItem");
        List<Profile.GameData.Item> itemListCopy = new List<Profile.GameData.Item>(p.gameData.allItems);
        int itemListSize = itemListCopy.Count;
        if( itemListSize > 0)
        {
            //get distinct items at random from item list
            for (int i = 0; i < shopItemCells.Length; i++)
            {
                int randomIndex = UnityEngine.Random.Range(0, itemListCopy.Count);
                //remove all "public display exclusive items, "Edelsteine" in this case"
                while (itemListCopy[randomIndex].Name.Contains("Edelstein"))
                {
                    itemListCopy.RemoveAt(randomIndex);
                    randomIndex = UnityEngine.Random.Range(0, itemListCopy.Count);
                }
                Profile.GameData.Item item1 = itemListCopy[randomIndex];
                //set the 2 texts
                Text[] texts = shopItemCells[i].GetComponentsInChildren<Text>();
                if (texts[0].gameObject.name == "Name")
                {
                    texts[0].text = item1.Name;
                    texts[1].text = item1.Cost.ToString();
                }
                else
                {
                    texts[1].text = item1.Name;
                    texts[0].text = item1.Cost.ToString();
                }
                //has 3 images because of parent
                Image[] images = shopItemCells[i].GetComponentsInChildren<Image>();
                for (int j = 0; j < images.Length; j++)
                {
                    if (images[j].gameObject.name == "Item")
                    {
                        images[j].sprite = Resources.Load<Sprite>(item1.ID);
                        images[j].color = new Color(1, 1, 1, 1);
                    }
                    if (images[j].gameObject.name == "GoldImage")
                    {
                        images[j].sprite = Resources.Load<Sprite>("Sprites/Item Icons/3");
                        images[j].color = new Color(1, 1, 1, 1);
                    }
                }
                itemListCopy.RemoveAt(randomIndex);
            }
        }

        shopPanel.SetActive(false);
    }

    public void openShopPanel()
    {
        if (!tutorialOn)
        {
            uiElementOn = true;
            shopPanel.SetActive(true);
        }
        else
        {
            if(tutorialPhase == 8)
            {
                uiElementOn = true;
                shopPanel.SetActive(true);
                StartTutorial();
            }
        }
    }

    public void closeShopPanel()
    {
        uiElementOn = false;
        payButton.SetActive(false);
        shopPanel.gameObject.SetActive(false);
    }
    #endregion
    #region Char Panel
   //called once at the start to init all text
    private void initCharPanel()
    {
        //normal stats (white)
        float strength = p.playerData.stats["Strength"] * p.playerData.ActivityStreak;
        float dex = p.playerData.stats["Dexterity"] * p.playerData.ActivityStreak;
        float intl = p.playerData.stats["Intelligence"] * p.playerData.ActivityStreak;
        float vit = p.playerData.stats["Vitality"] * p.playerData.ActivityStreak;

        float minDmg = p.playerData.stats["MinDmg"] * p.playerData.stats["DamageM"] * p.playerData.ActivityStreak;
        float maxDmg = p.playerData.stats["MaxDmg"] * p.playerData.stats["DamageM"] * p.playerData.ActivityStreak;
        float minMDmg = p.playerData.stats["MinMDmg"] * p.playerData.stats["DamageM"] * p.playerData.ActivityStreak;
        float maxMDmg = p.playerData.stats["MaxMDmg"] * p.playerData.stats["DamageM"] * p.playerData.ActivityStreak;
        float physRes = p.playerData.stats["PhysicalDefense"] * p.playerData.stats["PhysicalDefenseM"] * p.playerData.ActivityStreak;
        float magRes = p.playerData.stats["MagicDefense"] * p.playerData.stats["MagicDefenseM"] * p.playerData.ActivityStreak;
        float atk = p.playerData.stats["AttackSpeed"] / Mathf.Pow(1.01f, p.playerData.ActivityStreak);
        float end = p.playerData.stats["Endurance"] * p.playerData.ActivityStreak;
        float dodge = p.playerData.stats["DodgeChance"] * p.playerData.ActivityStreak;
        float crit = p.playerData.stats["CritChance"] * p.playerData.ActivityStreak;
        float recovery = p.playerData.stats["Recovery"] * p.playerData.ActivityStreak;
        float hp = p.playerData.stats["Health"] * p.playerData.stats["HealthM"] * p.playerData.ActivityStreak;

        //set all stats in the panel
        charPanel.SetActive(true);
        
        //deactivate tutorial avatar and set the real one
        GameObject.Find("CharAvatarImageTut").SetActive(false);
        GameObject avatar = GameObject.Find("CharAvatarImage");
        avatar.GetComponent<Image>().sprite = AvatarToLoad.GetComponent<SpriteRenderer>().sprite;
        avatar.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        avatar.GetComponent<Image>().preserveAspect = true;
        //set all text
        GameObject.Find("CharPlayername").GetComponent<Text>().text = p.playerData.AvatarName;
        GameObject.Find("CharPlayerlevel").GetComponent<Text>().text = "Lvl " + p.playerData.Level.ToString();
        
        int totalXP = 0;
        for(int i = 1; i <=p.playerData.Level; i++)
        {
            totalXP += i * 75;
        }
        int curXP = p.playerData.CurExperiencePoints;
        GameObject.Find("CharXPPoints").GetComponent<Text>().text = curXP + "/" + totalXP;
        float scale = (float)curXP / (float)totalXP;
        GameObject.Find("CharXPFG").GetComponent<RectTransform>().localScale = new Vector3(scale,1,1);

        GameObject.Find("StrNum").GetComponent<Text>().text = strength.ToString();
        GameObject.Find("IntNum").GetComponent<Text>().text = intl.ToString();
        GameObject.Find("DexNum").GetComponent<Text>().text = dex.ToString();
        GameObject.Find("VitNum").GetComponent<Text>().text = vit.ToString();

        GameObject.Find("physDNum").GetComponent<Text>().text = minDmg.ToString() + " - " + maxDmg.ToString();
        GameObject.Find("magDNum").GetComponent<Text>().text = minMDmg.ToString() + " - " + maxMDmg.ToString();
        GameObject.Find("physRNum").GetComponent<Text>().text = physRes.ToString();
        GameObject.Find("magRNum").GetComponent<Text>().text = magRes.ToString();
        GameObject.Find("AtkSpdNum").GetComponent<Text>().text = atk.ToString("F2") + " Sek";
        GameObject.Find("EndNum").GetComponent<Text>().text = end.ToString();
        GameObject.Find("DodGNum").GetComponent<Text>().text = dodge.ToString() + " %";
        GameObject.Find("critNum").GetComponent<Text>().text = crit.ToString() + " %";
        GameObject.Find("recNum").GetComponent<Text>().text = recovery.ToString() + "/Sek";
        GameObject.Find("hpNum").GetComponent<Text>().text = hp.ToString();

        if(p.playerData.FreeStatPointsLeft > 0)
        {
            StatText.SetActive(true);
            StatTextNum.SetActive(true);
            StatTextNum.GetComponent<Text>().text = p.playerData.FreeStatPointsLeft.ToString();
            Str.SetActive(true);
            Dex.SetActive(true);
            Int.SetActive(true);
            Vit.SetActive(true);
        }
        else
        {
            StatText.SetActive(false);
            StatTextNum.SetActive(false);
            Str.SetActive(false);
            Dex.SetActive(false);
            Int.SetActive(false);
            Vit.SetActive(false);
        }
        charPanel.SetActive(false);
    }
    //called when the corresponding stat button is pressed in char panel
    public void increaseStat(int statNum)
    {
        if (p.playerData.FreeStatPointsLeft == 0) return;
        p.playerData.FreeStatPointsLeft--;

        if (p.playerData.FreeStatPointsLeft == 0)
        {
            StatText.SetActive(false);
            StatTextNum.SetActive(false);
        }
        else
        {
            StatTextNum.GetComponent<Text>().text = p.playerData.FreeStatPointsLeft.ToString();
        }

        switch (statNum)
        {
            case 1:
                p.playerData.stats["Strength"]++;
                p.playerData.stats["MinDmg"]+=1;
                p.playerData.stats["MaxDmg"]+=6;
                p.playerData.stats["PhysicalDefense"]+= 1;
                float strength = p.playerData.stats["Strength"] * p.playerData.ActivityStreak;
                GameObject.Find("StrNum").GetComponent<Text>().text = strength.ToString("n1");
                GameObject.Find("StrNum").GetComponent<Text>().color = new Color(0, 1, 0, 1);
                float minDmg = p.playerData.stats["MinDmg"] * p.playerData.stats["DamageM"] * p.playerData.ActivityStreak;
                float maxDmg = p.playerData.stats["MaxDmg"] * p.playerData.stats["DamageM"] * p.playerData.ActivityStreak;
                GameObject.Find("physDNum").GetComponent<Text>().text = minDmg.ToString("n1") + " - " + maxDmg.ToString("n1");
                GameObject.Find("physDNum").GetComponent<Text>().color = new Color(0, 1, 0, 1);
                float physRes = p.playerData.stats["PhysicalDefense"] * p.playerData.stats["PhysicalDefenseM"] * p.playerData.ActivityStreak;
                GameObject.Find("physRNum").GetComponent<Text>().text = physRes.ToString("n1");
                GameObject.Find("physRNum").GetComponent<Text>().color = new Color(0, 1, 0, 1);
                break;
            case 2:
                p.playerData.stats["Dexterity"]++;
                p.playerData.stats["DodgeChance"] += 0.5f;
                p.playerData.stats["CritChance"] += 0.5f;
                p.playerData.stats["AttackSpeed"] /= 1.01f;
                float dex = p.playerData.stats["Dexterity"] * p.playerData.ActivityStreak;
                GameObject.Find("DexNum").GetComponent<Text>().text = dex.ToString("n1");
                GameObject.Find("DexNum").GetComponent<Text>().color = new Color(0, 1, 0, 1);
                float atk = p.playerData.stats["AttackSpeed"] / Mathf.Pow(1.01f, p.playerData.ActivityStreak);
                float dodge = p.playerData.stats["DodgeChance"] * p.playerData.ActivityStreak;
                float crit = p.playerData.stats["CritChance"] * p.playerData.ActivityStreak;
                GameObject.Find("AtkSpdNum").GetComponent<Text>().text = atk.ToString("n1");
                GameObject.Find("DodGNum").GetComponent<Text>().text = dodge.ToString("n1");
                GameObject.Find("critNum").GetComponent<Text>().text = crit.ToString("n1");
                GameObject.Find("AtkSpdNum").GetComponent<Text>().color = new Color(0, 1, 0, 1);
                GameObject.Find("DodGNum").GetComponent<Text>().color = new Color(0, 1, 0, 1);
                GameObject.Find("critNum").GetComponent<Text>().color = new Color(0, 1, 0, 1);
                break;
            case 3:
                p.playerData.stats["Intelligence"]++;
                p.playerData.stats["MinMDmg"] += 1;
                p.playerData.stats["MaxMDmg"] += 6;
                p.playerData.stats["MagicDefense"] += 1;
                float intl = p.playerData.stats["Intelligence"] * p.playerData.ActivityStreak;
                GameObject.Find("IntNum").GetComponent<Text>().text = intl.ToString("n1");
                GameObject.Find("IntNum").GetComponent<Text>().color = new Color(0, 1, 0, 1);
                float minMDmg = p.playerData.stats["MinMDmg"] * p.playerData.stats["DamageM"] * p.playerData.ActivityStreak;
                float maxMDmg = p.playerData.stats["MaxMDmg"] * p.playerData.stats["DamageM"] * p.playerData.ActivityStreak;
                GameObject.Find("magDNum").GetComponent<Text>().text = minMDmg.ToString("n1") + " - " + maxMDmg.ToString("n1");
                GameObject.Find("magDNum").GetComponent<Text>().color = new Color(0, 1, 0, 1);
                float magRes = p.playerData.stats["MagicDefense"] * p.playerData.stats["MagicDefenseM"] * p.playerData.ActivityStreak;
                GameObject.Find("magRNum").GetComponent<Text>().text = magRes.ToString("n1");
                GameObject.Find("magRNum").GetComponent<Text>().color = new Color(0, 1, 0, 1);
                break;
            case 4:
                p.playerData.stats["Vitality"]++;
                p.playerData.stats["Endurance"]++;
                p.playerData.stats["Recovery"] += 0.1f;
                p.playerData.stats["Health"] += 10;
                float vit = p.playerData.stats["Vitality"] * p.playerData.ActivityStreak;
                GameObject.Find("VitNum").GetComponent<Text>().text = vit.ToString("n1");
                GameObject.Find("VitNum").GetComponent<Text>().color = new Color(0, 1, 0, 1);
                float end = p.playerData.stats["Endurance"] * p.playerData.ActivityStreak;
                float recovery = p.playerData.stats["Recovery"] * p.playerData.ActivityStreak;
                float hp = p.playerData.stats["Health"] * p.playerData.stats["HealthM"] * p.playerData.ActivityStreak;
                GameObject.Find("EndNum").GetComponent<Text>().text = end.ToString("n1");
                GameObject.Find("recNum").GetComponent<Text>().text = recovery.ToString("n1");
                GameObject.Find("hpNum").GetComponent<Text>().text = hp.ToString("n1");
                GameObject.Find("EndNum").GetComponent<Text>().color = new Color(0, 1, 0, 1);
                GameObject.Find("recNum").GetComponent<Text>().color = new Color(0, 1, 0, 1);
                GameObject.Find("hpNum").GetComponent<Text>().color = new Color(0, 1, 0, 1);
                break;
            default: break;
        }
    }
    //called when avatar image is pressed on map
    public void openCharPanel()
    {
        if (!tutorialOn)
        {
            uiElementOn = true;
            charPanel.gameObject.SetActive(true);

            if (p.playerData.FreeStatPointsLeft > 0)
            {
                StatText.SetActive(true);
                StatTextNum.SetActive(true);
                StatTextNum.GetComponent<Text>().text = p.playerData.FreeStatPointsLeft.ToString();
            }
        }
        else
        {
            if(tutorialPhase == 3)
            {
                uiElementOn = true;
                charPanel.gameObject.SetActive(true);
                StartTutorial();
            }
        }
    }

    public void closeCharPanel()
    {
        uiElementOn = false;
        //set all text back to white color
        GameObject.Find("StrNum").GetComponent<Text>().color = new Color(1, 1, 1, 1);
        GameObject.Find("DexNum").GetComponent<Text>().color = new Color(1, 1, 1, 1);
        GameObject.Find("IntNum").GetComponent<Text>().color = new Color(1, 1, 1, 1);
        GameObject.Find("VitNum").GetComponent<Text>().color = new Color(1, 1, 1, 1);
        GameObject.Find("physDNum").GetComponent<Text>().color = new Color(1, 1, 1, 1);
        GameObject.Find("magDNum").GetComponent<Text>().color = new Color(1, 1, 1, 1);
        GameObject.Find("physRNum").GetComponent<Text>().color = new Color(1, 1, 1, 1);
        GameObject.Find("magRNum").GetComponent<Text>().color = new Color(1, 1, 1, 1);
        GameObject.Find("AtkSpdNum").GetComponent<Text>().color = new Color(1, 1, 1, 1);
        GameObject.Find("EndNum").GetComponent<Text>().color = new Color(1, 1, 1, 1);
        GameObject.Find("DodGNum").GetComponent<Text>().color = new Color(1, 1, 1, 1);
        GameObject.Find("critNum").GetComponent<Text>().color = new Color(1, 1, 1, 1);
        GameObject.Find("recNum").GetComponent<Text>().color = new Color(1, 1, 1, 1);
        GameObject.Find("hpNum").GetComponent<Text>().color = new Color(1, 1, 1, 1);
        charPanel.gameObject.SetActive(false);
    }
    #endregion
}

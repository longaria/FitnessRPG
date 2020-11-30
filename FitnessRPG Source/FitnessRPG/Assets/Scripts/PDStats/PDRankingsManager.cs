using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PDRankingsManager : MonoBehaviour {

    public Text clock, schedule;
    public GameObject LevelPanel, BSBPanel;
    private GameObject scrollArea;
    private float scrollCD, bottomDummy;
    private bool scroll, reachedBottom, rankingsloaded;
    private PDProfile profile;
    private Profile.PlayerData[] sortedplayers;
    private List<Profile.PlayerData> li;
    public GameObject rankingCell;

    class Level : IComparer<Profile.PlayerData>
    {
        public int Compare(Profile.PlayerData x, Profile.PlayerData y)
        {
            if (x == null || y == null)
            {
                return 0;
            }

            // CompareTo() method 
            return x.Level.CompareTo(y.Level);
        }
    }

    class BSB : IComparer<Profile.PlayerData>
    {
        public int Compare(Profile.PlayerData x, Profile.PlayerData y)
        {
            if (x == null || y == null)
            {
                return 0;
            }

            // CompareTo() method 
            return x.bestStageBeaten.CompareTo(y.bestStageBeaten);
        }
    }

    void LoadPDFight()
    {
        SceneManager.LoadScene("PDFight");
    }
    //updates every minute: the system clock text, the next fight text (or loads the fight, when time runs out)
    void UpdateClock()
    {
        TimeSpan t = profile.nextCourseID == "" ? new TimeSpan(0,1,0) : profile.nextCourse - DateTime.Now;
        clock.text = DateTime.Now.ToString("H:mm") + " H";
        schedule.text = string.Format("nächster Kampf in: {0}h {1}m", t.Hours, t.Minutes);

        if(t.Minutes <= 0)
        {
            //LoadPDFight();
        }
        else
        {
            Invoke("UpdateClock", 60);
        }    
    }
    //switch rankings every certain period
    void SwitchRankings()
    {
        //reset scroll position before switchting scroll areas
        scrollArea.GetComponent<ScrollRect>().verticalNormalizedPosition = 1;

        if (LevelPanel.activeSelf)
        {
            LevelPanel.SetActive(false);
            BSBPanel.SetActive(true);
        }
        else
        {
            BSBPanel.SetActive(false);
            LevelPanel.SetActive(true);            
        }
        scrollArea = GameObject.Find("ScrollArea");
        scrollCD = 5;
        reachedBottom = false;
        scroll = false;
    }

    void ScrollRankings()
    {
        if (reachedBottom)
        {
            Invoke("SwitchRankings", 3f);
        }
        else
        {
            //scroll and check if reachedBottom needs to be set
            ScrollRect r = scrollArea.GetComponent<ScrollRect>();
            //in my case my vertical bar starts at pos (1,1) scrolls down to (1,0)
            if(r.normalizedPosition.y > 0)
            {
                float y = r.normalizedPosition.y - (Time.deltaTime / 20);
                r.normalizedPosition = new Vector2(r.normalizedPosition.x, y);
            }
            else
            {
                reachedBottom = true;
            }
            Invoke("ScrollRankings", Time.deltaTime);
        }        
    }

    void Init()
    {
        //put all players into a list to sort
        li = new List<Profile.PlayerData>();
        foreach (KeyValuePair<string, Profile.PlayerData> k in profile.playerlist)
        {
            li.Add(profile.playerlist[k.Key]);
        }
        //init arrays with 100 length max, assuming the ranking list handles 100 cells - test with 10 first maybe
        sortedplayers = new Profile.PlayerData[Mathf.Min(li.Count, 100)];
    }

    void SortByLevel()
    {
        //sort by level and put into level array
        Level l = new Level();
        li.Sort(l);
        int i = 0;
        foreach (Profile.PlayerData p in li)
        {
            sortedplayers[i] = p;
            i++;
        }
        li.Reverse();
    }

    void SortByBSB()
    {
        //sort by bestStageBeaten and put into bsb array
        BSB b = new BSB();
        li.Sort(b);
        int i = 0;
        foreach (Profile.PlayerData p in li)
        {
            sortedplayers[i] = p;
            i++;
        }
        li.Reverse();
    }

    void LoadRankingCells()
    {
        Transform t = GameObject.Find("List").GetComponent<Transform>();
        int i = 1;
        foreach (Profile.PlayerData player in li)
        {
            GameObject cell = Instantiate(rankingCell, t);
            Text[] texts = cell.GetComponentsInChildren<Text>();
            foreach (Text text in texts)
            {
                if (text.gameObject.name == "Rank") text.text = i.ToString();

                if (text.gameObject.name == "Score")
                {
                        text.text = LevelPanel.activeSelf ? "LvL " + player.Level.ToString() : "Stufe " + player.bestStageBeaten.ToString();
                } 
                if (text.gameObject.name == "Playername") text.text = player.AvatarName;
            }
            Image[] images = cell.GetComponentsInChildren<Image>();
            foreach (Image image in images)
            {
                if (image.gameObject.name == "Medal1")
                {
                    if (player.Badge1)
                    {
                        image.sprite = Resources.Load<Sprite>("Sprites/Item Icons/7");
                        image.color = new Color(1, 1, 1, 1);
                    }
                    else
                    {
                        image.sprite = null;
                        image.color = new Color(1, 1, 1, 0);
                    }
                    
                } 
                if (image.gameObject.name == "Medal2")
                {
                    if (player.Badge2)
                    {
                        image.sprite = Resources.Load<Sprite>("Sprites/Item Icons/7");
                        image.color = new Color(1, 1, 1, 1);
                    }
                    else
                    {
                        image.sprite = null;
                        image.color = new Color(1, 1, 1, 0);
                    }

                }
                if (image.gameObject.name == "Medal3")
                {
                    if (player.Badge3)
                    {
                        image.sprite = Resources.Load<Sprite>("Sprites/Item Icons/7");
                        image.color = new Color(1, 1, 1, 1);
                    }
                    else
                    {
                        image.sprite = null;
                        image.color = new Color(1, 1, 1, 0);
                    }

                }
                if (image.gameObject.name == "AvatarImage")
                {
                    if (player.AvatarID != "")
                    {
                        image.sprite = Resources.Load<GameObject>("Avatars/" + player.AvatarID).GetComponent<SpriteRenderer>().sprite;
                    }
                    else
                    {
                        image.sprite = null;
                    }
                }
            }
            i++;
        }
    }
    //activates both panels and fills all cells ONCE - should not change until next PDfight
    void SetAllRankings()
    {
        //load Level Ranking Cells
        SortByLevel();
        LevelPanel.SetActive(true);
        LoadRankingCells();
        LevelPanel.SetActive(false);
        //best beaten stage cells
        SortByBSB();
        BSBPanel.SetActive(true);
        LoadRankingCells();
        BSBPanel.SetActive(false);

        LevelPanel.SetActive(true);
        scrollArea = GameObject.Find("ScrollArea");
        rankingsloaded = true;
    }

    // Use this for initialization
    void Start ()
    {
        profile = GameObject.Find("PDProfile").GetComponent<PDProfile>();
        scroll = rankingsloaded = reachedBottom = false;
        scrollCD = 5;
        Init();
        SetAllRankings();
        UpdateClock();
    }

    void Update()
    {
        if (!scroll && rankingsloaded)
        {
            scrollCD -= Time.deltaTime;

            if (scrollCD <= 0)
            {
                scroll = true;
                ScrollRankings();
            }
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            LoadPDFight();
        }
    }
}

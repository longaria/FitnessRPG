using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class PDProfile : MonoBehaviour
{
    public static PDProfile MyProfile;
    public Profile p;
    public bool ping, courseLoaded, playersloaded;
    public string courses, participants, allPlayers;
    public DateTime nextCourse;
    public string nextCourseID;
    public Dictionary<string, Profile.PlayerData> playerlist, participationlist;

    public void InitURLs()
    {
        StreamReader reader = new StreamReader(Application.dataPath + "/config.txt");
        string allText = reader.ReadToEnd();

        string[] all = allText.Split('\n');
        string[] res = all[7].Split('\t');
        courses = res[1].Substring(0, res[1].Length - 1);       //to get rid of \r at the end

        res = all[8].Split('\t');
        participants = res[1].Substring(0, res[1].Length - 1);

        res = all[9].Split('\t');
        allPlayers = res[1];
    }

    IEnumerator GetCourseID()
    {
        string courseID = "";
        DateTime result;
        //get the nearest courseID first from course list
        using (UnityWebRequest www = UnityWebRequest.Get(courses))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                Debug.Log(www.downloadHandler.text);
            }
            else
            {
                Debug.Log("schedule success");
                Debug.Log(www.downloadHandler.text);
                string all = www.downloadHandler.text;
                if (all == "")
                {
                    courseLoaded = true;
                    if (GameObject.Find("Connecting")) GameObject.Find("Connecting").SetActive(false);
                    yield break;
                }
                string[] rows = all.Split('\n');
                //looking for closest time to now (after)
                int i = 0;
                DateTime start = DateTime.Now;
                result = start;

                //while (i < rows.Length - 1)
                //{
                //    string[] elements = rows[i].Split('\t');
                //    DateTime d = DateTime.Parse(elements[1]);

                //    if (DateTime.Compare(d, start) > 0)            //must be later than now
                //    {
                //        if (result == start)      //if result still set to start, d is the first element later than now, so assign it
                //        {
                //            result = d;
                //            courseID = elements[0];
                //        }
                //        else if (DateTime.Compare(d, result) < 0)        //otherwise result is already set to a later date, so look for closest
                //        {
                //            result = d;
                //            courseID = elements[0];
                //        }
                //    }
                //    i++;
                //}
                //if (result != start)
                //{
                //    nextCourse = result;       //saving the DateTime of the next Course for future settings
                //    nextCourseID = courseID;
                //}

                string[] elements = rows[i].Split('\t');
                nextCourse = DateTime.Now;
                nextCourseID = elements[0];
                StartCoroutine(GetCourseParticipants(nextCourseID));
            }
        }
    }

    IEnumerator GetCourseParticipants(string courseID)
    {
        if (GameObject.Find("Connecting")) GameObject.Find("Connecting").SetActive(false);
        if (courseID == "")
        {
            courseLoaded = true;
            yield break;
        } 

        WWWForm form = new WWWForm();
        form.AddField("courseID", courseID);

        using (UnityWebRequest www = UnityWebRequest.Post(participants, form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                Debug.Log(www.downloadHandler.text);
            }
            else
            {
                Debug.Log("participation list success");
                participationlist = new Dictionary<string, Profile.PlayerData>();       //doing this resets the list everytime
                string all = www.downloadHandler.text;
                if (all == "") yield break;
                string[] rows = all.Split('\n');
                int i = 0;
                while(i < rows.Length - 1)
                {
                    string[] elements = rows[i].Split('\t');
                    participationlist.Add(elements[1], playerlist[elements[1]]);
                    i++;
                }
                courseLoaded = true;
            }
        }
    }
    //loads all players into a dictionary to display rankings or load paricipants later
    public IEnumerator LoadPlayers()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(allPlayers))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                Debug.Log(www.downloadHandler.text);
            }
            else
            {
                Debug.Log("Loading all players complete");
                string result = www.downloadHandler.text;
                string[] rows = result.Split('\n');
                for (int k = 0; k < rows.Length - 1; k++)
                {
                    string[] elements = rows[k].Split('\t');
                    Profile.PlayerData playerData = new Profile.PlayerData();

                    playerData.AvatarID = elements[1];
                    playerData.AvatarName = elements[0];
                    playerData.FirstLogin = int.Parse(elements[2]) == 1 ? true : false;

                    playerData.Badge1 = int.Parse(elements[3]) == 1 ? true : false;
                    playerData.Badge2 = int.Parse(elements[4]) == 1 ? true : false;
                    playerData.Badge3 = int.Parse(elements[5]) == 1 ? true : false;

                    playerData.ActivityStreak = float.Parse(elements[6]);
                    playerData.ActivityPoints = int.Parse(elements[7]);
                    playerData.ActivityPointsGained = int.Parse(elements[8]);

                    playerData.Level = int.Parse(elements[9]);
                    playerData.CurExperiencePoints = int.Parse(elements[10]);

                    playerData.FreeStatPointsLeft = int.Parse(elements[11]);
                    playerData.Gold = int.Parse(elements[12]);
                    playerData.bestStageBeaten = int.Parse(elements[13]);

                    //stage array process
                    string a = elements[14];
                    playerData.BeatenStages = new int[14];
                    if (a == "")
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
                    string b = elements[15];
                    playerData.inventory = new Profile.GameData.Item[20];

                    if (b == "")
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
                                Profile.GameData.Item j = p.gameData.allItems.Find(x => x.Name.Equals(invResult[i]));
                                playerData.inventory[i] = j;
                            }
                            else
                            {
                                playerData.inventory[i] = null;
                            }
                        }
                    }

                    //equipment
                    string c = elements[16];
                    playerData.equipment = new Profile.GameData.Item[4];

                    if (c == "")
                    {
                        for (int i = 0; i < playerData.equipment.Length; i++)
                        {
                            playerData.equipment[i] = null;
                        }
                    }
                    else
                    {
                        string[] eqResult = b.Split(',');
                        for (int i = 0; i < playerData.equipment.Length; i++)
                        {
                            if (eqResult[i] != "NULL")
                            {
                                Profile.GameData.Item j = p.gameData.allItems.Find(x => x.Name.Equals(eqResult[i]));
                                playerData.equipment[i] = j;
                            }
                            else
                            {
                                playerData.equipment[i] = null;
                            }
                        }
                    }
                    //stats
                    string dictionary = elements[17];
                    string[] res = dictionary.Split(',');

                    for (int i = 0; i < res.Length - 1; i += 2)
                    {
                        playerData.stats.Add(res[i], float.Parse(res[i + 1]));
                    }
                    playerlist.Add(playerData.AvatarName, playerData);
                }
                playersloaded = true;
            }
        }
    }
    //called from outer scripts after a PD fight is over, to get a new course etc.
    public void pingAgain()
    {
        ping = true;
    }

    void Awake()
    {
        if (MyProfile == null)
        {
            DontDestroyOnLoad(gameObject);
            MyProfile = this;
        }
        else if (MyProfile != this)
        {
            Destroy(gameObject);
        }
        p = GetComponent<Profile>();
    }
    // Start is called before the first frame update
    void Start()
    {
        ping = true;
        playersloaded = courseLoaded = false;
        playerlist = new Dictionary<string, Profile.PlayerData>();
        participationlist = new Dictionary<string, Profile.PlayerData>();
    }

    // Update is called once per frame
    void Update()
    {
        //wait for LoadGameData to finish
        if (playersloaded)
        {
            //ask server for schedule etc.
            if (ping)
            {                
                ping = false;
                StartCoroutine(GetCourseID());
            }
            if (courseLoaded)
            {
                courseLoaded = false;
                GameObject script = GameObject.FindGameObjectWithTag("myManager");
                if(script.GetComponent<PDManager>()) script.GetComponent<PDManager>().enabled = true;
                if(script.GetComponent<PDRankingsManager>()) script.GetComponent<PDRankingsManager>().enabled = true;
            }
        }
    }
}

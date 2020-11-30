using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.IO;
using UnityEngine.Networking;
using System.Collections.Generic;

// Login screen and title manager

public class TitleManager: MonoBehaviour {

	private EventSystem myEventSystem;
    private Profile p;
    private FileStream Stream;

    public Image loadbar, loadbarBG;
	public Text title, press, welcome, loading;
	public InputField field1, field2, rfield1, rfield2;
    public GameObject StartButtons, RegPanel, LogPanel, ErrorR, ErrorL;

    private bool wait;	
    private int errorCount;

    void Awake()
    {
		myEventSystem = EventSystem.current;
		p = GameObject.FindGameObjectWithTag ("myProfile").GetComponent<Profile> ();
    }

    void Start()
    {
        wait = true;
        errorCount = 0;
        StartCoroutine(MoveBigTitleIn());
    }

    void Update () {

        if (!wait)
        {
            if (Input.anyKeyDown || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)) //includes mouse button keys
            {
                wait = true;
                press.gameObject.SetActive(false);
                StartButtons.SetActive(true); 
            }
        }
	}
    //called when Anmelden or Registrieren is clicked
    public void ClickedStartButton(bool isLogin)
    {
        //in any case deactivate start buttons and 
        StartButtons.SetActive(false);
        if (isLogin)
        {
            //activate InputName Field and set focus on it
            LogPanel.SetActive(true);
            myEventSystem.SetSelectedGameObject(field1.gameObject);
        }
        else
        {
            RegPanel.SetActive(true);
            myEventSystem.SetSelectedGameObject(rfield1.gameObject);
            //possibly need to set input fields for touch later...
        }
    } 

	public void WaitOnce(){
		
		welcome.gameObject.SetActive (false);
		loadbar.gameObject.SetActive (true);
		loadbarBG.gameObject.SetActive (true);
		loading.gameObject.SetActive (true);

		StartCoroutine (LoadEffect ());
	}
    //called when Confirmation buttons are clicked on Registrieren or Anmelden 
    public void CallLogReg(bool isRegister)
    {
        if (isRegister)
        {
            StartCoroutine(RegisterPlayer());
        }
        else
        {
            StartCoroutine(LoginPlayer());
        }
    }
    //makes button interactable given the conditions, called on InputField change
    public void VerifyInput(Button b)
    {
        if(b.gameObject.name == "ConfirmL")
        {
            b.interactable = (field1.text.Length >= 3 && field2.text.Length >= 3);
        }
        else
        {
            b.interactable = (rfield1.text.Length >= 3 && rfield2.text.Length >= 3);
        }
    }

    //checks the gym data base for existing member first, then tries to register the user in our data base
    IEnumerator RegisterPlayer()
    {
        WWWForm form = new WWWForm();
        form.AddField("name", rfield1.text);
        form.AddField("password", rfield2.text);
        //----------------------------------------USABILITY TESTCODE-----------Register without check

        rfield1.text = "";
        rfield2.text = "";
        GameObject.Find("ConfirmR").GetComponent<Button>().interactable = false;

        using (UnityWebRequest www1 = UnityWebRequest.Post(p.reg, form))
        {
            yield return www1.SendWebRequest();

            if (www1.isNetworkError || www1.isHttpError || www1.downloadHandler.text != "0")
            {
                Debug.Log(www1.error);
                ErrorR.GetComponentInChildren<Text>().text = "Keine Gym Mitgliedschaft gefunden!";
                //        ErrorR.GetComponentInChildren<Text>().color = new Color(1, 0, 0, 1);
                //        ErrorR.gameObject.SetActive(true);
            }
            else
            {
                Debug.Log("registry success");
                Debug.Log(www1.downloadHandler.text);
                //show success message in white color, go back to start screen after 2 seconds
                ErrorR.GetComponentInChildren<Text>().text = "Registrierung erfolgreich!";
                ErrorR.GetComponentInChildren<Text>().color = new Color(1, 1, 1, 1);
                ErrorR.gameObject.SetActive(true);
                yield return new WaitForSeconds(3.0f);
                ErrorR.gameObject.SetActive(false);
                RegPanel.gameObject.SetActive(false);
                StartButtons.gameObject.SetActive(true);
            }
        }
        //-----------------------------------------------------------------

        //using (UnityWebRequest www = UnityWebRequest.Post(p.checkGym, form))
        //{
        //    yield return www.SendWebRequest();
        //    //in any case, clear input in register input fields and make button non-interactable again
        //    rfield1.text = "";
        //    rfield2.text = "";
        //    GameObject.Find("ConfirmR").GetComponent<Button>().interactable = false;

        //    if (www.isNetworkError || www.isHttpError || www.downloadHandler.text != "0")
        //    {
        //        //need to read error and make if statement to write right error message
        //        Debug.Log(www.error);
        //        Debug.Log(www.downloadHandler.text);
        //        //show error message, set text, set color red
        //        ErrorR.GetComponentInChildren<Text>().text = "Keine Gym Mitgliedschaft gefunden!";
        //        ErrorR.GetComponentInChildren<Text>().color = new Color(1, 0, 0, 1);
        //        ErrorR.gameObject.SetActive(true);
        //    }
        //    else
        //    {
        //        Debug.Log("gym check success");
        //        Debug.Log(www.downloadHandler.text);
        //        //gym check went through, now check that user doesnt exist in our data base and init it
        //        using (UnityWebRequest www1 = UnityWebRequest.Post(p.reg, form))
        //        {
        //            yield return www1.SendWebRequest();

        //            if (www1.isNetworkError || www1.isHttpError ||www1.downloadHandler.text != "0")
        //            {
        //                //need to read error and make if statement to write right error message
        //                Debug.Log(www1.error);
        //                Debug.Log(www1.downloadHandler.text);
        //                //show error message, set text, set color red
        //                ErrorR.GetComponentInChildren<Text>().text = "Login oder Passwort falsch.\nErneut versuchen!";
        //                ErrorR.GetComponentInChildren<Text>().color = new Color(1, 0, 0, 1);
        //                ErrorR.gameObject.SetActive(true);
        //            }
        //            else
        //            {
        //                Debug.Log("registry success");
        //                Debug.Log(www1.downloadHandler.text);
        //                //show success message in white color, go back to start screen after 2 seconds
        //                ErrorR.GetComponentInChildren<Text>().text = "Registrierung erfolgreich!";
        //                ErrorR.GetComponentInChildren<Text>().color = new Color(1, 1, 1, 1);
        //                ErrorR.gameObject.SetActive(true);
        //                yield return new WaitForSeconds(3.0f);
        //                ErrorR.gameObject.SetActive(false);
        //                RegPanel.gameObject.SetActive(false);
        //                StartButtons.gameObject.SetActive(true);
        //            }
        //        }
        //    }
        //}
    }
    //logs the player
    IEnumerator LoginPlayer()
    {
        WWWForm form = new WWWForm();
        form.AddField("name", field1.text);
        form.AddField("password", field2.text);

        using (UnityWebRequest www = UnityWebRequest.Post(p.log, form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError || www.downloadHandler.text != "0")
            {
                Debug.Log(www.error);
                Debug.Log(www.downloadHandler.text);

                //set input back, go back to start panel, show an error message
                field1.text = "";
                field2.text = "";
                GameObject.Find("ConfirmL").GetComponent<Button>().interactable = false;
                ErrorL.GetComponentInChildren<Text>().text = "Anmeldung fehlgeschlagen!";
                ErrorL.GetComponentInChildren<Text>().color = new Color(1, 0, 0, 1);
                yield return new WaitForSeconds(4.0f);
                ErrorL.SetActive(true);
                LogPanel.SetActive(false);
                StartButtons.SetActive(true);
            }
            else
            {
                Debug.Log("Login complete! " + www.downloadHandler.text);
                ErrorL.SetActive(false);
                //anmelden here
                p.Logname = field1.text;
                LogPanel.SetActive(false);
                welcome.text = "Willkommen ".ToUpper() + p.Logname.ToUpper() + "!";
                welcome.gameObject.SetActive(true);
                p.StartCoroutine(p.LoadGameData());
                Invoke("WaitOnce", 2);
            }
        }
    }

    IEnumerator MoveBigTitleIn(){ 
		
		title.gameObject.SetActive (true);
		title.GetComponent<Animator> ().Play ("LooneyToonTitle");
        yield return new WaitForSeconds(1.0f);
        press.gameObject.SetActive(true);
        press.GetComponent<Animator>().Play("LooneyToonTitle");
        wait = false;
	}

	IEnumerator LoadEffect(){

		float ratio = 0;
		float inc = 1.0f / 50;

        GameObject pref = Resources.Load<GameObject>("Prefabs/FX/MagicalSource");
        GameObject particles = Instantiate(pref);
        particles.transform.position = new Vector3(0, -1, 1);
        particles.transform.localScale = new Vector3(0.3f, 0.3f, 1);
        particles.SetActive(true);

        ParticleSystem ps = particles.GetComponent<ParticleSystem>();

        if (ps != null)
        {
            ps.GetComponent<Renderer>().sortingLayerName = "Players";
            ps.GetComponent<Renderer>().sortingOrder = 1;
        }
        while (ratio <= 1.0f) {
			loadbar.rectTransform.localScale = new Vector3(ratio, 1.0f, 1.0f);
			ratio += inc;
			yield return new WaitForSeconds(Time.deltaTime);
		}
		loadbar.rectTransform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        SceneManager.LoadScene("MAWorld");
    }
}

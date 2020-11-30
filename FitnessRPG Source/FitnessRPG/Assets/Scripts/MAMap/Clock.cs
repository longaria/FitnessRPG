using System;
using UnityEngine;
using UnityEngine.UI;

public class Clock : MonoBehaviour {

    public Text t;
    string hour, minute;

	// Use this for initialization
	void Start ()
    {
        UpdateClock();
	}
	   
    void UpdateClock()
    {
        DateTime n = DateTime.Now;
        hour = n.Hour.ToString();
        minute = n.Minute < 10 ? "0" + n.Minute.ToString() : n.Minute.ToString();
        t.text = hour + ":" + minute + " h";

        Invoke("UpdateClock", 60);
    }
}

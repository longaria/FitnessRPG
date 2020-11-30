using UnityEngine;

public class TriggerScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("trigger script called");
        PDProfile o = GameObject.Find("PDProfile").GetComponent<PDProfile>();
        if (o)
        {
            if(!o.ping) o.pingAgain();
        }
    }
}

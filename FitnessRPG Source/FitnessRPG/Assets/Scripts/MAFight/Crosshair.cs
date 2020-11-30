using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour {

	private bool activated;
	private MAFightManager manager;
	// Use this for initialization

	void Awake(){

		manager = GameObject.FindGameObjectWithTag ("myManager").GetComponent<MAFightManager> ();
	}

	void Start () {
		activated = false;
	}

	//starts and stops animation of crosshair on click
	public void AnimationOnClick(){
		
		activated = !activated;

		if (activated) {

            manager.Paused = true;
			GetComponent<Animator> ().Play ("crossH");

		} else {
			
			GetComponent<Animator> ().Play ("idle");
			manager.Paused = false;
            manager.CrosshairActive = false;
			float critDmgMultiplier = 1.0f / GetComponent<RectTransform> ().localScale.x;
			int dmg = GameObject.FindGameObjectWithTag ("myPlayer").GetComponent<Player> ().CalcDmg();
			dmg = Mathf.CeilToInt (critDmgMultiplier * dmg);
            GameObject e = GameObject.FindGameObjectWithTag("myEnemy");
            if (e)
            {
                e.GetComponent<Enemy>().UpdateHealth(dmg);
            }
            else
            {
                return;
            }
            Vector3 p = manager.enemy.transform.position;
            manager.SpawnParticle(new Vector3(p.x, p.y + 3, p.z), "Blood");
			Destroy (gameObject);
		}
	}
}

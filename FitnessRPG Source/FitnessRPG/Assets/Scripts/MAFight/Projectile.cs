using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

	// Use this for initialization
	private int thrower;
	public float rotationSpeed;
	public float velocity;
	private MAFightManager manager;

	void Awake()
    {
		manager = GameObject.FindGameObjectWithTag ("myManager").GetComponent<MAFightManager>();
	}

	void Start()
    {
		velocity = 5; //Random.Range (2, 10);
		rotationSpeed = 20;
	}
	// Update is called once per frame
	void Update ()
    {
        if (!manager.Paused)
        {
            if (thrower == 1)
            { 
                //player throws
                //transform.Translate(Vector3.right * Time.deltaTime * velocity, Space.World);
                transform.Rotate(Vector3.forward * rotationSpeed, Space.World);

            }
            if (thrower == 2)
            {
                //enemy throws right to left
                //transform.Translate(Vector3.left * Time.deltaTime * velocity, Space.World);
                transform.Rotate(Vector3.back * rotationSpeed, Space.World);
            }
        }
    }
    //TODO: so far assumes projectile is regular attack, not special item thrown
	void OnTriggerEnter2D(Collider2D col)
    {
		//work around out of necessity, otherwise they destroy themselves on spawn - there's probably a better solution
		if (col.gameObject.tag == "myPlayer" && (thrower == 2))
        {
            Enemy e = GameObject.FindGameObjectWithTag("myEnemy").GetComponent<Enemy>();
            int dmg = e.CalcDmg();
            if (dmg > 0)
            {
                col.gameObject.GetComponent<Player>().UpdateHealth(dmg);
                manager.SpawnParticle(transform.position, "Boom");
            }
            else
            {
                manager.SpawnParticle(transform.position, "Poof");
            }
            Destroy(gameObject);
		}
		else if (col.gameObject.tag == "myEnemy" && (thrower == 1))
        {
            //actually player throws a throwable item, so rework, what it should do
            Player p = GameObject.FindGameObjectWithTag("myPlayer").GetComponent<Player>();
            int dmg = p.CalcDmg();
            if (dmg > 0)
            {
                if (!manager.CrosshairActive) p.IncreaseHitCounter();
                col.gameObject.GetComponent<Enemy>().UpdateHealth(dmg);
                manager.SpawnParticle(transform.position, "Boom");
            }
            else
            {
                manager.SpawnParticle(transform.position, "Poof");
            }
            Destroy(gameObject);
		}
		else if (col.gameObject.tag == "projectile")
        {
			Physics2D.IgnoreCollision (GetComponent<Collider2D> (), col);
		}
	}

	public void SetThrower(int i)
    {
		thrower = i;
	}

	public int GetThrower()
    {
		return thrower;
	}
}

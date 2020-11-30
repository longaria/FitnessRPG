using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProjectileItem : MonoBehaviour
{
        // Use this for initialization
    private int thrower;
    public float rotationSpeed;
    public float velocity;
    private MAFightManager manager;

    void Awake()
    {
        manager = GameObject.FindGameObjectWithTag("myManager").GetComponent<MAFightManager>();
    }

    void Start()
    {
        velocity = 5; //Random.Range (2, 10);
        rotationSpeed = 20;
    }
    // Update is called once per frame
    void Update()
    {
        if (!manager.Paused)
        {
            transform.Rotate(Vector3.forward * rotationSpeed, Space.World);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        //work around out of necessity, otherwise they destroy themselves on spawn - there's probably a better solution
        if (col.gameObject.tag == "myEnemy" )
        {
            //actually player throws a throwable item, so rework, what it should do
            Profile.GameData.Item item = manager.p.playerData.equipment[2];
            manager.UpdateStats(item, false, item.Duration, 2);
            manager.enemy.GetComponent<Enemy>().DisplayFCTMsg(item.Name + "!");
            if (item.Duration > 0) manager.StartCoroutine(manager.ItemDuration(item.Duration, 2));
            Destroy(gameObject);
        }
        else
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), col);
        }
    }
}

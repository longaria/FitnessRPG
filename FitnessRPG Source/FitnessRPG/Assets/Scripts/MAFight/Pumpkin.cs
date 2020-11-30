using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pumpkin : Enemy
{
    public bool jumping, back, ulting;
    Vector3 oldPos;
    Quaternion oldRot;
    public int UltHitCounter { get; set; }

    new void Awake()
    {
        base.Awake();
    }

    new void Start()
    {
        base.Start();
        jumping = false;
        back = false;
        UltHitCounter = 3;
        ulting = false;
        oldPos = transform.position;
        oldRot = transform.rotation;
    }

    new void Update()
    {
        base.Update();

        if (back)      //set to true after attack finished by attack behaviour
        {
            if (transform.position.x < oldPos.x)
            {
                transform.Translate(Vector3.right * Time.deltaTime * 10, Space.World);
            }
            else
            {
                transform.position = oldPos;
                transform.localRotation = oldRot;
                back = false;
                anim.SetBool("Run", false);
                xScale = 0.0f;
                attackCD.localScale = new Vector3(xScale, 1.0f, 1.0f);
                attackDelay = manager.enemyCopy.EnemyStats["AttackSpeed"];
                attackCDText.text = "Angriff in:\t" + attackDelay.ToString("F2") + " Sek.";
                done = true;
            }
        }
        else if (!manager.Paused && !manager.End && ulting)
        {
            if (xScale < 1)
            {
                xScale += (Time.deltaTime / manager.enemyCopy.EnemyStats["AttackSpeed"]);
                ultBarCD.localScale = new Vector3(xScale, 1.0f, 1.0f);
                attackDelay -= Time.deltaTime;
                ultCDText.text = "Spezialattacke" + " in:\t" + attackDelay.ToString("F2") + " Sek.";
            }
            else
            {
                done = false;
                ultBarCD.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                ultCDText.text = "Spezialattacke" + " in:\t" + "0 Sek.";
                xScale = 0;
                ultBarCD.localScale = new Vector3(xScale, 1.0f, 1.0f);
                ultCDText.text = "Spezialattacke" + " in:\t" + attackDelay.ToString("F2") + " Sek.";
                attackDelay = manager.enemyCopy.EnemyStats["AttackSpeed"];
                UltimateCatAttack();
            }
        }
    }

    public void Turn()
    {
        transform.localRotation = Quaternion.Euler(0, 0, 0);
    }
    //called when attack bar is filled
    override protected void Attack()
    {
        
        if (UltHitCounter > 0)
        {
            UltHitCounter--;                        //so far the hit counter reduces independent of a hit, which is intended
            jumping = true;
            GetComponent<Rigidbody2D>().AddForce(new Vector2(-1, 1) * 50);
            anim.SetBool("Attack", true);
            //done = true;
        }
        else
        {
            attackCD.gameObject.SetActive(false);
            attackCDText.gameObject.SetActive(false);
            ultBar.SetActive(true);
            xScale = 0;             //redundant, but for clarity
            ultBarCD.localScale = new Vector3(xScale, 1.0f, 1.0f);
            ultCDText.text = "Spezialattacke";
            ulting = true;
        }
    }
    //called when counter hits zero, some special attack
    void UltimateCatAttack()
    {
        anim.SetBool("Attack", true);
        attackCD.gameObject.SetActive(true);
        attackCDText.gameObject.SetActive(true);
        xScale = 0;             //redundant, but for clarity
        ultBarCD.localScale = new Vector3(xScale, 1.0f, 1.0f);
        ultBar.SetActive(false);
        ulting = false;
        UltHitCounter = 3;
        done = true;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "block" && jumping)
        {
            jumping = false;
            Turn();
            back = true;
            anim.SetBool("Run", true);      //<-----doesnt need on state exit, because it has exit time anyways
        }
    }
}

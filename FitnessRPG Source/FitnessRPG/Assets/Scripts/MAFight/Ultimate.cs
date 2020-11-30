using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ultimate : MonoBehaviour
{
    bool isPlayer;

    void OnTriggerEnter2D(Collider2D collision)
{
        if (collision.gameObject.tag == "block") //"block"
        {
            MAFightManager m = GameObject.FindGameObjectWithTag("myManager").GetComponent<MAFightManager>();
            float r = Random.Range(-1f, 1f);
            float t = Random.Range(-1f, 1f);
            m.SpawnParticle(new Vector3(transform.position.x + r, transform.position.y + 2 + t, transform.position.z), "Boom");
            Destroy(gameObject);
        }
        else
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision);
        }
    }

    public void SetThrower(bool isPlayer)
    {
        this.isPlayer = isPlayer;
    }

    public bool GetThrower()
    {
        return isPlayer;
    }

}

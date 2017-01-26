using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class smiley : MonoBehaviour {

    public int maxDamage = 12, minDamage = 8;

    private void Start()
    {
        Destroy(gameObject, 3f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            int damage = Random.Range(minDamage, maxDamage);
            collision.GetComponent<EnemyStats>().Damage(damage) ;
            
            // Confetti Particles?

            Destroy(gameObject);
        }
    }
}

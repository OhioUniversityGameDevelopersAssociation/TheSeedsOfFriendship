using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackBox : MonoBehaviour {

    public int minDamage = 7, maxDamage = 11;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            other.GetComponent<PlayerStats>().Damage(Random.Range(minDamage, maxDamage));
        }
    }
}

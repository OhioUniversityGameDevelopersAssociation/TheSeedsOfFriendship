using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slice : MonoBehaviour {

    public int maxDamage = 12, minDamage = 8;
    public float knockbackForce = 5;
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            int damage = Random.Range(minDamage, maxDamage);
            collision.GetComponent<EnemyStats>().Damage(damage);
            Vector2 dir = collision.GetComponent<Rigidbody2D>().position - (Vector2)transform.position;
            if (collision.name != "Octonana")
            {
                StrawBearryAI bear;
                // Gator
                if (bear = collision.GetComponent<StrawBearryAI>())
                {
                    bear.knockedBack = true;
                    bear.knockBackTime = 0f;
                }
                StartCoroutine(ApplyKnockback(collision.GetComponent<Rigidbody2D>(), dir));
            }
            // Confetti Particles?
            
        }
    }

    IEnumerator ApplyKnockback(Rigidbody2D rb, Vector2 dir)
    {
        yield return new WaitForSeconds(0.04f);
        rb.AddForce(dir * knockbackForce, ForceMode2D.Impulse);
    }
}
